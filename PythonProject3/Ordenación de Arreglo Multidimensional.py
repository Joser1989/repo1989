def bubble_sort(fila):
    n = len(fila)
    for i in range(n):
        for j in range(0, n - i - 1):
            if fila[j] > fila[j + 1]:
                fila[j], fila[j + 1] = fila[j + 1], fila[j]

def ordenar_fila(matriz, fila_index):
    if 0 <= fila_index < len(matriz):
        bubble_sort(matriz[fila_index])
    else:
        print("Índice de fila fuera de rango.")

# Matriz 3x3 de ejemplo
matriz = [
    [5, 8, 2],
    [4, 9, 6],
    [7, 1, 3]
]

# Mostrar matriz original
print("Matriz original:")
for fila in matriz:
    print(fila)

# Selección de la fila a ordenar
fila_a_ordenar = int(input("Ingrese el índice de la fila a ordenar (0-2): "))
ordenar_fila(matriz, fila_a_ordenar)

# Mostrar matriz con la fila ordenada
print("\nMatriz después de ordenar la fila seleccionada:")
for fila in matriz:
    print(fila)

