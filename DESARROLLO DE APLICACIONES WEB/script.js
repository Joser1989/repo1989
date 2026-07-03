/* ================================================================
   AL WORK — script.js
   Cotizador de Pedidos Dinámico
   Semana 6 — Validaciones dinámicas y manejo básico de formularios
   ================================================================ */

// ── REFERENCIAS AL DOM ──────────────────────────────────────────
const formulario      = document.getElementById('formCotizacion');
const inputNombre     = document.getElementById('cotNombre');
const inputDesc       = document.getElementById('cotDescripcion');
const selectCategoria = document.getElementById('cotCategoria');
const listaRegistros  = document.getElementById('listaRegistros');
const contadorTotal   = document.getElementById('contadorTotal');
const mensajeVacio    = document.getElementById('mensajeVacio');
const alertaCotizacion = document.getElementById('alertaCotizacion');

// ── CONFIGURACIÓN DE VALIDACIÓN (Semana 6) ──────────────────────
const CONFIG = {
  nombreMinLength: 3,
  descMinLength: 10
};

// ── ARRAY DE PEDIDOS ────────────────────────────────────────────
let pedidos = [];
let contadorId = 1;

// ── FUNCIÓN: ACTUALIZAR CONTADOR ────────────────────────────────
function actualizarContador() {
  contadorTotal.textContent = pedidos.length;
}

// ── FUNCIÓN: MOSTRAR / OCULTAR MENSAJE VACÍO ────────────────────
function actualizarMensajeVacio() {
  mensajeVacio.style.display = pedidos.length === 0 ? 'block' : 'none';
}

/* ================================================================
   FEEDBACK POR CAMPO (is-invalid / is-valid)
   ================================================================ */

// ── FUNCIÓN: MOSTRAR ERROR EN CAMPO ─────────────────────────────
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

// ── FUNCIÓN: MOSTRAR ÉXITO EN CAMPO ─────────────────────────────
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

// ── FUNCIÓN: LIMPIAR ESTADO VISUAL DE UN CAMPO ──────────────────
function limpiarEstado(campo) {
  campo.classList.remove('is-invalid', 'is-valid');
}

// ── FUNCIÓN: LIMPIAR TODOS LOS CAMPOS DEL FORMULARIO ────────────
function limpiarValidaciones() {
  [inputNombre, inputDesc, selectCategoria].forEach(limpiarEstado);
}

/* ================================================================
   ALERTA GENERAL (alert-success / alert-danger) — Semana 6
   ================================================================ */

function mostrarAlerta(tipo, mensaje) {
  alertaCotizacion.classList.remove('d-none', 'alert-success', 'alert-danger');
  alertaCotizacion.classList.add(tipo === 'exito' ? 'alert-success' : 'alert-danger');
  alertaCotizacion.textContent = mensaje;

  // Oculta la alerta automáticamente después de unos segundos
  clearTimeout(alertaCotizacion._timeoutId);
  alertaCotizacion._timeoutId = setTimeout(function () {
    alertaCotizacion.classList.add('d-none');
  }, 4000);
}

/* ================================================================
   VALIDACIONES INDIVIDUALES POR CAMPO (Semana 6)
   Cada función valida SOLO su campo y devuelve true/false.
   Se reutilizan tanto en 'blur'/'input' como en el 'submit'.
   ================================================================ */

// ── VALIDAR NOMBRE / TÍTULO DEL CLIENTE ─────────────────────────
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

// ── VALIDAR DESCRIPCIÓN DEL PEDIDO ──────────────────────────────
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

// ── VALIDAR CATEGORÍA / TIPO DE PRENDA ──────────────────────────
function validarCategoria() {
  if (selectCategoria.value === '') {
    mostrarError(selectCategoria, 'Selecciona un tipo de prenda.');
    return false;
  }

  mostrarExito(selectCategoria, 'Categoría seleccionada.');
  return true;
}

// ── FUNCIÓN: VALIDAR FORMULARIO COMPLETO ────────────────────────
function validarFormulario() {
  // Se ejecutan las tres para que TODOS los campos muestren su
  // estado (no solo el primero que falle).
  const nombreOk    = validarNombre();
  const descOk      = validarDescripcion();
  const categoriaOk = validarCategoria();

  return nombreOk && descOk && categoriaOk;
}

/* ================================================================
   VALIDACIÓN EN TIEMPO REAL — eventos input / blur (Semana 6)
   ================================================================ */

// input: mientras el usuario escribe, si ya es válido lo confirmamos
inputNombre.addEventListener('input', function () {
  if (inputNombre.classList.contains('is-invalid')) validarNombre();
});
// blur: al salir del campo, se valida siempre
inputNombre.addEventListener('blur', validarNombre);

inputDesc.addEventListener('input', function () {
  if (inputDesc.classList.contains('is-invalid')) validarDescripcion();
});
inputDesc.addEventListener('blur', validarDescripcion);

selectCategoria.addEventListener('change', validarCategoria);
selectCategoria.addEventListener('blur', validarCategoria);

/* ================================================================
   TARJETAS DE PEDIDO (createElement + appendChild)
   ================================================================ */

// ── FUNCIÓN: OBTENER BADGE DE COLOR SEGÚN CATEGORÍA ─────────────
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

// ── FUNCIÓN: CREAR TARJETA DE PEDIDO ────────────────────────────
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
  // getBadgeCategoria puede devolver una o varias clases separadas por espacio
  // (ej: "bg-warning text-dark"), por eso se dividen antes de agregarlas.
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

// ── FUNCIÓN: AGREGAR PEDIDO ──────────────────────────────────────
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

// ── FUNCIÓN: ELIMINAR PEDIDO ─────────────────────────────────────
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

/* ================================================================
   EVENTO SUBMIT DEL FORMULARIO
   ================================================================ */

formulario.addEventListener('submit', function (e) {
  e.preventDefault(); // Evita que la página se recargue

  if (!validarFormulario()) {
    mostrarAlerta('error', '⚠ Revisa los campos marcados en rojo antes de continuar.');
    return;
  }

  agregarPedido(
    inputNombre.value,
    inputDesc.value,
    selectCategoria.value
  );

  mostrarAlerta('exito', '✔ Pedido registrado correctamente.');

  formulario.reset();
  limpiarValidaciones();

  listaRegistros.scrollIntoView({ behavior: 'smooth', block: 'start' });
});

/* ================================================================
   NAVBAR: MARCAR ENLACE ACTIVO AL HACER SCROLL
   ================================================================ */

const secciones = document.querySelectorAll('section[id]');
const navLinks  = document.querySelectorAll('.navbar-alwork .nav-link');

window.addEventListener('scroll', function () {
  let actual = '';
  secciones.forEach(function (sec) {
    const top = sec.offsetTop - 100;
    if (window.scrollY >= top) {
      actual = sec.getAttribute('id');
    }
  });
  navLinks.forEach(function (link) {
    link.classList.remove('active');
    if (link.getAttribute('href') === '#' + actual) {
      link.classList.add('active');
    }
  });
});
