# 1. Crear un diccionario con información personal ficticia
informacion_personal = {
    "nombre": "Linda Ramírez",
    "edad": 26,
    "ciudad": "Madrid",
    "profesion": "Ingeniera de Software"
}

# 2. Acceder y modificar el valor de "ciudad"
# Mostramos la ciudad original
print(f"Ciudad original: {informacion_personal['ciudad']}")
# Modificamos la ciudad a Barcelona
informacion_personal["ciudad"] = "Barcelona"

# 3. Agregar una nueva clave-valor (aunque "profesion" ya existe,
# voy a agregar "hobby" como nueva información)
informacion_personal["hobby"] = "Fotografía"

# 4. Verificar existencia de la clave "telefono"
if "telefono" not in informacion_personal:
    # Si no existe, la agregamos con un número ficticio
    informacion_personal["telefono"] = "0985367891"

# 5. Eliminar la clave "edad"
del informacion_personal["edad"]

# 6. Imprimir el diccionario final
print("\nDiccionario final:")
print(informacion_personal)
print("Información Consultada...Proporcionada")
