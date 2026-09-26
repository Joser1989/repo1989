import os
import sqlite3

from flask import Flask, render_template, redirect, url_for, request, flash
from flask_login import (
    LoginManager, UserMixin, login_user, logout_user,
    login_required, current_user
)
from werkzeug.security import generate_password_hash, check_password_hash

from forms.producto_form import ProductoForm
from forms.cliente_form import ClienteForm
from forms.proveedor_form import ProveedorForm
from forms.facturacion_form import FacturacionForm
from forms.usuario_form import UsuarioForm
from forms.login_form import LoginForm

from conexion.conexion import get_connection, init_db
from models import Usuario

app = Flask(__name__)

# Crea data/alwork.db y las tablas (sql/esquema.sql) si todavía no existen.
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


# ─── DATOS DEMOSTRATIVOS (módulos que aún no requieren persistencia esta semana) ───

clientes = [
    {"nombre": "Mecánica Torres", "tipo": "Mecánica automotriz", "contacto": "0991234567", "ciudad": "Quito"},
    {"nombre": "MotoExpress Llano Chico", "tipo": "Mecánica de motos", "contacto": "0987654321", "ciudad": "Quito"},
    {"nombre": "Restaurante El Fogón", "tipo": "Restaurante", "contacto": "0998765432", "ciudad": "Quito"},
    {"nombre": "Taller Automotriz Rivera", "tipo": "Mecánica automotriz", "contacto": "0976543210", "ciudad": "Quito"},
]

proveedores = [
    {"nombre": "Almacenes José Puebla", "tipo": "Mayorista de telas", "contacto": "022345678", "ciudad": "Quito"},
    {"nombre": "Lindtex", "tipo": "Mayorista de telas", "contacto": "022987654", "ciudad": "Quito"},
]

facturas = [
    {"numero": "001", "cliente": "Mecánica Torres", "prenda": "Polo racing (x15)", "total": 255.00, "fecha": "10/08/2026"},
    {"numero": "002", "cliente": "Restaurante El Fogón", "prenda": "Mandiles (x8)", "total": 96.00, "fecha": "12/08/2026"},
    {"numero": "003", "cliente": "MotoExpress Llano Chico", "prenda": "Camisa racing (x10)", "total": 170.00, "fecha": "13/08/2026"},
]

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
    cursor.execute('SELECT COUNT(*) FROM productos')
    total_productos = cursor.fetchone()[0]
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
        except sqlite3.IntegrityError:
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
    return render_template('clientes.html', clientes=clientes, active='clientes')


@app.route('/proveedores')
@login_required
def ver_proveedores():
    return render_template('proveedores.html', proveedores=proveedores, active='proveedores')


@app.route('/facturacion')
@login_required
def ver_facturacion():
    return render_template('facturacion.html', facturas=facturas, active='facturacion')


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

    if editar:
        cursor.execute('SELECT * FROM productos WHERE id = ?', (producto_id,))
        producto_existente = cursor.fetchone()
        form = ProductoForm(data=dict(producto_existente) if producto_existente else None) if request.method == 'GET' else ProductoForm()
    else:
        form = ProductoForm()

    if form.validate_on_submit():
        if editar:
            cursor.execute(
                'UPDATE productos SET nombre = ?, precio = ?, imagen = ?, descripcion = ?, disponible = ? WHERE id = ?',
                (form.nombre.data, float(form.precio.data), form.imagen.data,
                 form.descripcion.data, int(form.disponible.data), producto_id)
            )
        else:
            cursor.execute(
                'INSERT INTO productos (nombre, precio, imagen, descripcion, disponible) VALUES (?, ?, ?, ?, ?)',
                (form.nombre.data, float(form.precio.data), form.imagen.data,
                 form.descripcion.data, int(form.disponible.data))
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
    cursor.execute('DELETE FROM productos WHERE id = ?', (producto_id,))
    conn.commit()
    cursor.close()
    conn.close()
    return redirect(url_for('ver_productos'))


@app.route('/clientes/formulario', methods=['GET', 'POST'])
@app.route('/clientes/formulario/<int:cliente_id>', methods=['GET', 'POST'])
@login_required
def formulario_cliente(cliente_id=None):
    editar = cliente_id is not None
    form = ClienteForm(data=clientes[cliente_id]) if editar else ClienteForm()

    if form.validate_on_submit():
        datos = {
            "nombre": form.nombre.data,
            "tipo": form.tipo.data,
            "contacto": form.contacto.data,
            "ciudad": form.ciudad.data
        }
        if editar:
            clientes[cliente_id] = datos
        else:
            clientes.append(datos)
        return redirect(url_for('ver_clientes'))

    return render_template('formulario_cliente.html', form=form, editar=editar, active='clientes')


@app.route('/proveedores/formulario', methods=['GET', 'POST'])
@app.route('/proveedores/formulario/<int:proveedor_id>', methods=['GET', 'POST'])
@login_required
def formulario_proveedor(proveedor_id=None):
    editar = proveedor_id is not None
    form = ProveedorForm(data=proveedores[proveedor_id]) if editar else ProveedorForm()

    if form.validate_on_submit():
        datos = {
            "nombre": form.nombre.data,
            "tipo": form.tipo.data,
            "contacto": form.contacto.data,
            "ciudad": form.ciudad.data
        }
        if editar:
            proveedores[proveedor_id] = datos
        else:
            proveedores.append(datos)
        return redirect(url_for('ver_proveedores'))

    return render_template('formulario_proveedor.html', form=form, editar=editar, active='proveedores')


@app.route('/facturacion/formulario', methods=['GET', 'POST'])
@app.route('/facturacion/formulario/<int:factura_id>', methods=['GET', 'POST'])
@login_required
def formulario_facturacion(factura_id=None):
    editar = factura_id is not None
    form = FacturacionForm(data=facturas[factura_id]) if editar else FacturacionForm()

    if form.validate_on_submit():
        datos = {
            "numero": form.numero.data,
            "cliente": form.cliente.data,
            "prenda": form.prenda.data,
            "total": float(form.total.data),
            "fecha": form.fecha.data
        }
        if editar:
            facturas[factura_id] = datos
        else:
            facturas.append(datos)
        return redirect(url_for('ver_facturacion'))

    return render_template('formulario_facturacion.html', form=form, editar=editar, active='facturacion')


if __name__ == '__main__':
    app.run(debug=True)
