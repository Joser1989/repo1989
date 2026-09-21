-- Esquema de la base de datos SQLite del proyecto AL WORK (data/alwork.db)
-- Se ejecuta automáticamente al iniciar la app (init_db en conexion/conexion.py).

PRAGMA foreign_keys = ON;

CREATE TABLE IF NOT EXISTS proveedores (
    id_proveedor INTEGER PRIMARY KEY AUTOINCREMENT,
    nombre       TEXT NOT NULL,
    telefono     TEXT,
    correo       TEXT
);

CREATE TABLE IF NOT EXISTS productos (
    id           INTEGER PRIMARY KEY AUTOINCREMENT,
    nombre       TEXT NOT NULL,
    precio       REAL NOT NULL,
    imagen       TEXT NOT NULL,
    descripcion  TEXT NOT NULL,
    disponible   INTEGER NOT NULL,
    id_proveedor INTEGER,
    FOREIGN KEY (id_proveedor) REFERENCES proveedores(id_proveedor)
);

CREATE TABLE IF NOT EXISTS clientes (
    id_cliente INTEGER PRIMARY KEY AUTOINCREMENT,
    nombre     TEXT NOT NULL,
    cedula     TEXT UNIQUE,
    telefono   TEXT,
    correo     TEXT
);

CREATE TABLE IF NOT EXISTS facturas (
    id_factura INTEGER PRIMARY KEY AUTOINCREMENT,
    id_cliente INTEGER,
    fecha      TEXT NOT NULL,
    total      REAL NOT NULL,
    FOREIGN KEY (id_cliente) REFERENCES clientes(id_cliente)
);

-- Datos iniciales (solo se insertan si no existen, para no duplicar en cada arranque)
INSERT INTO proveedores (nombre, telefono, correo)
SELECT 'Almacenes José Puebla', '022345678', 'contacto@josepuebla.com'
WHERE NOT EXISTS (SELECT 1 FROM proveedores WHERE nombre = 'Almacenes José Puebla');

INSERT INTO proveedores (nombre, telefono, correo)
SELECT 'Lindtex', '022987654', 'ventas@lindtex.com'
WHERE NOT EXISTS (SELECT 1 FROM proveedores WHERE nombre = 'Lindtex');
