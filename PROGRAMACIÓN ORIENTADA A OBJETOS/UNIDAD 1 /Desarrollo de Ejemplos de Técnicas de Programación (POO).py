# batalla.py

class Criatura:
    """
    Clase base que representa a una criatura fantástica.
    """

    def __init__(self, nombre, ataque, magia, armadura, salud):
        self.nombre = nombre
        self.ataque = ataque
        self.magia = magia
        self.armadura = armadura
        self.salud = salud

    def mostrar_atributos(self):
        """Muestra los atributos de la criatura."""
        print(f"\n{self.nombre}")
        print("·Ataque:", self.ataque)
        print("·Magia:", self.magia)
        print("·Armadura:", self.armadura)
        print("·Salud:", self.salud)

    def esta_con_vida(self):
        """Indica si la criatura aún está viva."""
        return self.salud > 0

    def recibir_ataque(self, daño):
        """Reduce la salud de la criatura al recibir daño."""
        daño_final = max(0, daño - self.armadura)
        self.salud -= daño_final
        print(f"{self.nombre} recibió {daño_final} de daño.")
        if self.salud <= 0:
            self.salud = 0
            print(f"{self.nombre} ha caído en batalla.")

    def calcular_daño(self):
        """Calcula el daño base (puede ser sobrescrito)."""
        return self.ataque


class Dragon(Criatura):
    """
    Clase que representa a un dragón.
    """

    def __init__(self, nombre, ataque, magia, armadura, salud, fuego):
        super().__init__(nombre, ataque, magia, armadura, salud)
        self.fuego = fuego

    def calcular_daño(self):
        """Calcula el daño combinando ataque físico y fuego."""
        return self.ataque + self.fuego

    def mostrar_atributos(self):
        super().mostrar_atributos()
        print("·Fuego:", self.fuego)


class Hechicero(Criatura):
    """
    Clase que representa a un hechicero.
    """

    def __init__(self, nombre, ataque, magia, armadura, salud, baston):
        super().__init__(nombre, ataque, magia, armadura, salud)
        self.baston = baston

    def calcular_daño(self):
        """Calcula el daño mágico usando el poder del bastón."""
        return self.magia * self.baston

    def mostrar_atributos(self):
        super().mostrar_atributos()
        print("·Bastón mágico:", self.baston)


def batalla(criatura1, criatura2):
    """
    Simula una batalla por turnos entre dos criaturas.
    """
    turno = 1
    while criatura1.esta_con_vida() and criatura2.esta_con_vida():
        print(f"\n--- Turno {turno} ---")
        print(f"{criatura1.nombre} ataca a {criatura2.nombre}")
        daño = criatura1.calcular_daño()
        criatura2.recibir_ataque(daño)

        if not criatura2.esta_con_vida():
            break

        print(f"{criatura2.nombre} ataca a {criatura1.nombre}")
        daño = criatura2.calcular_daño()
        criatura1.recibir_ataque(daño)

        turno += 1

    # Resultado final
    print("\n🔚 Fin de la batalla")
    if criatura1.esta_con_vida():
        print(f"🏆 Ganador: {criatura1.nombre}")
    elif criatura2.esta_con_vida():
        print(f"🏆 Ganador: {criatura2.nombre}")
    else:
        print("🤝 ¡Empate!")


# === EJECUCIÓN DEL PROGRAMA ===
if __name__ == "__main__":
    dragon_rojo = Dragon("Draco", ataque=18, magia=5, armadura=6, salud=120, fuego=12)
    hechicera = Hechicero("Linda", ataque=4, magia=20, armadura=3, salud=90, baston=5)

    dragon_rojo.mostrar_atributos()
    hechicera.mostrar_atributos()

    batalla(dragon_rojo, hechicera)
