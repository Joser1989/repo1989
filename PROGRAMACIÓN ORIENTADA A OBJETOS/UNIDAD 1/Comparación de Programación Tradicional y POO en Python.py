# clima_tradicional.py

def ingresar_temperaturas():
    """
    Solicita al usuario ingresar la temperatura para cada día de la semana.
    Retorna una lista con las temperaturas.
    """
    dias = ["Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo"]
    temperaturas = []
    for dia in dias:
        temp = float(input(f"Ingrese la temperatura del {dia}: "))
        temperaturas.append(temp)
    return temperaturas

def calcular_promedio(temperaturas):
    """
    Calcula el promedio de las temperaturas semanales.
    """
    return sum(temperaturas) / len(temperaturas)

def mostrar_resultado(promedio):
    print(f"\nEl promedio de temperatura semanal es: {promedio:.2f}°C")

# Programa principal
def main():
    temperaturas = ingresar_temperaturas()
    promedio = calcular_promedio(temperaturas)
    mostrar_resultado(promedio)

if __name__ == "__main__":
    main()
