import os
import psycopg2
import psycopg2.extras

# Datos de conexión a PostgreSQL local (los mismos que configuraste en la instalación)
DB_CONFIG = {
    'host': 'localhost',
    'port': '5432',
    'dbname': 'alwork_db',
    'user': 'postgres',
    'password': '0705443539'   
}

BASE_DIR = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
ESQUEMA_PATH = os.path.join(BASE_DIR, 'sql', 'esquema.sql')


def get_connection():
    """Abre una conexión a la base de datos PostgreSQL (alwork_db).

    cursor_factory=RealDictCursor permite acceder a las columnas como
    diccionario, ej: fila['nombre'], igual que hacías con sqlite3.Row.
    """
    conn = psycopg2.connect(cursor_factory=psycopg2.extras.RealDictCursor, **DB_CONFIG)
    return conn


def init_db():
    """Crea las tablas en alwork_db ejecutando sql/esquema.sql.

    El script usa CREATE TABLE IF NOT EXISTS, así que se puede ejecutar
    todas las veces que haga falta sin borrar datos.
    """
    conn = get_connection()
    cur = conn.cursor()
    with open(ESQUEMA_PATH, encoding='utf-8') as archivo:
        cur.execute(archivo.read())
    conn.commit()
    cur.close()
    conn.close()