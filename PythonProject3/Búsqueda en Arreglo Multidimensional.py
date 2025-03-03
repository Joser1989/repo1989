def buscar_valor(matriz, valor):
    for i in range(len(matriz)):
        for j in range(len(matriz[i])):
            if matriz[i][j] == valor:
                return i, j
    return None

# Matriz 3x3 de ejemplo
matriz = [
    [5, 8, 2],
    [4, 9, 6],
    [7, 1, 3]
]

# Valor a buscar
valor_buscar = int(input("Ingrese el valor a buscar: "))

# Búsqueda del valor en la matriz
posicion = buscar_valor(matriz, valor_buscar)

if posicion:
    print(f"Valor {valor_buscar} encontrado en la posición {posicion}")
else:
    print(f"Valor {valor_buscar} no encontrado en la matriz,Dear Friend.")
