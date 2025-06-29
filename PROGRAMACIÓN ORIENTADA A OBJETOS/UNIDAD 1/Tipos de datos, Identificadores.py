# Programa para calcular el área de un círculo para la materia POO.
# Solicita al usuario el radio, calcula el área usando la fórmula A = π * r^2,
# muestra el resultado y confirma si el área es mayor a un valor de referencia.

import math  # Importa la biblioteca para usar pi

def calcular_area_circulo(radio):
    """
    Calcula el área de un círculo dado su radio.
    :param radio: Radio del círculo (float)
    :return: Área del círculo (float)
    """
    area = math.pi * radio ** 2
    return area

# Solicitar datos al usuario
nombre_usuario = input("Ingrese su nombre: ")  # str
radio_circulo = float(input("Ingrese el radio del círculo en cm: "))  # float

# Calcular el área
area_resultado = calcular_area_circulo(radio_circulo)

# Mostrar resultado
print(f"Hola {nombre_usuario}, el área del círculo es: {area_resultado:.2f} cm²")

# Usar tipo booleano para verificar si el área es mayor a 100 cm²
es_area_grande = area_resultado > 100  # bool

print(f"¿El área es mayor a 100 cm²? {es_area_grande}")

# Ejemplo de integer: contar cuántas cifras tiene el área (entero)
cifras_area = len(str(int(area_resultado)))  # int
print(f"El área tiene {cifras_area} cifras enteras.")
