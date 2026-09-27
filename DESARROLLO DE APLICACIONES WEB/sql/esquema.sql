-- Esquema de la base de datos PostgreSQL del proyecto AL WORK (alwork_db)
-- Se ejecuta automáticamente al iniciar la app (init_db en conexion/conexion.py).

CREATE TABLE IF NOT EXISTS proveedores (
    id_proveedor SERIAL PRIMARY KEY,
    nombre       TEXT NOT NULL,
    telefono     TEXT,
    correo       TEXT
);

CREATE TABLE IF NOT EXISTS productos (
    id           SERIAL PRIMARY KEY,
    nombre       TEXT NOT NULL,
    precio       REAL NOT NULL,
    imagen       TEXT NOT NULL,
    descripcion  TEXT NOT NULL,
    disponible   INTEGER NOT NULL,
    id_proveedor INTEGER,
    FOREIGN KEY (id_proveedor) REFERENCES proveedores(id_proveedor)
);

CREATE TABLE IF NOT EXISTS clientes (
    id_cliente SERIAL PRIMARY KEY,
    nombre     TEXT NOT NULL,
    cedula     TEXT UNIQUE,
    telefono   TEXT,
    correo     TEXT
);

CREATE TABLE IF NOT EXISTS facturas (
    id_factura SERIAL PRIMARY KEY,
    id_cliente INTEGER,
    fecha      TEXT NOT NULL,
    total      REAL NOT NULL,
    FOREIGN KEY (id_cliente) REFERENCES clientes(id_cliente)
);

-- Tabla de usuarios del sistema (Semana 14 - login con Flask-Login)
-- El campo "usuario" es UNIQUE para evitar registros duplicados.
-- El campo "password" NUNCA guarda texto plano: siempre un hash generado
-- con generate_password_hash() (Werkzeug) antes del INSERT.
CREATE TABLE IF NOT EXISTS usuarios (
    id       SERIAL PRIMARY KEY,
    usuario  TEXT NOT NULL UNIQUE,
    password TEXT NOT NULL
);

-- Datos iniciales (solo se insertan si no existen, para no duplicar en cada arranque)
INSERT INTO proveedores (nombre, telefono, correo)
SELECT 'Almacenes José Puebla', '022345678', 'contacto@josepuebla.com'
WHERE NOT EXISTS (SELECT 1 FROM proveedores WHERE nombre = 'Almacenes José Puebla');

INSERT INTO proveedores (nombre, telefono, correo)
SELECT 'Lindtex', '022987654', 'ventas@lindtex.com'
WHERE NOT EXISTS (SELECT 1 FROM proveedores WHERE nombre = 'Lindtex');
-- Ampliación de columnas para que coincidan con los formularios existentes
ALTER TABLE proveedores ADD COLUMN IF NOT EXISTS tipo TEXT;
ALTER TABLE proveedores ADD COLUMN IF NOT EXISTS ciudad TEXT;

ALTER TABLE clientes ADD COLUMN IF NOT EXISTS tipo TEXT;
ALTER TABLE clientes ADD COLUMN IF NOT EXISTS ciudad TEXT;

ALTER TABLE facturas ADD COLUMN IF NOT EXISTS numero TEXT;
ALTER TABLE facturas ADD COLUMN IF NOT EXISTS detalle TEXT;