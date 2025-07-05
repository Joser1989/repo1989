# Ejemplo de Programación Orientada a Objetos (POO) en Python.
# Conceptos aplicados:
# 1. Clase Base y Clase Derivada (Herencia)
# 2. Encapsulación de atributos
# 3. Polimorfismo mediante sobrescritura de métodos
# vehiculos_interactivo

import random

# Clase base
class Vehiculo:
    def __init__(self, marca, modelo, anio):
        self.marca = marca
        self.modelo = modelo
        self.anio = anio
        self.__numero_serie = self.generar_numero_serie()

    def generar_numero_serie(self):
        return str(random.randint(1000000, 9999999))

    # Mostrar información general
    def mostrar_info(self):
        print(f"🚘 Marca: {self.marca} | Modelo: {self.modelo} | Año: {self.anio}")

    # Método base arrancar (será sobrescrito)
    def arrancar(self):
        print("🔑 El vehículo está arrancando...")

    # Método para detenerse
    def detenerse(self):
        print(f"🛑 El vehículo {self.marca} se ha detenido.")

    # Encapsulamiento: obtener número de serie
    def obtener_numero_serie(self):
        return self.__numero_serie

    # Encapsulamiento: cambiar número de serie
    def establecer_numero_serie(self, nuevo_numero):
        if len(nuevo_numero) >= 5:
            self.__numero_serie = nuevo_numero
        else:
            print("❌ El número de serie es demasiado corto.")


# Clase derivada: Auto
class Auto(Vehiculo):
    def __init__(self, marca, modelo, anio, puertas):
        super().__init__(marca, modelo, anio)
        self.puertas = puertas

    def arrancar(self):
        print(f"🚗 El auto {self.marca} {self.modelo} posee un motor de 200 hp.")

    def mostrar_info(self):
        super().mostrar_info()
        print(f"🚪 Puertas: {self.puertas}")

    def detenerse(self):
        print(f"🅿️ El auto {self.marca} viene con frenos de disco en sus 4 ruedas.")


# Clase derivada: Motocicleta
class Motocicleta(Vehiculo):
    def __init__(self, marca, modelo, anio, tipo):
        super().__init__(marca, modelo, anio)
        self.tipo = tipo

    def arrancar(self):
        print(f"🏍️ La motocicleta {self.marca} {self.modelo} arranca con botón de encendido.")

    def mostrar_info(self):
        super().mostrar_info()
        print(f"🔧 Tipo: {self.tipo}")

    def detenerse(self):
        print(f"🛵 La motocicleta {self.marca} se detuvo usando freno trasero.")


# Programa principal con interacción
if __name__ == "__main__":
    print("🚦 Bienvenido al sistema nacional de registro de vehículos de la ANT 🚦\n")

    # Crear Auto con datos del usuario
    marca_auto = input("Ingresa la marca de tu auto: ")
    modelo_auto = input("Ingresa el modelo de tu auto: ")
    anio_auto = input("Ingresa el año de tu auto: ")
    puertas_auto = int(input("¿Cuántas puertas tiene tu auto?: "))
    auto_usuario = Auto(marca_auto, modelo_auto, anio_auto, puertas_auto)

    print("\n✅ Información del Auto:")
    auto_usuario.mostrar_info()
    auto_usuario.arrancar()
    auto_usuario.detenerse()
    print(f"🔒 Número de serie: {auto_usuario.obtener_numero_serie()}\n")

    # Crear Motocicleta con datos del usuario
    marca_moto = input("Ingresa la marca de tu motocicleta: ")
    modelo_moto = input("Ingresa el modelo de tu motocicleta: ")
    anio_moto = input("Ingresa el año de tu motocicleta: ")
    tipo_moto = input("¿Qué tipo de motocicleta es? (Deportiva, Scooter, etc.): ")
    moto_usuario = Motocicleta(marca_moto, modelo_moto, anio_moto, tipo_moto)

    print("\n✅ Información de la Motocicleta:")
    moto_usuario.mostrar_info()
    moto_usuario.arrancar()
    moto_usuario.detenerse()
    print(f"🔒 Número de serie: {moto_usuario.obtener_numero_serie()}\n")

    # Prueba de cambiar número de serie del auto
    nuevo_numero = input("Ingresa un nuevo número de serie para tu auto (mínimo 5 dígitos): ")
    auto_usuario.establecer_numero_serie(nuevo_numero)
    print(f"✅ Nuevo número de serie: {auto_usuario.obtener_numero_serie()}")

    print("\n🚗🏍️ Programa finalizado con éxito. Gracias por utilizar el sistema nacional de verificacion vehicular de la ANT. 🚦")

