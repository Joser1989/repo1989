import tkinter as tk
from tkinter import messagebox

# --- Funciones de eventos ---
def agregar_dato():
    """Agrega el dato ingresado a la lista si no está vacío."""
    dato = entrada.get().strip()
    if dato:
        lista.insert(tk.END, dato)  # Inserta al final de la lista
        entrada.delete(0, tk.END)   # Limpia el campo de texto
    else:
        messagebox.showwarning("Advertencia", "Por favor, ingresa un dato antes de agregarlo.")

def limpiar_dato():
    """
    Limpia la selección de la lista o el campo de texto.
    Si hay un elemento seleccionado en la lista, lo elimina.
    Si no hay seleción, limpia el campo de texto.
    """
    seleccion = lista.curselection()
    if seleccion:
        lista.delete(seleccion)
    else:
        entrada.delete(0, tk.END)

# --- Ventana principal ---
ventana = tk.Tk()
ventana.title("Gestor Básico de Datos POO by J.R.")
ventana.geometry("400x400")  # Tamaño de la ventana
ventana.resizable(False, False)

# --- Etiqueta de instrucciones ---
etiqueta = tk.Label(ventana, text="Ingrese un dato y presione 'Agregar':", font=("Arial", 11))
etiqueta.pack(pady=10)

# --- Campo de twxto ---
entrada = tk.Entry(ventana, width=40)
entrada.pack(pady=5)

# --- Botones ---
frame_botones = tk.Frame(ventana)
frame_botones.pack(pady=10)

boton_agregar = tk.Button(frame_botones, text="Agregar", width=12, command=agregar_dato)
boton_agregar.grid(row=0, column=0, padx=5)

boton_limpiar = tk.Button(frame_botones, text="Limpiar", width=12, command=limpiar_dato)
boton_limpiar.grid(row=0, column=1, padx=5)

# --- Lista para mostrar datos ---
lista = tk.Listbox(ventana, width=50, height=10)
lista.pack(pady=10)

# --- Inicia la aplicación ---
ventana.mainloop()
