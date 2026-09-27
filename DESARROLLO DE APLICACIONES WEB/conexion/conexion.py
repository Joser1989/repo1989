import os
import psycopg2
import psycopg2.extras

# Datos de conexión a PostgreSQL LOCAL (los mismos que configuraste en la instalación).
# Se usan solo si no existe la variable de entorno DATABASE_URL (es decir, en tu PC).
DB_CONFIG_LOCAL = {
    'host': 'localhost',
    'port': '5432',
    'dbname': 'alwork_db',
    'user': 'postgres',
    'password': '0705443539' 
}

# En Render, la variable de entorno DATABASE_URL contiene toda la conexión
# (host, usuario, contraseña, puerto, nombre de la base) en una sola cadena.
DATABASE_URL = os.environ.get('DATABASE_URL')

BASE_DIR = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
ESQUEMA_PATH = os.path.join(BASE_DIR, 'sql', 'esquema.sql')


def get_connection():
    """Abre una conexión a PostgreSQL.

    Si existe DATABASE_URL (estamos en Render), se conecta con esa cadena.
    Si no existe (estamos en tu PC), usa la configuración local de siempre.

    cursor_factory=RealDictCursor permite acceder a las columnas como
    diccionario, ej: fila['nombre'].
    """
    if DATABASE_URL:
        conn = psycopg2.connect(DATABASE_URL, cursor_factory=psycopg2.extras.RealDictCursor, sslmode='require')
    else:
        conn = psycopg2.connect(cursor_factory=psycopg2.extras.RealDictCursor, **DB_CONFIG_LOCAL)
    return conn


def init_db():
    """Crea las tablas ejecutando sql/esquema.sql.

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