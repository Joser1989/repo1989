# Modelado de una Tienda Online utilizando Programación Orientada a Objetos

# Clase Producto representa un artículo en venta
class Producto:
    def __init__(self, nombre, precio, stock):
        self.nombre = nombre
        self.precio = precio
        self.stock = stock

    def mostrar_info(self):
        print(f"{self.nombre} - ${self.precio} (Stock: {self.stock})")

    def actualizar_stock(self, cantidad):
        self.stock += cantidad


# Clase Cliente representa un usuario que puede comprar productos
class Cliente:
    def __init__(self, nombre):
        self.nombre = nombre
        self.carrito = []  # Lista de tuplas: (producto, cantidad)

    def agregar_al_carrito(self, producto, cantidad):
        if producto.stock >= cantidad:
            self.carrito.append((producto, cantidad))
            producto.actualizar_stock(-cantidad)
            print(f"{cantidad} unidades de '{producto.nombre}' añadidas al carrito.")
        else:
            print(f"No hay suficiente stock para '{producto.nombre}'.")

    def ver_carrito(self):
        print(f"\nCarrito de {self.nombre}:")
        total = 0
        for producto, cantidad in self.carrito:
            subtotal = producto.precio * cantidad
            print(f"- {producto.nombre}: {cantidad} x ${producto.precio} = ${subtotal}")
            total += subtotal
        print(f"Total a pagar: ${total}")


# Clase Tienda representa una tienda con productos y clientes
class Tienda:
    def __init__(self, nombre):
        self.nombre = nombre
        self.productos = []
        self.clientes = []

    def agregar_producto(self, producto):
        self.productos.append(producto)

    def registrar_cliente(self, cliente):
        self.clientes.append(cliente)

    def mostrar_productos(self):
        print(f"\nProductos disponibles en {self.nombre}:")
        for producto in self.productos:
            producto.mostrar_info()


# Ejemplo de uso del sistema
if __name__ == "__main__":
    # Crear tienda
    mi_tienda = Tienda("sUper tiEndA POO")

    # Crear productos
    p1 = Producto("Laptop Asus", 750, 5)
    p2 = Producto("Mouse x200", 25, 20)
    p3 = Producto("Teclado Gamer", 40, 10)

    # Agregar productos a la tienda
    mi_tienda.agregar_producto(p1)
    mi_tienda.agregar_producto(p2)
    mi_tienda.agregar_producto(p3)

    # Mostrar productos disponibles
    mi_tienda.mostrar_productos()

    # Crear cliente
    cliente1 = Cliente("Walter Nuñez")

    # Registrar cliente en la tienda
    mi_tienda.registrar_cliente(cliente1)

    # Cliente agrega productos al carrito
    cliente1.agregar_al_carrito(p1, 1)
    cliente1.agregar_al_carrito(p2, 2)

    # Mostrar carrito del cliente
    cliente1.ver_carrito()

    # Mostrar productos nuevamente para ver cambio en stock
    mi_tienda.mostrar_productos()
