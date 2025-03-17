def calcular_promedio_temperaturas(temperaturas, ciudades, semanas, dias):
    """
    Calcula la temperatura promedio de cada ciudad durante todas las semanas.
    :param temperaturas: Lista tridimensional con temperaturas [ciudades][semanas][días]
    :param ciudades: Lista con nombres de ciudades
    :param semanas: Lista con nombres de semanas
    :param dias: Lista con nombres de días de la semana
    """
    promedios = {}

    for i, ciudad in enumerate(ciudades):  # Iterar sobre ciudades
        suma_total = 0
        total_dias = 0

        for j in range(len(semanas)):  # Iterar sobre semanas
            suma_total += sum(temperaturas[i][j])
            total_dias += len(dias)

        promedio_ciudad = suma_total / total_dias  # Calcular promedio
        promedios[ciudad] = round(promedio_ciudad, 1)

    return promedios


# Datos de prueba
ciudades = ["QUITO", "AMSTERDAM", "BRASILIA"]
dias = ["Lun", "Mar", "Mié", "Jue", "Vie", "Sáb", "Dom"]
semanas = ["Semana 1", "Semana 2"]

# Datos de temperaturas (ejemplo con 2 semanas por ciudad)
temperaturas = [
    [  # QUTO
        [20, 22, 21, 23, 24, 22, 21],  # Semana 1
        [23, 24, 25, 23, 22, 24, 23]  # Semana 2
    ],
    [  # AMSTERDAM
        [25, 26, 24, 25, 26, 25, 24],  # Semana 1
        [27, 28, 26, 25, 27, 26, 25]  # Semana 2
    ],
    [  # BRASILIA
        [18, 19, 20, 19, 18, 20, 19],  # Semana 1
        [20, 21, 22, 20, 19, 21, 20]  # Semana 2
    ]
]

# Llamar a la función
resultado = calcular_promedio_temperaturas(temperaturas, ciudades, semanas, dias)

# Mostrar resultados
print("\nPromedio de temperaturas por ciudad:")
for ciudad, promedio in resultado.items():
    print(f"{ciudad}: {promedio}°C")
print ("Fin de programa del tiempo, Dear Friend")
