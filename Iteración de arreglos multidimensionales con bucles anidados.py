# Definimos la matriz 3D con datos de ejemplo
# Estructura: [ciudades][días][semanas]
temperaturas = [
    [  # Ciudad 1
        [20, 22, 21, 23, 24, 22, 21],  # Semana 1
        [23, 24, 25, 23, 22, 24, 23]   # Semana 2
    ],
    [  # Ciudad 2
        [25, 26, 24, 25, 26, 25, 24],  # Semana 1
        [27, 28, 26, 25, 27, 26, 25]   # Semana 2
    ],
    [  # Ciudad 3
        [18, 19, 20, 19, 18, 20, 19],  # Semana 1
        [20, 21, 22, 20, 19, 21, 20]   # Semana 2
    ]
]

# Nombres para mejor legibilidad
ciudades = ["QUITO", "AMSTERDAM", "BRASILIA"]
dias = ["Lun", "Mar", "Mié", "Jue", "Vie", "Sáb", "Dom"]
semanas = ["Semana 1", "Semana 2"]

# Calcular y mostrar promedios por ciudad y semana
for i in range(len(ciudades)):  # Iterar por ciudades
    print(f"\nTemperaturas promedio para {ciudades[i]}:")
    for j in range(len(semanas)):  # Iterar por semanas
        suma = 0
        # Calcular suma de temperaturas para esta ciudad y semana
        for k in range(len(dias)):  # Iterar por días
            suma += temperaturas[i][j][k]
        # Calcular promedio
        promedio = suma / len(dias)
        print(f"{semanas[j]}: {promedio:.1f}°C")

# Opcional: Mostrar todas las temperaturas detalladas
print("\nDatos completos:")
for i in range(len(ciudades)):
    print(f"\n{ciudades[i]}:")
    for j in range(len(semanas)):
        print(f"{semanas[j]}:", end=" ")
        for k in range(len(dias)):
            print(f"{dias[k]}: {temperaturas[i][j][k]}°C", end="  ")
        print()  # Nueva línea después de cada semana
print ("Fin de Datos del tiempo"),
