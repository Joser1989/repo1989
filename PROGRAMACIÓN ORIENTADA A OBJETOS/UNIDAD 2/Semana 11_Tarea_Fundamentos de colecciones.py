import json

# -------------------------------
# Clase Producto
# -------------------------------
class Producto:
    def __init__(self, id_producto, nombre, cantidad, precio):
        self.id_producto = id_producto
        self.nombre = nombre
        self.cantidad = cantidad
        self.precio = precio

    # Métodos Getters y Setters
    def get_id(self):
        return self.id_producto

    def get_nombre(self):
        return self.nombre

    def get_cantidad(self):
        return self.cantidad

    def get_precio(self):
        return self.precio

    def set_cantidad(self, nueva_cantidad):
        self.cantidad = nueva_cantidad

    def set_precio(self, nuevo_precio):
        self.precio = nuevo_precio

    # Representación en texto
    def __str__(self):
        return f"ID: {self.id_producto} | Nombre: {self.nombre} | Cantidad: {self.cantidad} | Precio: ${self.precio:.2f}"


# -------------------------------
# Classe Inventario
# -------------------------------
class Inventario:
    def __init__(self):
        # Diccionario con clave = ID, valor = objeto Producto
        self.productos = {}

    def añadir_producto(self, producto):
        if producto.get_id() in self.productos:
            print("❌ Error: Ya existe un producto con ese ID.")
        else:
            self.productos[producto.get_id()] = producto
            print("✅ Producto añadido correctamente.")

    def eliminar_producto(self, id_producto):
        if id_producto in self.productos:
            del self.productos[id_producto]
            print("✅ Producto eliminado.")
        else:
            print("❌ No existe un producto con ese ID.")

    def actualizar_producto(self, id_producto, nueva_cantidad=None, nuevo_precio=None):
        if id_producto in self.productos:
            producto = self.productos[id_producto]
            if nueva_cantidad is not None:
                producto.set_cantidad(nueva_cantidad)
            if nuevo_precio is not None:
                producto.set_precio(nuevo_precio)
            print("✅ Producto actualizado.")
        else:
            print("❌ No existe un producto con ese ID.")

    def buscar_producto(self, nombre):
        resultados = [p for p in self.productos.values() if nombre.lower() in p.get_nombre().lower()]
        if resultados:
            print("🔍 Resultados de la búsqueda:")
            for p in resultados:
                print(p)
        else:
            print("❌ No se encontraron productos con ese nombre.")

    def mostrar_todos(self):
        if self.productos:
            print("📦 Inventario actual:")
            for producto in self.productos.values():
                print(producto)
        else:
            print("❌ El inventario está vacío.")

    # -------------------------------
    # Manejo de Archivos
    # -------------------------------
    def guardar_en_archivo(self, archivo="inventario.json"):
        try:
            with open(archivo, "w") as f:
                json.dump(
                    {id_: {"nombre": p.get_nombre(), "cantidad": p.get_cantidad(), "precio": p.get_precio()}
                     for id_, p in self.productos.items()}, f, indent=4
                )
            print("💾 Inventario guardado en archivo JR.")
        except Exception as e:
            print(f"❌ Error al guardar el archivo: {e}")

    def cargar_desde_archivo(self, archivo="inventario.json"):
        try:
            with open(archivo, "r") as f:
                data = json.load(f)
                self.productos = {
                    id_: Producto(id_, info["nombre"], info["cantidad"], info["precio"])
                    for id_, info in data.items()
                }
            print("📂 Inventario cargado desde archivo.")
        except FileNotFoundError:
            print("⚠ No existe archivo de inventario, se iniciará vacío.")
        except Exception as e:
            print(f"❌ Error al cargar el archivo: {e}")


# -------------------------------
# Menú Interactivo
# -------------------------------
def menu():
    inventario = Inventario()
    inventario.cargar_desde_archivo()

    while True:
        print("\n===== MENÚ DE INVENTARIO =====")
        print("1. Añadir producto")
        print("2. Eliminar producto")
        print("3. Actualizar producto")
        print("4. Buscar producto por nombre")
        print("5. Mostrar todos los productos")
        print("6. Guardar inventario en archivo")
        print("7. Salir")
        opcion = input("Seleccione una opción: ")

        if opcion == "1":
            id_producto = input("Ingrese ID del producto: ")
            nombre = input("Ingrese nombre del producto: ")
            cantidad = int(input("Ingrese cantidad: "))
            precio = float(input("Ingrese precio: "))
            producto = Producto(id_producto, nombre, cantidad, precio)
            inventario.añadir_producto(producto)

        elif opcion == "2":
            id_producto = input("Ingrese ID del producto a eliminar: ")
            inventario.eliminar_producto(id_producto)

        elif opcion == "3":
            id_producto = input("Ingrese ID del producto a actualizar: ")
            nueva_cantidad = input("Ingrese nueva cantidad (o Enter para no cambiar): ")
            nuevo_precio = input("Ingrese nuevo precio (o Enter para no cambiar): ")

            cantidad_final = int(nueva_cantidad) if nueva_cantidad else None
            precio_final = float(nuevo_precio) if nuevo_precio else None
            inventario.actualizar_producto(id_producto, cantidad_final, precio_final)

        elif opcion == "4":
            nombre = input("Ingrese nombre del producto a buscar: ")
            inventario.buscar_producto(nombre)

        elif opcion == "5":
            inventario.mostrar_todos()

        elif opcion == "6":
            inventario.guardar_en_archivo()

        elif opcion == "7":
            inventario.guardar_en_archivo()
            print("👋 Saliendo del sistema JR. ¡Vuelve Pronto!")
            break

        else:
            print("❌ Opción no válida, intente de nuevo.")


# -------------------------------
# Ejecución del Programa
# -------------------------------
if __name__ == "__main__":
    menu()
