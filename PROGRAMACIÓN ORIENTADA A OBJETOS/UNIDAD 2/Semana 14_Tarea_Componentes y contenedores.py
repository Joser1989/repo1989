"""
Agenda Personal de J.R. - GUI con Tkinter
Archivo: agenda_tkinter.py
Descripción:
    Aplicación de escritorio que permite agregar, ver y eliminar eventos/tareas.
    - Interfaz: Tkinter + ttk.Treeview
    - DatePicker: implementación propia con calendario (no requiere librerías externas)
    - Funcionalidades: agregar evento, eliminar evento seleccionado, confirmar eliminación, salir, (marcas de agua joseramires uea 2025)

Instruciones:
    python agenda_tkinter.py

"""

import tkinter as tk
from tkinter import ttk, messagebox
import calendar
from datetime import datetime


class DatePicker(tk.Toplevel):
    """Ventana emergente simple para seleccionar una fecha.
    No depende de librerías externas; muestra un calendario del mes y permite
    seleccionar un día. Devuelve la fecha seleccionada en formato DD-MM-YYYY.
    """

    def __init__(self, parent, initial_date=None):
        super().__init__(parent)
        self.parent = parent
        self.title("Seleccionar fecha")
        self.resizable(False, False)
        self.transient(parent)
        self.grab_set()

        # Fecha inicial
        if initial_date:
            try:
                self.current = datetime.strptime(initial_date, "%d-%m-%Y")
            except Exception:
                self.current = datetime.today()
        else:
            self.current = datetime.today()

        self.selected_date = None

        # Cabecera: mes y año con botones para navegar
        header = ttk.Frame(self)
        header.pack(padx=8, pady=6)

        self.prev_btn = ttk.Button(header, text="<", width=3, command=self._prev_month)
        self.prev_btn.grid(row=0, column=0)

        self.month_label = ttk.Label(header, text="", width=18, anchor="center")
        self.month_label.grid(row=0, column=1, padx=6)

        self.next_btn = ttk.Button(header, text=">", width=3, command=self._next_month)
        self.next_btn.grid(row=0, column=2)

        # Marco para el calendario
        self.cal_frame = ttk.Frame(self)
        self.cal_frame.pack(padx=8, pady=(0,8))

        self._draw_calendar()

    def _draw_calendar(self):
        # Limpia el frame
        for w in self.cal_frame.winfo_children():
            w.destroy()

        year = self.current.year
        month = self.current.month
        self.month_label.config(text=f"{calendar.month_name[month]} {year}")

        # DÍAS DE LA SEMANA
        days = ['Lun','Mar','Mié','Jue','Vie','Sáb','Dom']
        for i, d in enumerate(days):
            ttk.Label(self.cal_frame, text=d).grid(row=0, column=i, padx=3, pady=2)

        cal = calendar.Calendar(firstweekday=0)  # Monday
        month_days = cal.monthdayscalendar(year, month)

        for r, week in enumerate(month_days, start=1):
            for c, day in enumerate(week):
                if day == 0:
                    ttk.Label(self.cal_frame, text='').grid(row=r, column=c, padx=2, pady=2)
                else:
                    b = ttk.Button(self.cal_frame, text=str(day), width=3,
                                   command=lambda d=day: self._select_day(d))
                    b.grid(row=r, column=c, padx=2, pady=2)

    def _select_day(self, day):
        self.selected_date = datetime(self.current.year, self.current.month, day).strftime('%Y-%m-%d')
        self.destroy()

    def _prev_month(self):
        year = self.current.year
        month = self.current.month - 1
        if month < 1:
            month = 12
            year -= 1
        self.current = self.current.replace(year=year, month=month, day=1)
        self._draw_calendar()

    def _next_month(self):
        year = self.current.year
        month = self.current.month + 1
        if month > 12:
            month = 1
            year += 1
        self.current = self.current.replace(year=year, month=month, day=1)
        self._draw_calendar()


class AgendaApp(tk.Tk):
    """Aplicación principal: ventana con Treeview, formularios y botones.
    """

    def __init__(self):
        super().__init__()
        self.title("Agenda Personal de José Ramírez 2025")
        self.geometry("700x400")

        # Contenedores (Frames)
        self.frame_list = ttk.Frame(self, padding=8)
        self.frame_list.pack(fill='both', expand=True)

        self.frame_entry = ttk.Frame(self, padding=8)
        self.frame_entry.pack(fill='x')

        self.frame_actions = ttk.Frame(self, padding=8)
        self.frame_actions.pack(fill='x')

        # Treeview para mostrar eventos
        self._create_treeview()

        # Campos de entrada
        self._create_entry_fields()

        # Botones de acción
        self._create_action_buttons()

    def _create_treeview(self):
        columns = ("date", "time", "desc")
        self.tree = ttk.Treeview(self.frame_list, columns=columns, show='headings', selectmode='browse')
        self.tree.heading('date', text='Fecha')
        self.tree.heading('time', text='Hora')
        self.tree.heading('desc', text='Descripción')
        self.tree.column('date', width=120, anchor='center')
        self.tree.column('time', width=80, anchor='center')
        self.tree.column('desc', width=440, anchor='w')

        vsb = ttk.Scrollbar(self.frame_list, orient="vertical", command=self.tree.yview)
        self.tree.configure(yscrollcommand=vsb.set)
        self.tree.grid(row=0, column=0, sticky='nsew')
        vsb.grid(row=0, column=1, sticky='ns')
        # *** marcas de agua, José Ramírez U.E.A. 2025 en el Treeview b ***
        positions = [(0.3, 0.3), (0.7, 0.3), (0.5, 0.5), (0.3, 0.7), (0.7, 0.7)]
        for x, y in positions:
            lbl = tk.Label(
                self.frame_list,
                text="José Ramírez U.E.A. 2025",
                font=("Arial", 12, "bold"),
                fg="lightgray",
                bg="White"
            )
            lbl.place(relx=x, rely=y, anchor="center")

    def _create_entry_fields(self):
        # Labels y Entries organizados en el frame_entry
        lbl_date = ttk.Label(self.frame_entry, text="Fecha (DD-MM-YYYY):")
        lbl_date.grid(row=0, column=0, padx=4, pady=4, sticky='w')

        self.entry_date = ttk.Entry(self.frame_entry, width=15)
        self.entry_date.grid(row=0, column=1, padx=4, pady=4)

        btn_datepicker = ttk.Button(self.frame_entry, text="📅", width=3, command=self._open_datepicker)
        btn_datepicker.grid(row=0, column=2, padx=2)

        lbl_time = ttk.Label(self.frame_entry, text="Hora (HH:MM):")
        lbl_time.grid(row=0, column=3, padx=8, pady=4, sticky='w')

        self.entry_time = ttk.Entry(self.frame_entry, width=10)
        self.entry_time.grid(row=0, column=4, padx=4, pady=4)

        lbl_desc = ttk.Label(self.frame_entry, text="Descripción:")
        lbl_desc.grid(row=1, column=0, padx=4, pady=4, sticky='nw')

        self.entry_desc = ttk.Entry(self.frame_entry, width=60)
        self.entry_desc.grid(row=1, column=1, columnspan=4, padx=4, pady=4, sticky='w')

    def _create_action_buttons(self):
        btn_add = ttk.Button(self.frame_actions, text="Agregar Evento", command=self._add_event)
        btn_add.pack(side='left', padx=6, pady=6)

        btn_del = ttk.Button(self.frame_actions, text="Eliminar Evento Seleccionado", command=self._delete_selected)
        btn_del.pack(side='left', padx=6, pady=6)

        btn_exit = ttk.Button(self.frame_actions, text="Salir", command=self._on_exit)
        btn_exit.pack(side='right', padx=6, pady=6)

    def _open_datepicker(self):
        """Abre la ventana DatePicker y asigna la fecha seleccionada al Entry de fecha."""
        dp = DatePicker(self, initial_date=self.entry_date.get())
        self.wait_window(dp)
        if hasattr(dp, 'selected_date') and dp.selected_date:
            self.entry_date.delete(0, tk.END)
            self.entry_date.insert(0, dp.selected_date)

    def _add_event(self):
        """Valida los campos y agrega el evento al Treeview.
        Fecha: DD-MM-YYYY
        Hora: HH:MM (24h)
        Descripción: texto breve
        """
        date_text = self.entry_date.get().strip()
        time_text = self.entry_time.get().strip()
        desc_text = self.entry_desc.get().strip()

        # Validaciones básicas
        if not date_text or not time_text or not desc_text:
            messagebox.showwarning("Campos incompletos", "Por favor complete todos los campos antes de agregar")
            return

        # Validar formato de fecha
        try:
            datetime.strptime(date_text, "%Y-%m-%d")
        except ValueError:
            messagebox.showerror("Formato de fecha inválido", "Use el formato DD-MM-YYYY. Por ejemplo: 19-09-2025")
            return

        # Validar formato de hora
        try:
            datetime.strptime(time_text, "%H:%M")
        except ValueError:
            messagebox.showerror("Formato de hora inválido", "Use el formato HH:MM en 24 horas. Por ejemplo: 14:30")
            return

        # Agregar al Treeview
        # Generamos un id único automáticamente (puede ser usado luego para eliminar)
        iid = self.tree.insert('', 'end', values=(date_text, time_text, desc_text))

        # Limpiar campos después de agregar
        self.entry_date.delete(0, tk.END)
        self.entry_time.delete(0, tk.END)
        self.entry_desc.delete(0, tk.END)

        # Para mejor experiencia, seleccionamos el nuevo elemento
        self.tree.selection_set(iid)

    def _delete_selected(self):
        """Elimina el evento seleccionado después de una confirmación opcional."""
        sel = self.tree.selection()
        if not sel:
            messagebox.showinfo("Sin selección", "Seleccione un evento para eliminar")
            return

        iid = sel[0]
        values = self.tree.item(iid, 'values')
        date_text, time_text, desc_text = values

        # Diálogo de confirmación
        confirm = messagebox.askyesno("Confirmar eliminación",
                                      f"¿Eliminar el evento del {date_text} {time_text}?\n\n{desc_text}")
        if confirm:
            self.tree.delete(iid)

    def _on_exit(self):
        if messagebox.askokcancel("Salir", "¿Desea salir de la Agenda?"):
            self.destroy()


if __name__ == '__main__':
    app = AgendaApp()
    app.mainloop()

