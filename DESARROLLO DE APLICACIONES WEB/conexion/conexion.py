import os
import sqlite3

# Rutas absolutas: funcionan sin importar desde dónde se ejecute la app.
BASE_DIR = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
DB_PATH = os.path.join(BASE_DIR, 'data', 'alwork.db')
ESQUEMA_PATH = os.path.join(BASE_DIR, 'sql', 'esquema.sql')


def get_connection():
    """Abre una conexión a la base de datos SQLite (data/alwork.db).

    SQLite guarda todo en un archivo local: no usa usuario ni contraseña,
    por eso ya no hay credenciales dentro del código.
    """
    conn = sqlite3.connect(DB_PATH)
    conn.row_factory = sqlite3.Row          # permite fila['nombre'] y fila.nombre en Jinja2
    conn.execute('PRAGMA foreign_keys = ON')  # activa las llaves foráneas en SQLite
    return conn


def init_db():
    """Crea data/alwork.db y sus tablas ejecutando sql/esquema.sql.

    El script usa CREATE TABLE IF NOT EXISTS, así que se puede ejecutar
    todas las veces que haga falta sin borrar datos.
    """
    os.makedirs(os.path.dirname(DB_PATH), exist_ok=True)
    conn = get_connection()
    with open(ESQUEMA_PATH, encoding='utf-8') as archivo:
        conn.executescript(archivo.read())
    conn.commit()
    conn.close()
