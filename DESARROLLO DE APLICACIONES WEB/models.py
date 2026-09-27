from flask_login import UserMixin

from conexion.conexion import get_connection


class Usuario(UserMixin):
    """Representa a un usuario autenticado. Flask-Login exige que el
    identificador expuesto como .id sea un string, por eso se convierte
    con str() en obtener_por_id (lo pide load_user())."""

    def __init__(self, id_usuario, usuario):
        self.id = str(id_usuario)
        self.usuario = usuario

    @staticmethod
    def obtener_por_id(user_id):
        """Usado por load_user() para reconstruir el usuario en cada request."""
        conn = get_connection()
        cursor = conn.cursor()
        cursor.execute('SELECT id, usuario FROM usuarios WHERE id = %s', (user_id,))
        fila = cursor.fetchone()
        cursor.close()
        conn.close()
        if fila is None:
            return None
        return Usuario(fila['id'], fila['usuario'])

    @staticmethod
    def obtener_por_nombre(nombre_usuario):
        """Devuelve la fila cruda (con el hash de password) para validar
        credenciales en la ruta /login. No devuelve un objeto Usuario porque
        aquí todavía necesitamos comparar el password con check_password_hash()."""
        conn = get_connection()
        cursor = conn.cursor()
        cursor.execute(
            'SELECT id, usuario, password FROM usuarios WHERE usuario = %s',
            (nombre_usuario,)
        )
        fila = cursor.fetchone()
        cursor.close()
        conn.close()
        return fila

    @staticmethod
    def crear(nombre_usuario, password_hash):
        """Inserta un nuevo usuario. Lanza psycopg2.errors.UniqueViolation si el
        nombre de usuario ya existe (columna UNIQUE)."""
        conn = get_connection()
        cursor = conn.cursor()
        cursor.execute(
            'INSERT INTO usuarios (usuario, password) VALUES (%s, %s)',
            (nombre_usuario, password_hash)
        )
        conn.commit()
        cursor.close()
        conn.close()