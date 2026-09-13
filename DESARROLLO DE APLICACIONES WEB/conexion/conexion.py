import mysql.connector

def get_connection():
    """Abre una conexión a la base de datos MySQL alwork_db."""
    return mysql.connector.connect(
        host="localhost",
        user="root",
        password="0705443539",
        database="alwork_db"
    )