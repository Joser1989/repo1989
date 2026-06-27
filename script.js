/* ================================================================
   AL WORK — script.js
   Cotizador de Pedidos Dinámico
   Semana 5 — Desarrollo de Aplicaciones Web
   ================================================================ */

// ── REFERENCIAS AL DOM ──────────────────────────────────────────
const formulario     = document.getElementById('formCotizacion');
const inputNombre    = document.getElementById('cotNombre');
const inputDesc      = document.getElementById('cotDescripcion');
const selectCategoria = document.getElementById('cotCategoria');
const listaRegistros = document.getElementById('listaRegistros');
const contadorTotal  = document.getElementById('contadorTotal');
const mensajeVacio   = document.getElementById('mensajeVacio');

// ── ARRAY DE PEDIDOS ────────────────────────────────────────────
let pedidos = [];
let contadorId = 1;

// ── FUNCIÓN: ACTUALIZAR CONTADOR ────────────────────────────────
function actualizarContador() {
  contadorTotal.textContent = pedidos.length;
}

// ── FUNCIÓN: MOSTRAR / OCULTAR MENSAJE VACÍO ────────────────────
function actualizarMensajeVacio() {
  if (pedidos.length === 0) {
    mensajeVacio.style.display = 'block';
  } else {
    mensajeVacio.style.display = 'none';
  }
}

// ── FUNCIÓN: MOSTRAR ERROR EN CAMPO ─────────────────────────────
function mostrarError(campo, mensaje) {
  campo.classList.add('is-invalid');
  let feedback = campo.nextElementSibling;
  if (!feedback || !feedback.classList.contains('invalid-feedback')) {
    feedback = document.createElement('div');
    feedback.classList.add('invalid-feedback');
    campo.parentNode.appendChild(feedback);
  }
  feedback.textContent = mensaje;
}

// ── FUNCIÓN: LIMPIAR ERROR EN CAMPO ─────────────────────────────
function limpiarError(campo) {
  campo.classList.remove('is-invalid');
  campo.classList.add('is-valid');
}

// ── FUNCIÓN: LIMPIAR TODOS LOS ESTADOS ──────────────────────────
function limpiarValidaciones() {
  [inputNombre, inputDesc, selectCategoria].forEach(function(campo) {
    campo.classList.remove('is-invalid', 'is-valid');
  });
}

// ── FUNCIÓN: VALIDAR FORMULARIO ──────────────────────────────────
function validarFormulario() {
  let valido = true;

  if (inputNombre.value.trim() === '') {
    mostrarError(inputNombre, 'El nombre del cliente es obligatorio.');
    valido = false;
  } else {
    limpiarError(inputNombre);
  }

  if (inputDesc.value.trim() === '') {
    mostrarError(inputDesc, 'La descripción del pedido es obligatoria.');
    valido = false;
  } else {
    limpiarError(inputDesc);
  }

  if (selectCategoria.value === '') {
    mostrarError(selectCategoria, 'Selecciona un tipo de prenda.');
    valido = false;
  } else {
    limpiarError(selectCategoria);
  }

  return valido;
}

// ── FUNCIÓN: OBTENER BADGE DE COLOR SEGÚN CATEGORÍA ─────────────
function getBadgeCategoria(categoria) {
  const colores = {
    'Polo Corporativo':    'bg-primary',
    'Polo Industrial':     'bg-warning text-dark',
    'Chaleco de Seguridad':'bg-danger',
    'Camisa de Trabajo':   'bg-success',
    'Uniforme Completo':   'bg-dark',
    'Personalizado':       'bg-secondary'
  };
  return colores[categoria] || 'bg-secondary';
}

// ── FUNCIÓN: CREAR TARJETA DE PEDIDO (createElement + appendChild)
function crearTarjetaPedido(pedido) {
  // Contenedor columna
  const col = document.createElement('div');
  col.classList.add('col-md-6', 'col-lg-4');
  col.setAttribute('id', 'pedido-' + pedido.id);

  // Card
  const card = document.createElement('div');
  card.classList.add('card', 'pedido-card', 'h-100', 'shadow-sm');

  // Card body
  const cardBody = document.createElement('div');
  cardBody.classList.add('card-body');

  // Badge categoría
  const badge = document.createElement('span');
  badge.classList.add('badge', getBadgeCategoria(pedido.categoria), 'mb-2');
  badge.textContent = pedido.categoria;

  // Número de pedido
  const numeroPedido = document.createElement('small');
  numeroPedido.classList.add('text-muted', 'd-block', 'mb-1');
  numeroPedido.textContent = '# Pedido ' + String(pedido.id).padStart(3, '0');

  // Título (nombre cliente)
  const titulo = document.createElement('h5');
  titulo.classList.add('card-title', 'pedido-nombre');
  titulo.textContent = pedido.nombre;

  // Descripción
  const descripcion = document.createElement('p');
  descripcion.classList.add('card-text', 'pedido-desc');
  descripcion.textContent = pedido.descripcion;

  // Fecha
  const fecha = document.createElement('small');
  fecha.classList.add('text-muted');
  fecha.textContent = '📅 ' + pedido.fecha;

  // Botón eliminar
  const btnEliminar = document.createElement('button');
  btnEliminar.classList.add('btn', 'btn-outline-danger', 'btn-sm', 'mt-3', 'w-100', 'btn-eliminar');
  btnEliminar.textContent = '🗑 Eliminar Pedido';
  btnEliminar.setAttribute('data-id', pedido.id);

  // addEventListener para eliminar
  btnEliminar.addEventListener('click', function() {
    eliminarPedido(pedido.id);
  });

  // Armar estructura con appendChild
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
  // Remover del array
  pedidos = pedidos.filter(function(p) { return p.id !== id; });

  // Remover del DOM con animación
  const elemento = document.getElementById('pedido-' + id);
  if (elemento) {
    elemento.style.transition = 'opacity 0.3s ease, transform 0.3s ease';
    elemento.style.opacity = '0';
    elemento.style.transform = 'scale(0.9)';
    setTimeout(function() {
      elemento.remove();
      actualizarContador();
      actualizarMensajeVacio();
    }, 300);
  }
}

// ── EVENTO SUBMIT DEL FORMULARIO ─────────────────────────────────
formulario.addEventListener('submit', function(e) {
  e.preventDefault(); // Evita que la página se recargue

  if (!validarFormulario()) return;

  agregarPedido(
    inputNombre.value,
    inputDesc.value,
    selectCategoria.value
  );

  // Limpiar formulario
  formulario.reset();
  limpiarValidaciones();

  // Scroll suave hacia los registros
  listaRegistros.scrollIntoView({ behavior: 'smooth', block: 'start' });
});

// ── LIMPIAR VALIDACIONES AL ESCRIBIR ────────────────────────────
inputNombre.addEventListener('input', function() {
  if (inputNombre.value.trim() !== '') limpiarError(inputNombre);
});

inputDesc.addEventListener('input', function() {
  if (inputDesc.value.trim() !== '') limpiarError(inputDesc);
});

selectCategoria.addEventListener('change', function() {
  if (selectCategoria.value !== '') limpiarError(selectCategoria);
});

// ── NAVBAR: MARCAR ENLACE ACTIVO AL HACER SCROLL ────────────────
const secciones = document.querySelectorAll('section[id]');
const navLinks  = document.querySelectorAll('.navbar-alwork .nav-link');

window.addEventListener('scroll', function() {
  let actual = '';
  secciones.forEach(function(sec) {
    const top = sec.offsetTop - 100;
    if (window.scrollY >= top) {
      actual = sec.getAttribute('id');
    }
  });
  navLinks.forEach(function(link) {
    link.classList.remove('active');
    if (link.getAttribute('href') === '#' + actual) {
      link.classList.add('active');
    }
  });
});
