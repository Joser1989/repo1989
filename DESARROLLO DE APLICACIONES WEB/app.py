from flask import Flask, render_template, redirect, url_for, request

from forms.producto_form import ProductoForm
from forms.cliente_form import ClienteForm
from forms.proveedor_form import ProveedorForm
from forms.facturacion_form import FacturacionForm

app = Flask(__name__)

# SECRET_KEY necesaria para que Flask-WTF genere y valide el token CSRF
# de cada formulario (form.hidden_tag()).
app.config['SECRET_KEY'] = 'alwork-clave-secreta-2026'

# ─── DATOS DEMOSTRATIVOS (sin base de datos por ahora) ───

productos = [
    {"nombre": "Polo Manga Larga", "precio": 14.00, "imagen": "polo1.png",
     "descripcion": "Ideal para equipos industriales y de logística. Tela dry-fit resistente.", "disponible": True},
    {"nombre": "Polo Técnico Manga Corta", "precio": 16.00, "imagen": "polo2.jpg",
     "descripcion": "Corte ergonómico, alta transpirabilidad, ideal para trabajo en campo.", "disponible": True},
    {"nombre": "Polo Abasto 589", "precio": 16.00, "imagen": "polo3.jpg",
     "descripcion": "Diseño moderno con franjas decorativas y bordado de logo incluido.", "disponible": True},
    {"nombre": "Polo Tricolor Racing", "precio": 17.00, "imagen": "polo4.jpg",
     "descripcion": "Tres bloques de color, ideal para uniformes de equipo.", "disponible": True},
    {"nombre": "Polo Bicolor Hard Work", "precio": 19.00, "imagen": "polo5.jpg",
     "descripcion": "Refuerzos en hombros, tejido anti-desgarro.", "disponible": True},
    {"nombre": "Polo Sport Blanco", "precio": 16.00, "imagen": "polo6.png",
     "descripcion": "Elegante y funcional, apto para oficina y eventos corporativos.", "disponible": True},
    {"nombre": "Polo Premium Tricolor", "precio": 18.00, "imagen": "polo7.jpg",
     "descripcion": "Tela piqué de alta gama con bordado o DTF.", "disponible": False},
    {"nombre": "Polo Corporativo Azul Royale", "precio": 17.00, "imagen": "polo8.jpg",
     "descripcion": "Color azul intenso con paneles blancos.", "disponible": True},
]

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
    return dict(empresa=empresa, total_productos=len(productos))

# ─── RUTAS ───

@app.route('/')
def inicio():
    return render_template('index.html', active='inicio')

@app.route('/productos')
def ver_productos():
    return render_template('productos.html', productos=productos, active='productos')

@app.route('/clientes')
def ver_clientes():
    return render_template('clientes.html', clientes=clientes, active='clientes')

@app.route('/proveedores')
def ver_proveedores():
    return render_template('proveedores.html', proveedores=proveedores, active='proveedores')

@app.route('/facturacion')
def ver_facturacion():
    return render_template('facturacion.html', facturas=facturas, active='facturacion')

# ─── RUTAS DE FORMULARIOS (Flask-WTF) ───
# Cada ruta acepta GET (mostrar el formulario) y POST (procesar los datos).
# La misma clase de formulario y la misma plantilla se reutilizan para
# "registrar" (sin id en la URL) y "editar" (con id en la URL).

@app.route('/productos/formulario', methods=['GET', 'POST'])
@app.route('/productos/formulario/<int:producto_id>', methods=['GET', 'POST'])
def formulario_producto(producto_id=None):
    editar = producto_id is not None
    form = ProductoForm(data=productos[producto_id]) if editar else ProductoForm()

    if form.validate_on_submit():
        datos = {
            "nombre": form.nombre.data,
            "precio": float(form.precio.data),
            "imagen": form.imagen.data,
            "descripcion": form.descripcion.data,
            "disponible": form.disponible.data
        }
        if editar:
            productos[producto_id] = datos
        else:
            productos.append(datos)
        return redirect(url_for('ver_productos'))

    return render_template('formulario_producto.html', form=form, editar=editar, active='productos')


@app.route('/clientes/formulario', methods=['GET', 'POST'])
@app.route('/clientes/formulario/<int:cliente_id>', methods=['GET', 'POST'])
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