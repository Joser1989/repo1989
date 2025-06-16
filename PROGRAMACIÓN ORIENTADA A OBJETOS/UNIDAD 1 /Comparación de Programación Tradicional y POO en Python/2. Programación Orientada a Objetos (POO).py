# clima_poo.py

class DiaClima:
    """
    Clase que representa la información del clima para un día.
    """

    def __init__(self, dia, temperatura):
        self.__dia = dia
        self.__temperatura = temperatura

    def obtener_temperatura(self):
        return self.__temperatura

    def __str__(self):
        return f"{self.__dia}: {self.__temperatura}°C"

class SemanaClima:
    """
    Clase que representa una semana de datos climáticos.
    """

    def __init__(self):
        self.__dias = []

    def agregar_dia(self, dia, temperatura):
        self.__dias.append(DiaClima(dia, temperatura))

    def promedio_semanal(self):
        total = sum(dia.obtener_temperatura() for dia in self.__dias)
        return total / len(self.__dias) if self.__dias else 0

    def mostrar_dias(self):
        print("\nTemperaturas registradas:")
        for dia in self.__dias:
            print(dia)

# Programa principal
def main():
    dias_semana = ["Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo"]
    semana = SemanaClima()

    for dia in dias_semana:
        temp = float(input(f"Ingrese la temperatura del {dia}: "))
        semana.agregar_dia(dia, temp)

    semana.mostrar_dias()
    promedio = semana.promedio_semanal()
    print(f"\nEl promedio de temperatura semanal es: {promedio:.2f}°C")

if __name__ == "__main__":
    main()
