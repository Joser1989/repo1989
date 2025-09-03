# +++++++++++++++++++++++++++++++++++++++++++++++++
# Sistema de Gestión de Biblioteca Digital "U.E.A."
# +++++++++++++++++++++++++++++++++++++++++++++++++

from datetime import datetime

# ------------------------------
# Clase Libro
# ------------------------------
class Libro:
    """
    Representa un libro.
    - Guarda (autor, titulo) en una TUPLA para que sea inmutable.
    - category y isbn sí pueden existir como atributos nornales.
    """
    def __init__(self, titulo, autor, categoria, isbn):
        # Tupla inmutable con (autor, titulo) como pide la tarea
        self.info = (autor, titulo)
        self.categoria = categoria
        self.isbn = isbn

    # Propiedades de sólo lectura para acceder cómodo
    @property
    def autor(self):
        return self.info[0]

    @property
    def titulo(self):
        return self.info[1]

    def __repr__(self):
        return f"Libro(titulo='{self.titulo}', autor='{self.autor}', cat='{self.categoria}', ISBN='{self.isbn}')"


# ------------------------------
# Clase Usuario
# ------------------------------
class Usuario:
    """
    Representa un usuaio de la biblioteca.
    - id_usuario es único (la Biblioteca controla la unicidad con un conjunto).
    - libros_prestados es una LISTA (de ISBNs) como pide la tarea.
    """
    def __init__(self, nombre, id_usuario):
        self.nombre = nombre
        self.id_usuario = id_usuario
        self.libros_prestados = []  # lista de ISBNs

    def __repr__(self):
        return f"Usuario(nombre='{self.nombre}', id={self.id_usuario}, prestados={self.libros_prestados})"

# -----------Clase Biblioteca ------------
class Biblioteca:
    """
    Gestiona:
      - Libros disponibles en un DICCIONARIO: { isbn: Libro }
      - Usuarios en un DICCIONARIO: { id_usuario: Usuario }
      - IDs únicos en un CONJUNTO: { id_usuario, ... }
      - Préstamos activos en un DICCIONARIO: { isbn: id_usuario }
      - Historial (lista simple de eventos)
    """
    def __init__(self, nombre="Mi Biblioteca"):
        self.nombre = nombre
        self.libros = {}            # isbn -> Libro
        self.usuarios = {}          # id_usuario -> Usuario
        self.ids_usuarios = set()   # para asegurar unicidad
        self.prestamos_activos = {} # isbn -> id_usuario
        self.historial = []         # lista de dicts con eventos de préstamo/devolución

    # -------helpers simples--------
    def _evento(self, accion, id_usuario, isbn):
        # Guarda en historial un registro básico con hora
        self.historial.append({
            "fecha_hora": datetime.now().strftime("%Y-%m-%d %H:%M:%S"),
            "accion": accion,               # "PRESTAMO" o "DEVOLUCION" o "ALTA/BAJA/LIBRO"
            "id_usuario": id_usuario,
            "isbn": isbn
        })

    #  funcionalidades principales

    # ------Añadir libro------
    def agregar_libro(self, libro):
        if libro.isbn in self.libros:
            print(f"[X] Ya existe un libro con ISBN {libro.isbn}.")
            return False
        self.libros[libro.isbn] = libro
        self._evento("ALTA_LIBRO", None, libro.isbn)
        print(f"[✓] Libro agregado: {libro.titulo} (ISBN {libro.isbn}).")
        return True

    # Quitar libro (solo si no está prestado)
    def quitar_libro(self, isbn):
        if isbn not in self.libros:
            print(f"[X] No existe un libro con ISBN {isbn}.")
            return False
        if isbn in self.prestamos_activos:
            print(f"[X] No se puede quitar. El libro (ISBN {isbn}) está prestado.")
            return False
        libro = self.libros.pop(isbn)
        self._evento("BAJA_LIBRO", None, isbn)
        print(f"[✓] Libro quitado: {libro.titulo} (ISBN {isbn}).")
        return True

    # ---------Registrar usuario-------
    def registrar_usuario(self, usuario):
        if usuario.id_usuario in self.ids_usuarios:
            print(f"[X] Ya existe un usuario con ID {usuario.id_usuario}.")
            return False
        self.ids_usuarios.add(usuario.id_usuario)  # conjunto para unicidad
        self.usuarios[usuario.id_usuario] = usuario  # también lo guardamos para acceso rápido
        self._evento("ALTA_USUARIO", usuario.id_usuario, None)
        print(f"[✓] Usuario registrado: {usuario.nombre} (ID {usuario.id_usuario}).")
        return True

    # Dar de baja usuario (solo si no tiene libros prestados)
    def dar_baja_usuario(self, id_usuario):
        if id_usuario not in self.usuarios:
            print(f"[X] No existe el usuario con ID {id_usuario}.")
            return False
        usuario = self.usuarios[id_usuario]
        if usuario.libros_prestados:
            print(f"[X] No se puede dar de baja: el usuario tiene libros prestados: {usuario.libros_prestados}")
            return False
        # eliminar de ambos: dict y set
        del self.usuarios[id_usuario]
        self.ids_usuarios.remove(id_usuario)
        self._evento("BAJA_USUARIO", id_usuario, None)
        print(f"[✓] Usuario dado de baja: {id_usuario}.")
        return True

    # ---------Prestar libro----------
    def prestar_libro(self, id_usuario, isbn):
        if id_usuario not in self.usuarios:
            print(f"[X] Usuario ID {id_usuario} no existe.")
            return False
        if isbn not in self.libros:
            print(f"[X] Libro ISBN {isbn} no existe.")
            return False
        if isbn in self.prestamos_activos:
            print(f"[X] El libro ISBN {isbn} ya está prestado al usuario ID {self.prestamos_activos[isbn]}.")
            return False

        usuario = self.usuarios[id_usuario]
        if isbn in usuario.libros_prestados:
            print(f"[X] El usuario ya tiene registrado este libro (ISBN {isbn}).")
            return False

        # -------Registrar préstamo-------
        usuario.libros_prestados.append(isbn)          # lista en el usuario
        self.prestamos_activos[isbn] = id_usuario      # control global
        self._evento("PRESTAMO", id_usuario, isbn)
        libro = self.libros[isbn]
        print(f"[✓] Préstamo OK: '{libro.titulo}' a {usuario.nombre}.")
        return True

    # ---------Devolver libro---------
    def devolver_libro(self, id_usuario, isbn):
        if id_usuario not in self.usuarios:
            print(f"[X] Usuario ID {id_usuario} no existe.")
            return False
        if isbn not in self.libros:
            print(f"[X] Libro ISBN {isbn} no existe.")
            return False
        if isbn not in self.prestamos_activos:
            print(f"[X] Ese libro (ISBN {isbn}) no está registrado como prestado.")
            return False
        # Validar que lo devolviese quien lo tenía
        if self.prestamos_activos[isbn] != id_usuario:
            print(f"[X] El libro (ISBN {isbn}) está registrado como prestado a otro usuario (ID {self.prestamos_activos[isbn]}).")
            return False

        usuario = self.usuarios[id_usuario]
        if isbn in usuario.libros_prestados:
            usuario.libros_prestados.remove(isbn)
        self.prestamos_activos.pop(isbn)
        self._evento("DEVOLUCION", id_usuario, isbn)
        libro = self.libros[isbn]
        print(f"[✓] Devolución OK: '{libro.titulo}' por {usuario.nombre}.")
        return True

    # Buscar por título (contiene, sin distinguir mayúsculas)
    def buscar_por_titulo(self, texto):
        texto = texto.lower()
        resultados = [lib for lib in self.libros.values() if texto in lib.titulo.lower()]
        return resultados

    # -------------Buscar por autor--------
    def buscar_por_autor(self, texto):
        texto = texto.lower()
        resultados = [lib for lib in self.libros.values() if texto in lib.autor.lower()]
        return resultados

    # Buscar por categoría (igualdad simple, ignorando mayúsculas)
    def buscar_por_categoria(self, categoria):
        categoria = categoria.lower()
        resultados = [lib for lib in self.libros.values() if lib.categoria.lower() == categoria]
        return resultados

    # Listar libros prestados a un usuario (devuelve objetos Libro)
    def listar_prestados_usuario(self, id_usuario):
        if id_usuario not in self.usuarios:
            print(f"[X] Usuario ID {id_usuario} no existe.")
            return []
        usuario = self.usuarios[id_usuario]
        return [self.libros[isbn] for isbn in usuario.libros_prestados if isbn in self.libros]

    # Mostrar historial
    def mostrar_historial(self):
        if not self.historial:
            print("(No hay eventos en el historial)")
            return
        print("\n--- Historial de eventos ---")
        for ev in self.historial:
            print(f"{ev['fecha_hora']} | {ev['accion']} | user={ev['id_usuario']} | isbn={ev['isbn']}")
        print("-----------------------------\n")


# -----------PRUEBA DEL SISTEMA------------
if __name__ == "__main__":
    # 1) Crear biblioteca
    biblio = Biblioteca("Biblioteca Central UEA")

    # 2) Crear y agregar libros (ISBN únicos)
    l1 = Libro(titulo="El Principito", autor="Antoine de Saint-Exupéry", categoria="Ficción", isbn="978-1")
    l2 = Libro(titulo="Cien Años de Soledad", autor="Gabriel García Márquez", categoria="Realismo Mágico", isbn="978-2")
    l3 = Libro(titulo="POO para Todos", autor="W. Nuñez", categoria="Tecnología", isbn="978-3")

    biblio.agregar_libro(l1)
    biblio.agregar_libro(l2)
    biblio.agregar_libro(l3)

    # 3) Registrar usuarios (IDs únicos controlados por un CONJUNTO)
    u1 = Usuario(nombre="Anita Lucia Proaño", id_usuario=101)
    u2 = Usuario(nombre="José Ramírez", id_usuario=102)

    biblio.registrar_usuario(u1)
    biblio.registrar_usuario(u2)

    # 4) Prestar libros
    biblio.prestar_libro(id_usuario=101, isbn="978-1")  # Anita lucia proaño -> El Principito
    biblio.prestar_libro(id_usuario=102, isbn="978-2")  # Jose -> Cien Años de Soledad

    # 5) Listar libros prestados a un usuario
    print("Libros prestados a Anita lucia proaño (ID 101):", biblio.listar_prestados_usuario(101))
    print("Libros prestados a Jose (ID 102):", biblio.listar_prestados_usuario(102))

    # 6) Búsquedas
    print("\nBuscar por título 'POO':", biblio.buscar_por_titulo("Programación"))
    print("Buscar por autor 'García':", biblio.buscar_por_autor("García"))
    print("Buscar por categoría 'Tecnologías':", biblio.buscar_por_categoria("Tecnología"))

    # 7) Intentar quitar un libro prestado
    biblio.quitar_libro("978-1")

    # 8) Devolver y luego quitar
    biblio.devolver_libro(id_usuario=101, isbn="978-1")
    biblio.quitar_libro("978-1")

    # 9) Intentar dar de baja a un usuario con libros prestados
    biblio.dar_baja_usuario(102)  # Luis aún tiene "978-2"
    # Devolver y dar de baja
    biblio.devolver_libro(id_usuario=102, isbn="978-2")
    biblio.dar_baja_usuario(102)

    # 10) Mostrar historial
    biblio.mostrar_historial()

    # 11) Estado final básico
    print("Catálogo actual:", list(biblio.libros.values()))
    print("Usuarios actuales:", list(biblio.usuarios.values()))
    print ("Gracias por utilizar nuestra biblioteca digital U.E.A.")