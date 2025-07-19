# Dashboard.py
# Desarrollado por: José Ramírez
# Fecha: 17 de julio del 2025
# Descripción:
# Este archivo permite organizar y visualizar scripts del proyecto
# de Programación Orientada a Objetos.
# conteo de líneas y rutas ampliadas.

import os


def mostrar_codigo(ruta_script):
    """
    Muestra el contenido del archivo Python seleccionado
    y cuenta cuántas líneas de código tiene.
    """
    ruta_script_absoluta = os.path.abspath(ruta_script)
    try:
        with open(ruta_script_absoluta, 'r') as archivo:
            print(f"\n📂 --- Código de {ruta_script} ---\n")
            print(archivo.read())
            contar_lineas(ruta_script_absoluta)
    except FileNotFoundError:
        print("⚠️ El archivo no se encontró.")
    except Exception as e:
        print(f"Ocurrió un error al leer el archivo: {e}")


def contar_lineas(ruta_script):
    """
    Cuenta y muestra el número de líneas de un archivo.
    """
    try:
        with open(ruta_script, 'r') as archivo:
            lineas = archivo.readlines()
            print(f"\n📊 Este archivo tiene {len(lineas)} líneas de código.\n")
    except Exception as e:
        print(f"No se pudo contar las líneas: {e}")


def mostrar_menu():
    """
    Muestra el menú principal del Dashboard con rutas personalizadas.
    El usuario puede elegir qué script ver.
    """
    # Ruta base del proyecto
    ruta_base = os.path.dirname(__file__)

    # Opciones de scripts disponibles
    opciones = {
        '1': 'UNIDAD 1/1.2. Tecnicas de Programacion/1.2.1. Ejemplo Tecnicas de Programacion.py',
        '2': 'UNIDAD 2/Herencia/HerenciaEjemplo.py',
        '3': 'UNIDAD 3/Encapsulacion/EncapsulacionDemo.py',
        '4': 'UNIDAD 4/Polimorfismo/PolimorfismoEjemplo.py',
        '5': 'UNIDAD 5/Abstraccion/AbstraccionDemo.py'
        }

    while True:
        print("\n===============================")
        print("📌 Welcome to my DASHBOARD POO  📌")
        print("===============================\n")
        print("Selecciona una opción para ver el código:\n")

        for key in opciones:
            print(f"{key} - {opciones[key]}")

        print("0 - Salir del Dashboard")

        eleccion = input("\n👉 Escribe tu opción here,please: ")

        if eleccion == '0':
            print("\n👋 Saliendo del Dashboard. ¡Hasta pronto dear friend UEAno!")
            break
        elif eleccion in opciones:
            # Construye la ruta completa
            ruta_script = os.path.join(ruta_base, opciones[eleccion])
            mostrar_codigo(ruta_script)
        else:
            print("❌ Opción no válida. Por favor, elige una opción del menú.")


# Punto de entrada del programa
if __name__ == "__main__":
    mostrar_menu()
