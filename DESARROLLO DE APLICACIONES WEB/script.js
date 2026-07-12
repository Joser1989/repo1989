/* ================================================================
   AL WORK — script.js
   Semana 6: Validaciones dinámicas del Cotizador
   Semana 7: Renderizado de plantillas con fetch() + templates/
   ================================================================ */

/* ================================================================
   DATOS GLOBALES
   Estos datos viven mientras la pestaña esté abierta, sin importar
   qué sección se esté mostrando en <main id="contenido">.
   ================================================================ */
let pedidos = [];
let contadorId = 1;

const CONFIG = {
  nombreMinLength: 3,
  descMinLength: 10
};

// Array de productos — antes eran 8 tarjetas escritas a mano en el HTML.
const productos = [
  { id: 1, nombre: 'Polo Manga Larga',            precio: 14.00, imagen: 'img/polo1.png', descripcion: 'Ideal para equipos industriales y de logística. Tela dry-fit resistente.',            disponible: true },
  { id: 2, nombre: 'Polo Técnico Manga Corta',     precio: 16.00, imagen: 'img/polo2.jpg', descripcion: 'Corte ergonómico, alta transpirabilidad, perfecto para trabajo en campo.',            disponible: true },
  { id: 3, nombre: 'Polo Abasto 589',              precio: 16.00, imagen: 'img/polo3.jpg', descripcion: 'Diseño moderno con franjas decorativas y bordado de logo incluido.',                  disponible: true },
  { id: 4, nombre: 'Polo Tricolor Racing',         precio: 17.00, imagen: 'img/polo4.jpg', descripcion: 'Tres bloques de color, ideal para uniformes Team Abasto.',                             disponible: true },
  { id: 5, nombre: 'Polo Bicolor hard work',       precio: 19.00, imagen: 'img/polo5.jpg', descripcion: 'Refuerzos en hombros, tejido anti-desgarro para entornos exigentes.',                  disponible: true },
  { id: 6, nombre: 'Polo Sport Blanco',            precio: 16.00, imagen: 'img/polo6.png', descripcion: 'Elegante y funcional, apto para oficina y eventos corporativos.',                      disponible: true },
  { id: 7, nombre: 'Polo Premium Tricolor',        precio: 18.00, imagen: 'img/polo7.jpg', descripcion: 'Diseño exclusivo azul, gris y negro. Tela piqué de alta gama con bordado o DTF.',       disponible: false },
  { id: 8, nombre: 'Polo Corporativo Azul Royale', precio: 17.00, imagen: 'img/polo8.jpg', descripcion: 'Color azul intenso con paneles blancos. Ideal para uniformes de team.',                 disponible: true }
];

const sonidoExito = new Audio('img/audiologoALWORK.mp3');

/* ================================================================
   🧩 SEMANA 7 — CARGA DE PLANTILLAS CON fetch()
   El contenedor #contenido es el único que cambia. header, nav y
   footer quedan fijos en index.html y nunca se vuelven a pedir.
   ================================================================ */
const contenedorPrincipal = document.getElementById('contenido');

function cargarSeccion(nombre) {
  fetch('templates/' + nombre + '.html')
    .then(function (respuesta) {
      if (!respuesta.ok) throw new Error('No se encontró templates/' + nombre + '.html');
      return respuesta.text();
    })
    .then(function (html) {
      contenedorPrincipal.innerHTML = html;
      inicializarSeccion(nombre);
      marcarLinkActivo(nombre);
      window.scrollTo({ top: 0, behavior: 'smooth' });
    })
    .catch(function (error) {
      contenedorPrincipal.innerHTML = '<p class="text-center text-muted py-5">No se pudo cargar el contenido. Verifica que Live Server esté activo.</p>';
      console.error('Error al cargar sección:', error);
    });
}

// Después de insertar el fragmento, se "conectan" sus scripts,
// porque esos elementos no existían en el DOM hasta este momento.
function inicializarSeccion(nombre) {
  if (nombre === 'productos') inicializarProductos();
  if (nombre === 'cotizador') inicializarCotizador();
}

// Marca visualmente el link activo del nav (antes se hacía con scroll)
function marcarLinkActivo(nombre) {
  document.querySelectorAll('.navbar-alwork .nav-link').forEach(function (link) {
    link.classList.toggle('active', link.getAttribute('data-seccion') === nombre);
  });
}

// Conecta los clics del nav (y de cualquier link con data-seccion,
// como los botones del hero) a cargarSeccion(), usando delegación
// de eventos para que funcione también con contenido inyectado.
document.addEventListener('click', function (e) {
  const link = e.target.closest('[data-seccion]');
  if (!link) return;
  e.preventDefault();
  cargarSeccion(link.getAttribute('data-seccion'));

  // Si el menú móvil está abierto, lo cierra al navegar
  const menu = document.getElementById('menuPrincipal');
  if (menu && menu.classList.contains('show') && window.bootstrap) {
    const bsCollapse = window.bootstrap.Collapse.getInstance(menu) || new window.bootstrap.Collapse(menu);
    bsCollapse.hide();
  }
});

// Carga la sección "inicio" al abrir la página
document.addEventListener('DOMContentLoaded', function () {
  cargarSeccion('inicio');
});

/* ================================================================
   PRODUCTOS — inicialización (se llama tras cargar templates/productos.html)
   ================================================================ */
function inicializarProductos() {
  const listaProductos      = document.getElementById('listaProductos');
  const mensajeSinProductos = document.getElementById('mensajeSinProductos');

  function getBadgeDisponibilidad() {
    return '';
  }

  function crearTarjetaProducto(producto) {
    const col = document.createElement('div');
    col.classList.add('col-sm-6', 'col-md-4', 'col-lg-3');

    const card = document.createElement('div');
    card.classList.add('card', 'prod-card-aw');

    const img = document.createElement('img');
    img.classList.add('card-img-top');
    img.src = producto.imagen;
    img.alt = producto.nombre;

    const cardBody = document.createElement('div');
    cardBody.classList.add('card-body');

    const titulo = document.createElement('h5');
    titulo.classList.add('card-title');
    titulo.textContent = producto.nombre;

    const desc = document.createElement('p');
    desc.classList.add('card-text');
    desc.textContent = producto.descripcion;

    cardBody.appendChild(titulo);
    cardBody.appendChild(desc);

    // ── CONDICIÓN: según el estado del dato "disponible" ────────
    if (producto.disponible) {
      const precio = document.createElement('p');
      precio.classList.add('precio-tag');
      precio.textContent = '$' + producto.precio.toFixed(2);
      cardBody.appendChild(precio);
    } else {
      const agotado = document.createElement('p');
      agotado.classList.add('precio-tag');
      agotado.style.color = '#dc3545';
      agotado.textContent = 'Agotado temporalmente';
      cardBody.appendChild(agotado);
    }

    card.appendChild(img);
    card.appendChild(cardBody);
    col.appendChild(card);
    return col;
  }

  // Recorre el arreglo (ESTRUCTURA REPETITIVA) y renderiza cada tarjeta
  function renderizarProductos() {
    listaProductos.innerHTML = '';

    if (productos.length === 0) {
      mensajeSinProductos.style.display = 'block';
      return;
    }
    mensajeSinProductos.style.display = 'none';

    productos.forEach(function (producto) {
      listaProductos.appendChild(crearTarjetaProducto(producto));
    });
  }

  renderizarProductos();
}

/* ================================================================
   COTIZADOR — inicialización (se llama tras cargar templates/cotizador.html)
   Contiene TODA la lógica de validación de la Semana 6, intacta,
   ahora dentro de una función para poder "re-conectarla" cada vez
   que el fragmento se vuelve a insertar en el DOM.
   ================================================================ */
function inicializarCotizador() {
  const formulario       = document.getElementById('formCotizacion');
  const inputNombre      = document.getElementById('cotNombre');
  const inputDesc        = document.getElementById('cotDescripcion');
  const selectCategoria  = document.getElementById('cotCategoria');
  const listaRegistros   = document.getElementById('listaRegistros');
  const contadorTotal    = document.getElementById('contadorTotal');
  const mensajeVacio     = document.getElementById('mensajeVacio');
  const alertaCotizacion = document.getElementById('alertaCotizacion');

  function actualizarContador() {
    contadorTotal.textContent = pedidos.length;
  }

  // ── CONDICIÓN: muestra/oculta mensaje según el estado del array ──
  function actualizarMensajeVacio() {
    mensajeVacio.style.display = pedidos.length === 0 ? 'block' : 'none';
  }

  function mostrarError(campo, mensaje) {
    campo.classList.remove('is-valid');
    campo.classList.add('is-invalid');
    let feedback = campo.parentNode.querySelector('.invalid-feedback');
    if (!feedback) {
      feedback = document.createElement('div');
      feedback.classList.add('invalid-feedback');
      campo.parentNode.appendChild(feedback);
    }
    feedback.textContent = mensaje;
  }

  function mostrarExito(campo, mensaje) {
    campo.classList.remove('is-invalid');
    campo.classList.add('is-valid');
    let feedback = campo.parentNode.querySelector('.valid-feedback');
    if (!feedback) {
      feedback = document.createElement('div');
      feedback.classList.add('valid-feedback');
      campo.parentNode.appendChild(feedback);
    }
    feedback.textContent = mensaje;
  }

  function limpiarEstado(campo) {
    campo.classList.remove('is-invalid', 'is-valid');
  }

  function limpiarValidaciones() {
    [inputNombre, inputDesc, selectCategoria].forEach(limpiarEstado);
  }

  function reproducirSonidoExito() {
    try {
      sonidoExito.currentTime = 0;
      sonidoExito.play().catch(function (error) {
        console.warn('No se pudo reproducir el sonido de éxito:', error);
      });
    } catch (error) {
      console.warn('No se pudo reproducir el sonido de éxito:', error);
    }
  }

  function mostrarAlerta(tipo, mensaje) {
    alertaCotizacion.classList.remove('d-none', 'alert-success', 'alert-danger');
    alertaCotizacion.classList.add(tipo === 'exito' ? 'alert-success' : 'alert-danger');
    alertaCotizacion.textContent = mensaje;
    clearTimeout(alertaCotizacion._timeoutId);
    alertaCotizacion._timeoutId = setTimeout(function () {
      alertaCotizacion.classList.add('d-none');
    }, 4000);
  }

  function validarNombre() {
    const valor = inputNombre.value.trim();
    if (valor === '') {
      mostrarError(inputNombre, 'El nombre del cliente es obligatorio.');
      return false;
    }
    if (valor.length < CONFIG.nombreMinLength) {
      mostrarError(inputNombre, 'Debe tener al menos ' + CONFIG.nombreMinLength + ' caracteres.');
      return false;
    }
    mostrarExito(inputNombre, 'Nombre válido.');
    return true;
  }

  function validarDescripcion() {
    const valor = inputDesc.value.trim();
    if (valor === '') {
      mostrarError(inputDesc, 'La descripción del pedido es obligatoria.');
      return false;
    }
    if (valor.length < CONFIG.descMinLength) {
      mostrarError(inputDesc, 'Agrega más detalle (mínimo ' + CONFIG.descMinLength + ' caracteres).');
      return false;
    }
    mostrarExito(inputDesc, 'Descripción válida.');
    return true;
  }

  function validarCategoria() {
    if (selectCategoria.value === '') {
      mostrarError(selectCategoria, 'Selecciona un tipo de prenda.');
      return false;
    }
    mostrarExito(selectCategoria, 'Categoría seleccionada.');
    return true;
  }

  function validarFormulario() {
    const nombreOk    = validarNombre();
    const descOk      = validarDescripcion();
    const categoriaOk = validarCategoria();
    return nombreOk && descOk && categoriaOk;
  }

  inputNombre.addEventListener('input', function () {
    if (inputNombre.classList.contains('is-invalid')) validarNombre();
  });
  inputNombre.addEventListener('blur', validarNombre);

  inputDesc.addEventListener('input', function () {
    if (inputDesc.classList.contains('is-invalid')) validarDescripcion();
  });
  inputDesc.addEventListener('blur', validarDescripcion);

  selectCategoria.addEventListener('change', validarCategoria);
  selectCategoria.addEventListener('blur', validarCategoria);

  function getBadgeCategoria(categoria) {
    const colores = {
      'Polo Corporativo':     'bg-primary',
      'Polo Industrial':      'bg-warning text-dark',
      'Chaleco de Seguridad': 'bg-danger',
      'Camisa de Trabajo':    'bg-success',
      'Uniforme Completo':    'bg-dark',
      'Personalizado':        'bg-secondary'
    };
    return colores[categoria] || 'bg-secondary';
  }

  function crearTarjetaPedido(pedido) {
    const col = document.createElement('div');
    col.classList.add('col-md-6', 'col-lg-4');
    col.setAttribute('id', 'pedido-' + pedido.id);

    const card = document.createElement('div');
    card.classList.add('card', 'pedido-card', 'h-100', 'shadow-sm');

    const cardBody = document.createElement('div');
    cardBody.classList.add('card-body');

    const badge = document.createElement('span');
    badge.classList.add('badge', 'mb-2');
    getBadgeCategoria(pedido.categoria).split(' ').forEach(function (clase) {
      if (clase) badge.classList.add(clase);
    });
    badge.textContent = pedido.categoria;

    const numeroPedido = document.createElement('small');
    numeroPedido.classList.add('text-muted', 'd-block', 'mb-1');
    numeroPedido.textContent = '# Pedido ' + String(pedido.id).padStart(3, '0');

    const titulo = document.createElement('h5');
    titulo.classList.add('card-title', 'pedido-nombre');
    titulo.textContent = pedido.nombre;

    const descripcion = document.createElement('p');
    descripcion.classList.add('card-text', 'pedido-desc');
    descripcion.textContent = pedido.descripcion;

    const fecha = document.createElement('small');
    fecha.classList.add('text-muted');
    fecha.textContent = '📅 ' + pedido.fecha;

    const btnEliminar = document.createElement('button');
    btnEliminar.classList.add('btn', 'btn-outline-danger', 'btn-sm', 'mt-3', 'w-100', 'btn-eliminar');
    btnEliminar.textContent = '🗑 Eliminar Pedido';
    btnEliminar.setAttribute('data-id', pedido.id);

    btnEliminar.addEventListener('click', function () {
      eliminarPedido(pedido.id);
    });

    cardBody.appendChild(numeroPedido);
    cardBody.appendChild(badge);
    cardBody.appendChild(titulo);
    cardBody.appendChild(descripcion);
    cardBody.appendChild(fecha);
    cardBody.appendChild(btnEliminar);
    card.appendChild(cardBody);
    col.appendChild(card);
    return col;
  }

  function agregarPedido(nombre, descripcion, categoria) {
    const ahora = new Date();
    const fechaFormateada = ahora.toLocaleDateString('es-EC', {
      day: '2-digit', month: '2-digit', year: 'numeric',
      hour: '2-digit', minute: '2-digit'
    });

    const nuevoPedido = {
      id: contadorId++,
      nombre: nombre.trim(),
      descripcion: descripcion.trim(),
      categoria: categoria,
      fecha: fechaFormateada
    };

    pedidos.push(nuevoPedido);

    const tarjeta = crearTarjetaPedido(nuevoPedido);
    listaRegistros.appendChild(tarjeta);

    actualizarContador();
    actualizarMensajeVacio();
  }

  function eliminarPedido(id) {
    pedidos = pedidos.filter(function (p) { return p.id !== id; });
    const elemento = document.getElementById('pedido-' + id);
    if (elemento) {
      elemento.style.transition = 'opacity 0.3s ease, transform 0.3s ease';
      elemento.style.opacity = '0';
      elemento.style.transform = 'scale(0.9)';
      setTimeout(function () {
        elemento.remove();
        actualizarContador();
        actualizarMensajeVacio();
      }, 300);
    }
  }

  formulario.addEventListener('submit', function (e) {
    e.preventDefault();

    if (!validarFormulario()) {
      mostrarAlerta('error', '⚠ Revisa los campos marcados en rojo antes de continuar.');
      return;
    }

    agregarPedido(inputNombre.value, inputDesc.value, selectCategoria.value);

    mostrarAlerta('exito', '✔ Pedido registrado correctamente.');
    reproducirSonidoExito();

    formulario.reset();
    limpiarValidaciones();

    // Antes hacía scroll hacia la lista y la alerta quedaba fuera de
    // vista. Ahora se centra en la alerta para que el usuario SÍ vea
    // la confirmación de "Pedido registrado correctamente".
    alertaCotizacion.scrollIntoView({ behavior: 'smooth', block: 'center' });
  });

  // Repinta los pedidos que ya existían en el array `pedidos`
  // (si el usuario navegó a otra sección y volvió al cotizador)
  pedidos.forEach(function (pedido) {
    listaRegistros.appendChild(crearTarjetaPedido(pedido));
  });
  actualizarContador();
  actualizarMensajeVacio();
}
