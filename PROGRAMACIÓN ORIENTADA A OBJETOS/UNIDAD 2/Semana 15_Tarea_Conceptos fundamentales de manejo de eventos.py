"""
todolist_tkinter.py
Aplicaión GUI de ListA de Tareas usando Tkinter.

Características:
- Añadir tareas via botón o presionando Enter.
- Marcar tareas como completadas via botón o doble clic (toggle).
- Eliminar tareas via botón o tecla Supr/Del.
- Selección múltiple para marcar/eliminar varias tareas a la vez.

Diseño y decisiones:
- Modelo de datos simple: lista de dicts {'text': ..., 'done': bool}.
- Listbox para mostrar tareas (por simplicidad y claridad).
- Uso de fonts normales/tachado (overstrike) cuando el sistema lo soporte para mostrar tareas completadas.
- Se intenta aplicar estilos por índice con listbox.itemconfig(), pero el código tolera fallos en plataformas donde no esté disponible.

"""

import tkinter as tk
from tkinter import ttk, messagebox, font


class TodoApp:
    """Clase principal de la aplicación.

    Contiene la interfaz (widgets) y la lógica de eventos.
    """

    def __init__(self, root: tk.Tk):
        self.root = root
        self.root.title("Lista de Tareas de José Ramírez UEA")
        self.root.geometry("480x420")
        # Evitamos redimensionamiento para mantener el layout simple
        self.root.resizable(False, False)

        # Modelo de datos: lista de tareas (cada una es un dict con 'text' y 'done')
        self.tasks = []

        # Fuentes: normal y con tachado (overstrike) para tareas completadas.
        # No es obligatorio que el sistema soporte overstrike; en ese caso
        # el código seguirá funcionando pero sin tachado visual.
        self.font_normal = font.Font(family="Helvetica", size=11)
        self.font_strike = font.Font(family="Helvetica", size=11, overstrike=1)

        # --- Parte superior: entrada + botón Añadir ---
        top_frame = ttk.Frame(self.root, padding=(10, 10))
        top_frame.pack(fill='x')

        # Campo de entrada para nuevas tareas
        self.entry = ttk.Entry(top_frame)
        self.entry.pack(side='left', fill='x', expand=True)
        # Permitir añadir con la tecla Enter
        self.entry.bind('<Return>', self.on_enter)

        add_btn = ttk.Button(top_frame, text='Añadir Tarea', command=self.add_task)
        add_btn.pack(side='left', padx=(8, 0))

        # --- Área central: Listbox con scrollbar ---
        middle_frame = ttk.Frame(self.root, padding=(10, 0, 10, 0))
        middle_frame.pack(fill='both', expand=True)

        # Listbox permite selección múltiple (EXTENDED)
        self.listbox = tk.Listbox(middle_frame, selectmode=tk.EXTENDED, activestyle='none')
        self.listbox.pack(side='left', fill='both', expand=True)

        # Scrollbar para la lista
        scrollbar = ttk.Scrollbar(middle_frame, orient='vertical', command=self.listbox.yview)
        scrollbar.pack(side='right', fill='y')
        self.listbox.config(yscrollcommand=scrollbar.set)

        # Eventos en la lista:
        # - doble clic para alternar completado (mejora de uabilidad)
        # - tecla Supr/Del para eliminar la(s) tarea(s) seleccionada(s)
        self.listbox.bind('<Double-Button-1>', self.on_double_click)
        # Cuando la listbox tenga foco, pulsar Supr eliminará la selección
        self.listbox.bind('<Delete>', self.delete_task_event)

        # También permitimos pulsar Supr incluso si la listbox no tiene foco
        self.root.bind('<Delete>', self.delete_task_event)

        # --- Parte inferior: botones de acción ---
        bottom_frame = ttk.Frame(self.root, padding=(10, 10))
        bottom_frame.pack(fill='x')

        complete_btn = ttk.Button(bottom_frame, text='Marcar como Completada', command=self.mark_completed)
        complete_btn.pack(side='left')

        delete_btn = ttk.Button(bottom_frame, text='Eliminar Tarea', command=self.delete_task)
        delete_btn.pack(side='left', padx=(8, 0))

        # Label de ayuda rápida
        help_label = ttk.Label(bottom_frame, text='Enter = Añadir · Doble clic = Toggle completada · Supr = Eliminar')
        help_label.pack(side='right')

        # Inicializamos con algunas tareas de ejemplo (opcional)
        # self.tasks = [{'text': 'Ejemplo: Comprar pan', 'done': False}, {'text': 'Ejemplo: Estudiar matemáticas', 'done': True}]
        # self.refresh_listbox()

    # ------------- Lógica de la aplicación -------------
    def add_task(self, event=None):
        """Añade la tarea que haya en el Entry.

        Limpiamos espacios en blanco y mostramos una alerta si está vacío.
        Después actualizamos la vista (listbox).
        """
        text = self.entry.get().strip()
        if not text:
            messagebox.showwarning('Aviso', 'La tarea está vacía. Escribe algo antes de añadir.')
            return

        # Añadimos al modelo
        self.tasks.append({'text': text, 'done': False})

        # Limpiamos el entry y actualizamos la lista
        self.entry.delete(0, tk.END)
        self.refresh_listbox()

    def refresh_listbox(self):
        """Refresca la Listbox a partir del modelo self.tasks.

        Para cada tarea mostramos un prefijo '[ ]' o '[✓]' y tratamos de aplicar
        un estilo (color y/o fuente tachada) cuando la tarea está completada.
        """
        self.listbox.delete(0, tk.END)
        for i, task in enumerate(self.tasks):
            prefix = '[✓]' if task['done'] else '[ ]'
            display = f"{prefix} {task['text']}"
            self.listbox.insert(tk.END, display)

            # Intentamos aplicar estilo visual para tareas completadas.
            # Algunos backends de Tkinter aceptan itemconfig con 'fg' y 'font'.
            # Si no está soportado, capturamos la excepción y continuamos.
            try:
                if task['done']:
                    self.listbox.itemconfig(i, fg='gray', font=self.font_strike)
                else:
                    self.listbox.itemconfig(i, fg='black', font=self.font_normal)
            except Exception:
                # Si el sistema no soporta itemconfig por índice, no es crítico:
                # la información sigue siendo visible mediante el prefijo [✓]/[ ].
                pass

    def get_selected_indices(self):
        """Devuelve la lista de índices seleccionados (como ints).

        Usamos map(int, ...) para convertir los índices devueltos por curselection().
        """
        return list(map(int, self.listbox.curselection()))

    def mark_completed(self, event=None):
        """Marca (o desmarca) como completadas las tareas seleccionadas.

        Hacemos toggle de su estado "done". Si no hay selección mostramos un aviso.
        """
        indices = self.get_selected_indices()
        if not indices:
            messagebox.showinfo('Info', 'Selecciona al menos una tarea para marcar/desmarcar.')
            return

        # Alternamos el estado done para cada índice seleccionado
        for idx in indices:
            # Protección por si la lista cambió y el índice ya no existe
            if 0 <= idx < len(self.tasks):
                self.tasks[idx]['done'] = not self.tasks[idx]['done']

        self.refresh_listbox()

    def delete_task(self, event=None):
        """Elimina la(s) tarea(s) seleccionada(s) tras pedir confirmación.

        Eliminamos de la estructura de datos en orden inverso para no romper
        los índices al ir borando.
        """
        indices = self.get_selected_indices()
        if not indices:
            messagebox.showinfo('Info', 'Selecciona al menos una tarea para eliminar.')
            return

        if not messagebox.askyesno('Confirmar', '¿Deseas eliminar la(s) tarea(s) seleccionada(s)?'):
            return

        for idx in sorted(indices, reverse=True):
            if 0 <= idx < len(self.tasks):
                del self.tasks[idx]

        self.refresh_listbox()

    # Envoltorios para eventos (cuando el binding pasa un event)
    def on_enter(self, event):
        # Añadir tarea usando Enter
        self.add_task()

    def on_double_click(self, event):
        # Doble clic: averiguamos el índice más cercano y lo alternamos
        idx = self.listbox.nearest(event.y)
        if 0 <= idx < len(self.tasks):
            self.tasks[idx]['done'] = not self.tasks[idx]['done']
            self.refresh_listbox()

    def delete_task_event(self, event):
        # Al recibir un evento de tecla (Delete), llamamos al méodo de borradop.
        # Retornamos 'break' para evitar que otros manejadores procesen la tecla
        # si deseamos bloquear comportamiento por defecto (no estríctamente necesario here).
        self.delete_task()
        return 'break'


if __name__ == '__main__':
    root = tk.Tk()
    app = TodoApp(root)
    root.mainloop()


