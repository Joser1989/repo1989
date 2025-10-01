#(Semana 16) Tarea: Manejadores de eventos
#Tarea: Aplicación GUI para Gestión de Tareas con Atajos de Teclado
#Objetivo: Desarrollar una aplicación GUI que permita a los usuarios gestionar una lista de tareas pendientes. La aplicación deberá permitir añadir nuevas tareas, marcar tareas como completadas, y eliminar tareas utilizando tanto la interfaz gráfica (clics de botón) como atajos de teclado.

import tkinter as tk
from tkinter import messagebox

class GestorTareas:
    def __init__(self, root):
        self.root = root
        self.root.title("Gestor de Tareas José Ramírez")
        self.root.geometry("400x400")

        # Campo de entrada
        self.entry = tk.Entry(root, width=40)
        self.entry.pack(pady=10)
        self.entry.focus()

        # Botones
        frame_botones = tk.Frame(root)
        frame_botones.pack()

        btn_add = tk.Button(frame_botones, text="Añadir", command=self.añadir_tarea)
        btn_add.grid(row=0, column=0, padx=5)

        btn_complete = tk.Button(frame_botones, text="Completar", command=self.completar_tarea)
        btn_complete.grid(row=0, column=1, padx=5)

        btn_delete = tk.Button(frame_botones, text="Delete", command=self.eliminar_tarea)
        btn_delete.grid(row=0, column=2, padx=5)

        # Lista de tareas
        self.lista = tk.Listbox(root, selectmode=tk.SINGLE, width=50, height=15)
        self.lista.pack(pady=10)

        # Diccionario para estado de las tareas
        self.tareas = {}

        # Atajos de teclado
        self.entry.bind("<Return>", lambda event: self.añadir_tarea())
        self.root.bind("<c>", lambda event: self.completar_tarea())
        self.root.bind("<C>", lambda event: self.completar_tarea())  # mayúsculas también
        self.root.bind("<Delete>", lambda event: self.eliminar_tarea())
        self.root.bind("<d>", lambda event: self.eliminar_tarea())
        self.root.bind("<D>", lambda event: self.eliminar_tarea())
        self.root.bind("<Escape>", lambda event: self.cerrar())

    def añadir_tarea(self):
        tarea = self.entry.get().strip()
        if tarea:
            self.lista.insert(tk.END, tarea)
            self.tareas[tarea] = False  # False = pendiente
            self.entry.delete(0, tk.END)
        else:
            messagebox.showwarning("Advertencia", "No puedes añadir una tarea vacía.")

    def completar_tarea(self):
        seleccion = self.lista.curselection()
        if seleccion:
            index = seleccion[0]
            tarea = self.lista.get(index)
            if not self.tareas[tarea]:
                self.tareas[tarea] = True
                self.lista.delete(index)
                self.lista.insert(index, f"✔ {tarea}")
                self.lista.itemconfig(index, fg="green")
            else:
                self.tareas[tarea] = False
                tarea_sin_check = tarea.replace("✔ ", "")
                self.lista.delete(index)
                self.lista.insert(index, tarea_sin_check)
                self.lista.itemconfig(index, fg="black")
        else:
            messagebox.showinfo("Información", "Selecciona una tarea para marcarla,Dear friend.")

    def eliminar_tarea(self):
        seleccion = self.lista.curselection()
        if seleccion:
            index = seleccion[0]
            tarea = self.lista.get(index).replace("✔ ", "")
            self.lista.delete(index)
            if tarea in self.tareas:
                del self.tareas[tarea]
        else:
            messagebox.showinfo("Información", "Selecciona una tarea para eliminarla.")

    def cerrar(self):
        self.root.quit()

if __name__ == "__main__":
    root = tk.Tk()
    app = GestorTareas(root)
    root.mainloop()
