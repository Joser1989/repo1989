# Semana 07_ POO_ Tarea: Constructores y Destructores
# Clase MarcaAuto que demuestra el uso de constructores (__init__) y destructores (__del__)
class MarcaAuto:
    def __init__(self, nombre, pais_origen):
        """
        Constructor de la clase MarcaAuto.
        Se ejecuta automáticamente cuando se crea un objeto de la clase.

        :param nombre: Nombre de la marca de auto (ej. 'Toyota')
        :param pais_origen: País de origen de la marca (ej. 'Japón')
        """
        self.nombre = nombre
        self.pais_origen = pais_origen
        print(f"Se ha creado la marca '{self.nombre}' de {self.pais_origen}.")

    def mostrar_info(self):
        """
        Método para mostrar información de la marca de auto.
        """
        print(f"Marca: {self.nombre} | País de Origen: {self.pais_origen}")

    def __del__(self):
        """
        Destructor de la clase MarcaAuto.
        Se ejecuta automáticamente cuando el objeto es destruido o eliminado.
        Sirve para liberar recursos, cerrar archivos o mostrar mensajes de limpieza.
        """
        print(f"Se ha eliminado la marca '{self.nombre}' de la memoria.")


# Programa principal para probar la clase MarcaAuto
if __name__ == "__main__":
    # Crear objetos de tipo MarcaAuto
    marca1 = MarcaAuto("Toyota", "Japón")
    marca2 = MarcaAuto("Aston Martin", "Reino Unido")
    marca3 = MarcaAuto("BMW", "Alemania")

    # Mostrar información de las marcas
    marca1.mostrar_info()
    marca2.mostrar_info()
    marca3.mostrar_info()

    # Eliminar un objeto manualmente para ver el destructor en acción
    del marca2

    print("Fin del programa. Los destructores de los objetos restantes se llamarán automáticamente.")
