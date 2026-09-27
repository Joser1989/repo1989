import os

from flask import Flask, render_template, redirect, url_for, request, flash
from flask_login import (
    LoginManager, UserMixin, login_user, logout_user,
    login_required, current_user
)
from werkzeug.security import generate_password_hash, check_password_hash
from psycopg2 import errors as pg_errors

from forms.producto_form import ProductoForm
from forms.cliente_form import ClienteForm
from forms.proveedor_form import ProveedorForm
from forms.facturacion_form import FacturacionForm
from forms.usuario_form import UsuarioForm
from forms.login_form import LoginForm

from conexion.conexion import get_connection, init_db
from models import Usuario

app = Flask(__name__)

# Crea alwork_db y las tablas (sql/esquema.sql) si todavía no existen.
init_db()

# SECRET_KEY necesaria para que Flask-WTF genere y valide el token CSRF
# de cada formulario (form.hidden_tag()) y para firmar la cookie de sesión
# que usa Flask-Login.
app.config['SECRET_KEY'] = 'alwork-clave-secreta-2026'

# ─── CONFIGURACIÓN DE FLASK-LOGIN ───
login_manager = LoginManager()
login_manager.init_app(app)
# Si alguien no autenticado intenta entrar a una ruta con @login_required,
# Flask-Login lo redirige automáticamente a esta vista.
login_manager.login_view = 'login'
login_manager.login_message = 'Debes iniciar sesión para acceder a esta página.'
login_manager.login_message_category = 'warning'


@login_manager.user_loader
def load_user(user_id):
    """Flask-Login llama a esta función en cada request para reconstruir
    el usuario autenticado a partir del id guardado en la sesión."""
    return Usuario.obtener_por_id(user_id)


# ─── DATOS DE LA EMPRESA (diccionario / objeto estructurado) ───

empresa = {
    "nombre": "AL WORK",
    "slogan": "Viste con Identidad, Trabaja con Estilo",
    "telefono": "0980099875",
    "direccion": "Gran Colombia y Paquisha, Quito",
    "anio": 2026
}


# Este context_processor envía "empresa" y "total_productos" a TODAS las
# plantillas automáticamente (incluidos navbar.html y footer.html),
# sin necesidad de repetirlo en cada render_template().
@app.context_processor
def datos_globales():
    conn = get_connection()
    cursor = conn.cursor()
    cursor.execute('SELECT COUNT(*) AS total FROM productos')
    total_productos = cursor.fetchone()['total']
    cursor.close()
    conn.close()
    return dict(empresa=empresa, total_productos=total_productos)


# ─── RUTAS PÚBLICAS ───

@app.route('/')
def inicio():
    return render_template('index.html', active='inicio')


# ─── AUTENTICACIÓN (Semana 14) ───

@app.route('/registro', methods=['GET', 'POST'])
def registro():
    # Si ya hay sesión activa, no tiene sentido volver a registrarse.
    if current_user.is_authenticated:
        return redirect(url_for('dashboard'))

    form = UsuarioForm()
    if form.validate_on_submit():
        password_hash = generate_password_hash(form.password.data)
        try:
            Usuario.crear(form.usuario.data, password_hash)
        except pg_errors.UniqueViolation:
            # Salta si el nombre de usuario ya existe (columna UNIQUE).
            flash('Ese nombre de usuario ya está en uso, elige otro.', 'danger')
            return render_template('registro.html', form=form, active='registro')

        flash('Usuario registrado correctamente. Ya puedes iniciar sesión.', 'success')
        return redirect(url_for('login'))

    return render_template('registro.html', form=form, active='registro')


@app.route('/login', methods=['GET', 'POST'])
def login():
    if current_user.is_authenticated:
        return redirect(url_for('dashboard'))

    form = LoginForm()
    if form.validate_on_submit():
        fila = Usuario.obtener_por_nombre(form.usuario.data)

        # No se compara la contraseña escrita directamente contra la
        # almacenada: siempre se usa check_password_hash().
        if fila is not None and check_password_hash(fila['password'], form.password.data):
            usuario_autenticado = Usuario(fila['id'], fila['usuario'])
            login_user(usuario_autenticado)
            return redirect(url_for('dashboard'))

        flash('Usuario o contraseña incorrectos.', 'danger')

    return render_template('login.html', form=form, active='login')


@app.route('/logout')
@login_required
def logout():
    logout_user()
    flash('Sesión cerrada correctamente.', 'info')
    return redirect(url_for('login'))


@app.route('/dashboard')
@login_required
def dashboard():
    return render_template('dashboard.html', active='dashboard')


# ─── RUTAS PROTEGIDAS (requieren sesión iniciada) ───

@app.route('/productos')
@login_required
def ver_productos():
    conn = get_connection()
    cursor = conn.cursor()
    cursor.execute('''
        SELECT p.*, pr.nombre AS proveedor_nombre
        FROM productos p
        LEFT JOIN proveedores pr ON p.id_proveedor = pr.id_proveedor
        ORDER BY p.id
    ''')
    productos = cursor.fetchall()
    cursor.close()
    conn.close()
    return render_template('productos.html', productos=productos, active='productos')


@app.route('/clientes')
@login_required
def ver_clientes():
    conn = get_connection()
    cursor = conn.cursor()
    cursor.execute('SELECT * FROM clientes ORDER BY id_cliente')
    clientes_db = cursor.fetchall()
    cursor.close()
    conn.close()
    return render_template('clientes.html', clientes=clientes_db, active='clientes')


@app.route('/proveedores')
@login_required
def ver_proveedores():
    conn = get_connection()
    cursor = conn.cursor()
    cursor.execute('SELECT * FROM proveedores ORDER BY id_proveedor')
    proveedores_db = cursor.fetchall()
    cursor.close()
    conn.close()
    return render_template('proveedores.html', proveedores=proveedores_db, active='proveedores')


@app.route('/facturacion')
@login_required
def ver_facturacion():
    conn = get_connection()
    cursor = conn.cursor()
    cursor.execute('''
        SELECT f.*, c.nombre AS cliente_nombre
        FROM facturas f
        LEFT JOIN clientes c ON f.id_cliente = c.id_cliente
        ORDER BY f.id_factura
    ''')
    facturas_db = cursor.fetchall()
    cursor.close()
    conn.close()
    return render_template('facturacion.html', facturas=facturas_db, active='facturacion')


# ─── RUTAS DE FORMULARIOS (Flask-WTF) — protegidas también ───
# Cada ruta acepta GET (mostrar el formulario) y POST (procesar los datos).
# La misma clase de formulario y la misma plantilla se reutilizan para
# "registrar" (sin id en la URL) y "editar" (con id en la URL).

@app.route('/productos/formulario', methods=['GET', 'POST'])
@app.route('/productos/formulario/<int:producto_id>', methods=['GET', 'POST'])
@login_required
def formulario_producto(producto_id=None):
    editar = producto_id is not None
    conn = get_connection()
    cursor = conn.cursor()

    cursor.execute('SELECT id_proveedor, nombre FROM proveedores ORDER BY nombre')
    proveedores_lista = cursor.fetchall()
    opciones_proveedor = [(p['id_proveedor'], p['nombre']) for p in proveedores_lista]

    if editar:
        cursor.execute('SELECT * FROM productos WHERE id = %s', (producto_id,))
        producto_existente = cursor.fetchone()
        form = ProductoForm(data=dict(producto_existente) if producto_existente else None) if request.method == 'GET' else ProductoForm()
    else:
        form = ProductoForm()
        if request.method == 'GET':
            # Por defecto selecciona Lindtex si existe
            lindtex = next((p for p in proveedores_lista if p['nombre'] == 'Lindtex'), None)
            if lindtex:
                form.id_proveedor.data = lindtex['id_proveedor']

    form.id_proveedor.choices = opciones_proveedor

    if form.validate_on_submit():
        if editar:
            cursor.execute(
                '''UPDATE productos
                   SET nombre = %s, precio = %s, imagen = %s, descripcion = %s,
                       disponible = %s, id_proveedor = %s
                   WHERE id = %s''',
                (form.nombre.data, float(form.precio.data), form.imagen.data,
                 form.descripcion.data, int(form.disponible.data),
                 form.id_proveedor.data, producto_id)
            )
        else:
            cursor.execute(
                '''INSERT INTO productos (nombre, precio, imagen, descripcion, disponible, id_proveedor)
                   VALUES (%s, %s, %s, %s, %s, %s)''',
                (form.nombre.data, float(form.precio.data), form.imagen.data,
                 form.descripcion.data, int(form.disponible.data), form.id_proveedor.data)
            )
        conn.commit()
        cursor.close()
        conn.close()
        return redirect(url_for('ver_productos'))

    cursor.close()
    conn.close()
    return render_template('formulario_producto.html', form=form, editar=editar, active='productos')


@app.route('/productos/eliminar/<int:producto_id>', methods=['POST'])
@login_required
def eliminar_producto(producto_id):
    conn = get_connection()
    cursor = conn.cursor()
    cursor.execute('DELETE FROM productos WHERE id = %s', (producto_id,))
    conn.commit()
    cursor.close()
    conn.close()
    return redirect(url_for('ver_productos'))


@app.route('/clientes/formulario', methods=['GET', 'POST'])
@app.route('/clientes/formulario/<int:cliente_id>', methods=['GET', 'POST'])
@login_required
def formulario_cliente(cliente_id=None):
    editar = cliente_id is not None
    conn = get_connection()
    cursor = conn.cursor()

    if editar:
        cursor.execute('SELECT * FROM clientes WHERE id_cliente = %s', (cliente_id,))
        cliente_existente = cursor.fetchone()
        if request.method == 'GET' and cliente_existente:
            datos_iniciales = dict(cliente_existente)
            datos_iniciales['contacto'] = datos_iniciales.get('telefono')
            form = ClienteForm(data=datos_iniciales)
        else:
            form = ClienteForm()
    else:
        form = ClienteForm()

    if form.validate_on_submit():
        if editar:
            cursor.execute(
                'UPDATE clientes SET nombre = %s, tipo = %s, telefono = %s, ciudad = %s WHERE id_cliente = %s',
                (form.nombre.data, form.tipo.data, form.contacto.data, form.ciudad.data, cliente_id)
            )
        else:
            cursor.execute(
                'INSERT INTO clientes (nombre, tipo, telefono, ciudad) VALUES (%s, %s, %s, %s)',
                (form.nombre.data, form.tipo.data, form.contacto.data, form.ciudad.data)
            )
        conn.commit()
        cursor.close()
        conn.close()
        return redirect(url_for('ver_clientes'))

    cursor.close()
    conn.close()
    return render_template('formulario_cliente.html', form=form, editar=editar, active='clientes')


@app.route('/clientes/eliminar/<int:cliente_id>', methods=['POST'])
@login_required
def eliminar_cliente(cliente_id):
    conn = get_connection()
    cursor = conn.cursor()
    cursor.execute('DELETE FROM clientes WHERE id_cliente = %s', (cliente_id,))
    conn.commit()
    cursor.close()
    conn.close()
    return redirect(url_for('ver_clientes'))


@app.route('/proveedores/formulario', methods=['GET', 'POST'])
@app.route('/proveedores/formulario/<int:proveedor_id>', methods=['GET', 'POST'])
@login_required
def formulario_proveedor(proveedor_id=None):
    editar = proveedor_id is not None
    conn = get_connection()
    cursor = conn.cursor()

    if editar:
        cursor.execute('SELECT * FROM proveedores WHERE id_proveedor = %s', (proveedor_id,))
        proveedor_existente = cursor.fetchone()
        if request.method == 'GET' and proveedor_existente:
            datos_iniciales = dict(proveedor_existente)
            datos_iniciales['contacto'] = datos_iniciales.get('telefono')
            form = ProveedorForm(data=datos_iniciales)
        else:
            form = ProveedorForm()
    else:
        form = ProveedorForm()

    if form.validate_on_submit():
        if editar:
            cursor.execute(
                'UPDATE proveedores SET nombre = %s, tipo = %s, telefono = %s, ciudad = %s WHERE id_proveedor = %s',
                (form.nombre.data, form.tipo.data, form.contacto.data, form.ciudad.data, proveedor_id)
            )
        else:
            cursor.execute(
                'INSERT INTO proveedores (nombre, tipo, telefono, ciudad) VALUES (%s, %s, %s, %s)',
                (form.nombre.data, form.tipo.data, form.contacto.data, form.ciudad.data)
            )
        conn.commit()
        cursor.close()
        conn.close()
        return redirect(url_for('ver_proveedores'))

    cursor.close()
    conn.close()
    return render_template('formulario_proveedor.html', form=form, editar=editar, active='proveedores')


@app.route('/proveedores/eliminar/<int:proveedor_id>', methods=['POST'])
@login_required
def eliminar_proveedor(proveedor_id):
    conn = get_connection()
    cursor = conn.cursor()
    cursor.execute('DELETE FROM proveedores WHERE id_proveedor = %s', (proveedor_id,))
    conn.commit()
    cursor.close()
    conn.close()
    return redirect(url_for('ver_proveedores'))


@app.route('/facturacion/formulario', methods=['GET', 'POST'])
@app.route('/facturacion/formulario/<int:factura_id>', methods=['GET', 'POST'])
@login_required
def formulario_facturacion(factura_id=None):
    editar = factura_id is not None
    conn = get_connection()
    cursor = conn.cursor()

    cursor.execute('SELECT id_cliente, nombre FROM clientes ORDER BY nombre')
    clientes_lista = cursor.fetchall()
    opciones_cliente = [(c['id_cliente'], c['nombre']) for c in clientes_lista]

    if editar:
        cursor.execute('SELECT * FROM facturas WHERE id_factura = %s', (factura_id,))
        factura_existente = cursor.fetchone()
        if request.method == 'GET' and factura_existente:
            datos_iniciales = dict(factura_existente)
            datos_iniciales['prenda'] = datos_iniciales.get('detalle')
            form = FacturacionForm(data=datos_iniciales)
        else:
            form = FacturacionForm()
    else:
        form = FacturacionForm()

    form.id_cliente.choices = opciones_cliente

    if form.validate_on_submit():
        if editar:
            cursor.execute(
                '''UPDATE facturas
                   SET numero = %s, id_cliente = %s, detalle = %s, total = %s, fecha = %s
                   WHERE id_factura = %s''',
                (form.numero.data, form.id_cliente.data, form.prenda.data,
                 float(form.total.data), form.fecha.data, factura_id)
            )
        else:
            cursor.execute(
                '''INSERT INTO facturas (numero, id_cliente, detalle, total, fecha)
                   VALUES (%s, %s, %s, %s, %s)''',
                (form.numero.data, form.id_cliente.data, form.prenda.data,
                 float(form.total.data), form.fecha.data)
            )
        conn.commit()
        cursor.close()
        conn.close()
        return redirect(url_for('ver_facturacion'))

    cursor.close()
    conn.close()
    return render_template('formulario_facturacion.html', form=form, editar=editar, active='facturacion')


@app.route('/facturacion/eliminar/<int:factura_id>', methods=['POST'])
@login_required
def eliminar_factura(factura_id):
    conn = get_connection()
    cursor = conn.cursor()
    cursor.execute('DELETE FROM facturas WHERE id_factura = %s', (factura_id,))
    conn.commit()
    cursor.close()
    conn.close()
    return redirect(url_for('ver_facturacion'))


if __name__ == '__main__':
    app.run(debug=True)