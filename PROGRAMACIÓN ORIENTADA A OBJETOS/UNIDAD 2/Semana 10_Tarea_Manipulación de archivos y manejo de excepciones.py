# ================================
# Sistema de Gestión de Inventarios JR, Week 10
# Autor: José Ramirez
# ================================

# Clase Producto
import os

class Producto:
    def __init__(self, id, nombre, cantidad, precio):
        self._id = id
        self._nombre = nombre
        self._cantidad = cantidad
        self._precio = precio

    def get_id(self):
        return self._id

    def get_nombre(self):
        return self._nombre

    def get_cantidad(self):
        return self._cantidad

    def get_precio(self):
        return self._precio

    def set_nombre(self, nombre):
        self._nombre = nombre

    def set_cantidad(self, cantidad):
        self._cantidad = cantidad

    def set_precio(self, precio):
        self._precio = precio

    def __str__(self):
        return f"ID: {self._id}, Nombre: {self._nombre}, Cantidad: {self._cantidad}, Precio: ${self._precio:.2f}"

    # Representación en archivo (línea de texto)
    def to_line(self):
        return f"{self._id},{self._nombre},{self._cantidad},{self._precio}\n"

    @staticmethod
    def from_line(line):
        try:
            id, nombre, cantidad, precio = line.strip().split(",")
            return Producto(id, nombre, int(cantidad), float(precio))
        except ValueError:
            return None  # Si hay error en el formato de la línea


# Clase Inventario con manejo de archivos
class Inventario:
    def __init__(self, archivo="inventario.txt"):
        self.archivo = archivo
        self.productos = []
        self.cargar_desde_archivo()

    def cargar_desde_archivo(self):
        """Carga productos desde el archivo inventario.txt"""
        if not os.path.exists(self.archivo):
            # Si no existe, se crea vacío
            open(self.archivo, "a").close()
            return

        try:
            with open(self.archivo, "r", encoding="utf-8") as f:
                for line in f:
                    producto = Producto.from_line(line)
                    if producto:
                        self.productos.append(producto)
        except (FileNotFoundError, PermissionError) as e:
            print(f"Error al leer el archivo: {e}")

    def guardar_en_archivo(self):
        """Guarda los productos en el archivo inventario.txt"""
        try:
            with open(self.archivo, "w", encoding="utf-8") as f:
                for p in self.productos:
                    f.write(p.to_line())
        except PermissionError:
            print("No tienes permisos para escribir en el archivo.")
        except Exception as e:
            print(f"Ocurrió un error al guardar: {e}")

    def agregar_producto(self, id, nombre, cantidad, precio):
        for p in self.productos:
            if p.get_id() == id:
                print("El ID ya existe. No se agregó el producto.")
                return
        self.productos.append(Producto(id, nombre, cantidad, precio))
        self.guardar_en_archivo()
        print("Producto agregado y guardado en archivo.")

    def eliminar_producto(self, id):
        for p in self.productos:
            if p.get_id() == id:
                self.productos.remove(p)
                self.guardar_en_archivo()
                print("Producto eliminado y archivo actualizado.")
                return
        print("No se encontró el producto.")

    def actualizar_producto(self, id, cantidad=None, precio=None):
        for p in self.productos:
            if p.get_id() == id:
                if cantidad is not None:
                    p.set_cantidad(cantidad)
                if precio is not None:
                    p.set_precio(precio)
                self.guardar_en_archivo()
                print("Producto actualizado y archivo modificado.")
                return
        print("No se encontró el producto.")

    def buscar_producto(self, nombre):
        encontrados = [p for p in self.productos if nombre.lower() in p.get_nombre().lower()]
        if encontrados:
            for p in encontrados:
                print(p)
        else:
            print("No se encontraron productos.")

    def mostrar_todos(self):
        if not self.productos:
            print("Inventario vacío.")
        else:
            for p in self.productos:
                print(p)


# Menú principal
def menu():
    inventario = Inventario()

    while True:
        print("\n--- Menú de Inventario ---")
        print("1. Añadir producto")
        print("2. Eliminar producto")
        print("3. Actualizar producto")
        print("4. Buscar producto por nombre")
        print("5. Mostrar todos los productos")
        print("6. Salir")

        opcion = input("Elige una opción: ")

        if opcion == "1":
            id = input("ID: ")
            nombre = input("Nombre: ")
            try:
                cantidad = int(input("Cantidad: "))
                precio = float(input("Precio: "))
                inventario.agregar_producto(id, nombre, cantidad, precio)
            except ValueError:
                print("Error: La cantidad debe ser número entero y el precio un número decimal.")

        elif opcion == "2":
            id = input("ID a eliminar: ")
            inventario.eliminar_producto(id)

        elif opcion == "3":
            id = input("ID a actualizar: ")
            cantidad = input("Nueva cantidad (dejar vacío si no cambia): ")
            precio = input("Nuevo precio (dejar vacío si no cambia): ")
            try:
                nueva_cantidad = int(cantidad) if cantidad else None
                nuevo_precio = float(precio) if precio else None
                inventario.actualizar_producto(id, nueva_cantidad, nuevo_precio)
            except ValueError:
                print("Error: Datos inválidos para cantidad o precio.")

        elif opcion == "4":
            nombre = input("Nombre a buscar: ")
            inventario.buscar_producto(nombre)

        elif opcion == "5":
            inventario.mostrar_todos()

        elif opcion == "6":
            print("Saliendo... ¡Gracias por usar nuestro sistema de inventarios JR 2025!")
            break

        else:
            print("Opción inválida.")


if __name__ == "__main__":
    menu()
