# Escritura de Archivo de Texto
# Se crea y escribe en un archivo llamado my_notes.txt
with open("my_notes.txt", "w") as file:
    file.write("First note: Aprender Python es divertido con la supervición del Profe Walter.\n")
    file.write("Second note: Practicar escritura y lectura de archivos me ayudara a desenvolverme mas en esto de la programación.\n")
    file.write("Third note: La practica hace al Maestro.\n")
    file.write("Fourth note: Cerrar always los archivos después de usarlos.\n")

# Lectura de Archivo de Texto
# Se abre el archivo en modo lectura y se leen las líneas una por una
with open("my_notes.txt", "r") as file:
    for line in file:  # Se lee cada línea del archivo
        print(line.strip())  # Se muestra en consola sin los saltos de línea adicionales

# El archivo se cierra automáticamente al salir del bloque with
print ("Fin de Prácticas for Today.")
