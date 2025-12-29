using System;
using System.Collections.Generic;
using System.Linq;

namespace AgendaTelefonica
{
    // ============================================================================
    // CLASE CONTACTO
    // Representa un contacto individual en la agenda telefónica
    // Implementa encapsulación con propiedades validadas
    // ============================================================================
    
    public class Contacto
    {
        // Campos privados para encapsulación
        private string nombre;
        private string telefono;
        private string email;
        private string direccion;
        private string categoria;

        // Propiedad: Nombre del contacto con validación
        public string Nombre
        {
            get { return nombre; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre no puede estar vacío");
                nombre = value.Trim();
            }
        }

        // Propiedad: Teléfono único del contacto con validación de formato
        public string Telefono
        {
            get { return telefono; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El teléfono no puede estar vacío");
                
                string telefonoLimpio = value.Trim().Replace("-", "").Replace(" ", "");
                if (telefonoLimpio.Length < 7 || telefonoLimpio.Length > 15)
                    throw new ArgumentException("El teléfono debe tener entre 7 y 15 dígitos");
                
                telefono = value.Trim();
            }
        }

        // Propiedad: Email con validación básica
        public string Email
        {
            get { return email; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value) && !value.Contains("@"))
                    throw new ArgumentException("El email debe contener @");
                email = value?.Trim();
            }
        }

        // Propiedad: Dirección física del contacto
        public string Direccion
        {
            get { return direccion; }
            set { direccion = value?.Trim(); }
        }

        // Propiedad: Categoría para clasificación (Familia, Trabajo, Amigos, etc.)
        public string Categoria
        {
            get { return categoria; }
            set { categoria = value?.Trim() ?? "General"; }
        }

        // Propiedad: Fecha de creación del registro (solo lectura externa)
        public DateTime FechaCreacion { get; private set; }

        // Propiedad: Fecha de última modificación
        public DateTime FechaModificacion { get; set; }

        // Constructor completo con todos los parámetros
        public Contacto(string nombre, string telefono, string email = "", 
                       string direccion = "", string categoria = "General")
        {
            Nombre = nombre;
            Telefono = telefono;
            Email = email;
            Direccion = direccion;
            Categoria = categoria;
            FechaCreacion = DateTime.Now;
            FechaModificacion = DateTime.Now;
        }

        // Constructor simplificado con datos mínimos
        public Contacto(string nombre, string telefono)
        {
            Nombre = nombre;
            Telefono = telefono;
            Email = "";
            Direccion = "";
            Categoria = "General";
            FechaCreacion = DateTime.Now;
            FechaModificacion = DateTime.Now;
        }

        // Método para actualizar información del contacto
        public void ActualizarInformacion(string nombre = null, string telefono = null,
                                         string email = null, string direccion = null, 
                                         string categoria = null)
        {
            if (nombre != null) Nombre = nombre;
            if (telefono != null) Telefono = telefono;
            if (email != null) Email = email;
            if (direccion != null) Direccion = direccion;
            if (categoria != null) Categoria = categoria;
            FechaModificacion = DateTime.Now;
        }

        // Representación completa del contacto como texto
        public override string ToString()
        {
            return $"Nombre: {Nombre}\n" +
                   $"Teléfono: {Telefono}\n" +
                   $"Email: {(string.IsNullOrEmpty(Email) ? "No especificado" : Email)}\n" +
                   $"Dirección: {(string.IsNullOrEmpty(Direccion) ? "No especificada" : Direccion)}\n" +
                   $"Categoría: {Categoria}\n" +
                   $"Fecha de creación: {FechaCreacion:dd/MM/yyyy HH:mm}\n" +
                   $"Última modificación: {FechaModificacion:dd/MM/yyyy HH:mm}";
        }

        // Representación resumida en una línea
        public string ToStringResumido()
        {
            return $"{Nombre,-25} | {Telefono,-15} | {Categoria,-12}";
        }

        // Comparación de igualdad basada en el teléfono
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Contacto other = (Contacto)obj;
            return Telefono == other.Telefono;
        }

        // Código hash basado en el teléfono para uso en colecciones hash
        public override int GetHashCode()
        {
            return Telefono.GetHashCode();
        }
    }

    // ============================================================================
    // CLASE AGENDATELEFONICA
    // Gestiona la colección completa de contactos
    // Implementa operaciones CRUD y búsquedas con múltiples estructuras de datos
    // ============================================================================
    
    public class AgendaTelefonica
    {
        // ESTRUCTURAS DE DATOS PRINCIPALES:
        
        // Lista genérica: almacenamiento principal con orden de inserción
        // Ventajas: tamaño dinámico, fácil iteración, ordenamiento
        // Complejidad: O(1) inserción al final, O(n) búsqueda
        private List<Contacto> contactos;
        
        // Diccionario: índice para búsqueda rápida por teléfono
        // Ventajas: búsqueda O(1), previene duplicados
        // Complejidad: O(1) búsqueda, inserción, eliminación
        private Dictionary<string, Contacto> indiceTelefonico;
        
        // Diccionario de listas: índice para filtrado por categoría
        // Ventajas: agrupa contactos relacionados, consulta rápida
        // Complejidad: O(1) acceso a categoría, O(k) iteración donde k = contactos en categoría
        private Dictionary<string, List<Contacto>> indiceCategoria;

        // Propiedad de solo lectura: total de contactos registrados
        public int TotalContactos => contactos.Count;

        // Constructor: inicializa todas las estructuras de datos vacías
        public AgendaTelefonica()
        {
            contactos = new List<Contacto>();
            indiceTelefonico = new Dictionary<string, Contacto>();
            indiceCategoria = new Dictionary<string, List<Contacto>>();
        }

        // ========== OPERACIÓN CREATE (CRUD) ==========
        // Agrega un nuevo contacto validando que el teléfono sea único
        public bool AgregarContacto(Contacto nuevoContacto)
        {
            if (nuevoContacto == null)
                throw new ArgumentNullException(nameof(nuevoContacto));

            // Verificar unicidad del teléfono usando el índice
            if (indiceTelefonico.ContainsKey(nuevoContacto.Telefono))
            {
                Console.WriteLine($"Error: Ya existe un contacto con el teléfono {nuevoContacto.Telefono}");
                return false;
            }

            // Agregar a lista principal
            contactos.Add(nuevoContacto);

            // Agregar a índice telefónico para búsqueda O(1)
            indiceTelefonico[nuevoContacto.Telefono] = nuevoContacto;

            // Agregar a índice de categorías
            if (!indiceCategoria.ContainsKey(nuevoContacto.Categoria))
            {
                indiceCategoria[nuevoContacto.Categoria] = new List<Contacto>();
            }
            indiceCategoria[nuevoContacto.Categoria].Add(nuevoContacto);

            Console.WriteLine($"Contacto '{nuevoContacto.Nombre}' agregado exitosamente.");
            return true;
        }

        // ========== OPERACIÓN READ (CRUD) ==========
        // Búsqueda exacta por teléfono usando índice (Complejidad: O(1))
        public Contacto BuscarPorTelefono(string telefono)
        {
            if (string.IsNullOrWhiteSpace(telefono))
                return null;

            if (indiceTelefonico.TryGetValue(telefono.Trim(), out Contacto contacto))
            {
                return contacto;
            }

            return null;
        }

        // Búsqueda parcial por nombre usando LINQ (Complejidad: O(n))
        public List<Contacto> BuscarPorNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return new List<Contacto>();

            return contactos.Where(c => c.Nombre.ToLower().Contains(nombre.ToLower()))
                           .ToList();
        }

        // Obtener contactos de una categoría específica (Complejidad: O(1) + O(k))
        public List<Contacto> ObtenerPorCategoria(string categoria)
        {
            if (string.IsNullOrWhiteSpace(categoria))
                return new List<Contacto>();

            if (indiceCategoria.TryGetValue(categoria.Trim(), out List<Contacto> contactosCategoria))
            {
                return new List<Contacto>(contactosCategoria);
            }

            return new List<Contacto>();
        }

        // ========== OPERACIÓN DELETE (CRUD) ==========
        // Elimina contacto manteniendo sincronización entre estructuras
        public bool EliminarContacto(string telefono)
        {
            Contacto contacto = BuscarPorTelefono(telefono);
            if (contacto == null)
            {
                Console.WriteLine("Contacto no encontrado.");
                return false;
            }

            // Eliminar de lista principal
            contactos.Remove(contacto);

            // Eliminar de índice telefónico
            indiceTelefonico.Remove(telefono);

            // Eliminar de índice de categorías
            if (indiceCategoria.ContainsKey(contacto.Categoria))
            {
                indiceCategoria[contacto.Categoria].Remove(contacto);
                
                // Si la categoría queda vacía, eliminarla del índice
                if (indiceCategoria[contacto.Categoria].Count == 0)
                {
                    indiceCategoria.Remove(contacto.Categoria);
                }
            }

            Console.WriteLine($"Contacto '{contacto.Nombre}' eliminado exitosamente.");
            return true;
        }

        // ========== OPERACIÓN UPDATE (CRUD) ==========
        // Modifica contacto existente actualizando todos los índices
        public bool ModificarContacto(string telefonoActual, string nuevoNombre = null,
                                     string nuevoTelefono = null, string nuevoEmail = null,
                                     string nuevaDireccion = null, string nuevaCategoria = null)
        {
            Contacto contacto = BuscarPorTelefono(telefonoActual);
            if (contacto == null)
            {
                Console.WriteLine("Contacto no encontrado.");
                return false;
            }

            string categoriaAnterior = contacto.Categoria;

            // Si cambia el teléfono, actualizar índice telefónico
            if (nuevoTelefono != null && nuevoTelefono != telefonoActual)
            {
                if (indiceTelefonico.ContainsKey(nuevoTelefono))
                {
                    Console.WriteLine("El nuevo teléfono ya existe en la agenda.");
                    return false;
                }

                indiceTelefonico.Remove(telefonoActual);
                indiceTelefonico[nuevoTelefono] = contacto;
            }

            // Actualizar información del contacto
            contacto.ActualizarInformacion(nuevoNombre, nuevoTelefono, nuevoEmail, 
                                          nuevaDireccion, nuevaCategoria);

            // Si cambió categoría, actualizar índice de categorías
            if (nuevaCategoria != null && nuevaCategoria != categoriaAnterior)
            {
                // Remover de categoría anterior
                if (indiceCategoria.ContainsKey(categoriaAnterior))
                {
                    indiceCategoria[categoriaAnterior].Remove(contacto);
                    if (indiceCategoria[categoriaAnterior].Count == 0)
                    {
                        indiceCategoria.Remove(categoriaAnterior);
                    }
                }

                // Agregar a nueva categoría
                if (!indiceCategoria.ContainsKey(nuevaCategoria))
                {
                    indiceCategoria[nuevaCategoria] = new List<Contacto>();
                }
                indiceCategoria[nuevaCategoria].Add(contacto);
            }

            Console.WriteLine($"Contacto modificado exitosamente.");
            return true;
        }

        // ========== OPERACIONES DE REPORTERÍA ==========
        
        // Lista todos los contactos ordenados alfabéticamente
        public void ListarContactos()
        {
            if (contactos.Count == 0)
            {
                Console.WriteLine("\nNo hay contactos en la agenda.");
                return;
            }

            Console.WriteLine($"\n{'='} LISTA DE CONTACTOS {'='}");
            Console.WriteLine($"Total de contactos: {contactos.Count}\n");

            // Ordenar usando LINQ (Complejidad: O(n log n))
            var contactosOrdenados = contactos.OrderBy(c => c.Nombre).ToList();

            Console.WriteLine($"{"Nombre",-25} | {"Teléfono",-15} | {"Categoría",-12}");
            Console.WriteLine(new string('-', 60));

            foreach (var contacto in contactosOrdenados)
            {
                Console.WriteLine(contacto.ToStringResumido());
            }
            Console.WriteLine();
        }

        // Muestra información detallada de un contacto específico
        public void MostrarDetalleContacto(string telefono)
        {
            Contacto contacto = BuscarPorTelefono(telefono);
            if (contacto == null)
            {
                Console.WriteLine("Contacto no encontrado.");
                return;
            }

            Console.WriteLine("\n" + new string('=', 50));
            Console.WriteLine("INFORMACIÓN DETALLADA DEL CONTACTO");
            Console.WriteLine(new string('=', 50));
            Console.WriteLine(contacto.ToString());
            Console.WriteLine(new string('=', 50) + "\n");
        }

        // Genera estadísticas completas de la agenda
        public void MostrarEstadisticas()
        {
            Console.WriteLine("\n" + new string('=', 50));
            Console.WriteLine("ESTADÍSTICAS DE LA AGENDA");
            Console.WriteLine(new string('=', 50));
            Console.WriteLine($"Total de contactos: {contactos.Count}");
            Console.WriteLine($"Número de categorías: {indiceCategoria.Count}");
            Console.WriteLine("\nContactos por categoría:");

            foreach (var categoria in indiceCategoria.OrderByDescending(kvp => kvp.Value.Count))
            {
                Console.WriteLine($"  - {categoria.Key}: {categoria.Value.Count} contacto(s)");
            }

            if (contactos.Count > 0)
            {
                var contactoMasReciente = contactos.OrderByDescending(c => c.FechaCreacion).First();
                Console.WriteLine($"\nÚltimo contacto agregado: {contactoMasReciente.Nombre}");
                Console.WriteLine($"Fecha: {contactoMasReciente.FechaCreacion:dd/MM/yyyy HH:mm}");
            }

            Console.WriteLine(new string('=', 50) + "\n");
        }

        // Obtiene lista de todas las categorías existentes
        public List<string> ObtenerCategorias()
        {
            return indiceCategoria.Keys.OrderBy(k => k).ToList();
        }

        // ========== OPERACIONES CON MATRICES Y ARRAYS ==========
        
        // Exporta contactos a matriz bidimensional (Array 2D)
        // Útil para exportación a CSV o integración con otros sistemas
        public string[,] ExportarAMatriz()
        {
            if (contactos.Count == 0)
                return new string[0, 0];

            // Crear matriz: filas = contactos, columnas = campos
            string[,] matriz = new string[contactos.Count, 5];

            for (int i = 0; i < contactos.Count; i++)
            {
                matriz[i, 0] = contactos[i].Nombre;
                matriz[i, 1] = contactos[i].Telefono;
                matriz[i, 2] = contactos[i].Email;
                matriz[i, 3] = contactos[i].Direccion;
                matriz[i, 4] = contactos[i].Categoria;
            }

            return matriz;
        }

        // Búsqueda global en todos los campos del contacto
        public List<Contacto> BusquedaGlobal(string termino)
        {
            if (string.IsNullOrWhiteSpace(termino))
                return new List<Contacto>();

            string terminoLower = termino.ToLower();

            return contactos.Where(c =>
                c.Nombre.ToLower().Contains(terminoLower) ||
                c.Telefono.Contains(termino) ||
                (c.Email != null && c.Email.ToLower().Contains(terminoLower)) ||
                (c.Direccion != null && c.Direccion.ToLower().Contains(terminoLower)) ||
                c.Categoria.ToLower().Contains(terminoLower)
            ).ToList();
        }

        // Limpia toda la agenda (usar con precaución)
        public void LimpiarAgenda()
        {
            contactos.Clear();
            indiceTelefonico.Clear();
            indiceCategoria.Clear();
            Console.WriteLine("Agenda limpiada exitosamente.");
        }
    }

    // ============================================================================
    // CLASE PROGRAM
    // Programa principal con interfaz de usuario mediante consola
    // Implementa menú interactivo y gestión de operaciones
    // ============================================================================
    
    class Program
    {
        private static AgendaTelefonica agenda = new AgendaTelefonica();

        static void Main(string[] args)
        {
            // Cargar datos de ejemplo para demostración
            CargarDatosEjemplo();

            bool salir = false;

            while (!salir)
            {
                try
                {
                    MostrarMenu();
                    int opcion = LeerOpcion();

                    switch (opcion)
                    {
                        case 1:
                            AgregarNuevoContacto();
                            break;
                        case 2:
                            BuscarContacto();
                            break;
                        case 3:
                            ModificarContactoExistente();
                            break;
                        case 4:
                            EliminarContactoExistente();
                            break;
                        case 5:
                            agenda.ListarContactos();
                            break;
                        case 6:
                            ListarPorCategoria();
                            break;
                        case 7:
                            BusquedaAvanzada();
                            break;
                        case 8:
                            agenda.MostrarEstadisticas();
                            break;
                        case 9:
                            MostrarReporteCompleto();
                            break;
                        case 0:
                            salir = true;
                            Console.WriteLine("\n¡Gracias por usar la Agenda Telefónica!");
                            break;
                        default:
                            Console.WriteLine("\nOpción no válida. Intente nuevamente.");
                            break;
                    }

                    if (!salir)
                    {
                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nError: {ex.Message}");
                    Console.WriteLine("Presione cualquier tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }

        // Muestra el menú principal con todas las opciones disponibles
        static void MostrarMenu()
        {
            Console.Clear();
            Console.WriteLine("╔═══════════════════════════════════════════════════╗");
            Console.WriteLine("║       SISTEMA DE AGENDA TELEFÓNICA - C#           ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════╝");
            Console.WriteLine($"\nContactos registrados: {agenda.TotalContactos}");
            Console.WriteLine("\n┌─── MENÚ PRINCIPAL ────────────────────────────────┐");
            Console.WriteLine("│                                                   │");
            Console.WriteLine("│  1. Agregar nuevo contacto                        │");
            Console.WriteLine("│  2. Buscar contacto                               │");
            Console.WriteLine("│  3. Modificar contacto                            │");
            Console.WriteLine("│  4. Eliminar contacto                             │");
            Console.WriteLine("│  5. Listar todos los contactos                    │");
            Console.WriteLine("│  6. Listar por categoría                          │");
            Console.WriteLine("│  7. Búsqueda avanzada                             │");
            Console.WriteLine("│  8. Ver estadísticas                              │");
            Console.WriteLine("│  9. Reporte completo                              │");
            Console.WriteLine("│  0. Salir                                         │");
            Console.WriteLine("│                                                   │");
            Console.WriteLine("└───────────────────────────────────────────────────┘");
            Console.Write("\nSeleccione una opción: ");
        }

        // Lee y valida la opción ingresada por el usuario
        static int LeerOpcion()
        {
            if (int.TryParse(Console.ReadLine(), out int opcion))
            {
                return opcion;
            }
            return -1;
        }

        // OPCIÓN 1: Agregar nuevo contacto con validación completa
        static void AgregarNuevoContacto()
        {
            Console.Clear();
            Console.WriteLine("═══ AGREGAR NUEVO CONTACTO ═══\n");

            try
            {
                Console.Write("Nombre completo: ");
                string nombre = Console.ReadLine();

                Console.Write("Teléfono: ");
                string telefono = Console.ReadLine();

                Console.Write("Email (opcional): ");
                string email = Console.ReadLine();

                Console.Write("Dirección (opcional): ");
                string direccion = Console.ReadLine();

                Console.Write("Categoría (Familia/Trabajo/Amigos/General): ");
                string categoria = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(categoria))
                    categoria = "General";

                Contacto nuevoContacto = new Contacto(nombre, telefono, email, 
                                                     direccion, categoria);
                agenda.AgregarContacto(nuevoContacto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError al agregar contacto: {ex.Message}");
            }
        }

        // OPCIÓN 2: Buscar contacto por teléfono o nombre
        static void BuscarContacto()
        {
            Console.Clear();
            Console.WriteLine("═══ BUSCAR CONTACTO ═══\n");

            Console.WriteLine("Buscar por:");
            Console.WriteLine("1. Número de teléfono");
            Console.WriteLine("2. Nombre");
            Console.Write("\nOpción: ");

            int opcion = LeerOpcion();

            if (opcion == 1)
            {
                Console.Write("\nIngrese el número de teléfono: ");
                string telefono = Console.ReadLine();

                Contacto contacto = agenda.BuscarPorTelefono(telefono);
                if (contacto != null)
                {
                    agenda.MostrarDetalleContacto(telefono);
                }
                else
                {
                    Console.WriteLine("\nContacto no encontrado.");
                }
            }
            else if (opcion == 2)
            {
                Console.Write("\nIngrese el nombre a buscar: ");
                string nombre = Console.ReadLine();

                List<Contacto> resultados = agenda.BuscarPorNombre(nombre);
                if (resultados.Count > 0)
                {
                    Console.WriteLine($"\nSe encontraron {resultados.Count} contacto(s):\n");
                    Console.WriteLine($"{"Nombre",-25} | {"Teléfono",-15} | {"Categoría",-12}");
                    Console.WriteLine(new string('-', 60));

                    foreach (var contacto in resultados)
                    {
                        Console.WriteLine(contacto.ToStringResumido());
                    }
                }
                else
                {
                    Console.WriteLine("\nNo se encontraron contactos con ese nombre.");
                }
            }
        }

        // OPCIÓN 3: Modificar contacto existente
        static void ModificarContactoExistente()
        {
            Console.Clear();
            Console.WriteLine("═══ MODIFICAR CONTACTO ═══\n");

            Console.Write("Ingrese el teléfono del contacto a modificar: ");
            string telefono = Console.ReadLine();

            Contacto contacto = agenda.BuscarPorTelefono(telefono);
            if (contacto == null)
            {
                Console.WriteLine("\nContacto no encontrado.");
                return;
            }

            Console.WriteLine("\nContacto actual:");
            Console.WriteLine(contacto.ToString());

            Console.WriteLine("\n\nIngrese los nuevos datos (Enter para mantener actual):");

            Console.Write($"Nombre [{contacto.Nombre}]: ");
            string nuevoNombre = Console.ReadLine();

            Console.Write($"Teléfono [{contacto.Telefono}]: ");
            string nuevoTelefono = Console.ReadLine();

            Console.Write($"Email [{contacto.Email}]: ");
            string nuevoEmail = Console.ReadLine();

            Console.Write($"Dirección [{contacto.Direccion}]: ");
            string nuevaDireccion = Console.ReadLine();

            Console.Write($"Categoría [{contacto.Categoria}]: ");
            string nuevaCategoria = Console.ReadLine();

            agenda.ModificarContacto(telefono,
                string.IsNullOrWhiteSpace(nuevoNombre) ? null : nuevoNombre,
                string.IsNullOrWhiteSpace(nuevoTelefono) ? null : nuevoTelefono,
                string.IsNullOrWhiteSpace(nuevoEmail) ? null : nuevoEmail,
                string.IsNullOrWhiteSpace(nuevaDireccion) ? null : nuevaDireccion,
                string.IsNullOrWhiteSpace(nuevaCategoria) ? null : nuevaCategoria);
        }

        // OPCIÓN 4: Eliminar contacto con confirmación
        static void EliminarContactoExistente()
        {
            Console.Clear();
            Console.WriteLine("═══ ELIMINAR CONTACTO ═══\n");

            Console.Write("Ingrese el teléfono del contacto a eliminar: ");
            string telefono = Console.ReadLine();

            Contacto contacto = agenda.BuscarPorTelefono(telefono);
            if (contacto == null)
            {
                Console.WriteLine("\nContacto no encontrado.");
                return;
            }

            Console.WriteLine("\nContacto a eliminar:");
            Console.WriteLine(contacto.ToStringResumido());

            Console.Write("\n¿Está seguro de eliminar este contacto? (S/N): ");
            string confirmacion = Console.ReadLine();

            if (confirmacion.ToUpper() == "S")
            {
                agenda.EliminarContacto(telefono);
            }
            else
            {
                Console.WriteLine("\nOperación cancelada.");
            }
        }

        // OPCIÓN 6: Listar contactos por categoría
        static void ListarPorCategoria()
        {
            Console.Clear();
            Console.WriteLine("═══ LISTAR POR CATEGORÍA ═══\n");

            List<string> categorias = agenda.ObtenerCategorias();

            if (categorias.Count == 0)
            {
                Console.WriteLine("No hay categorías disponibles.");
                return;
            }

            Console.WriteLine("Categorías disponibles:");
            for (int i = 0; i < categorias.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {categorias[i]}");
            }

            Console.Write("\nSeleccione una categoría: ");
            int opcion = LeerOpcion();

            if (opcion > 0 && opcion <= categorias.Count)
            {
                string categoriaSeleccionada = categorias[opcion - 1];
                List<Contacto> contactosCategoria = agenda.ObtenerPorCategoria(categoriaSeleccionada);

                Console.WriteLine($"\n\nContactos en la categoría '{categoriaSeleccionada}':");
                Console.WriteLine($"{"Nombre",-25} | {"Teléfono",-15}");
                Console.WriteLine(new string('-', 45));

                foreach (var contacto in contactosCategoria)
                {
                    Console.WriteLine($"{contacto.Nombre,-25} | {contacto.Telefono,-15}");
                }
            }
        }

        // OPCIÓN 7: Búsqueda avanzada en todos los campos
        static void BusquedaAvanzada()
        {
            Console.Clear();
            Console.WriteLine("═══ BÚSQUEDA AVANZADA ═══\n");

            Console.Write("Ingrese término de búsqueda: ");
            string termino = Console.ReadLine();

            List<Contacto> resultados = agenda.BusquedaGlobal(termino);

            if (resultados.Count > 0)
            {
                Console.WriteLine($"\nSe encontraron {resultados.Count} resultado(s):\n");
                Console.WriteLine($"{"Nombre",-25} | {"Teléfono",-15} | {"Categoría",-12}");
                Console.WriteLine(new string('-', 60));

                foreach (var contacto in resultados)
                {
                    Console.WriteLine(contacto.ToStringResumido());
                }
            }
            else
            {
                Console.WriteLine("\nNo se encontraron resultados.");
            }
        }

        // OPCIÓN 9: Reporte completo con estadísticas y exportación matricial
        static void MostrarReporteCompleto()
        {
            Console.Clear();
            Console.WriteLine("═══════════════════════════════════════════════════");
            Console.WriteLine("           REPORTE COMPLETO DE AGENDA              ");
            Console.WriteLine("═══════════════════════════════════════════════════\n");

            // Mostrar estadísticas generales
            agenda.MostrarEstadisticas();

            // Exportar a matriz y mostrar en formato tabular
            Console.WriteLine("\n" + new string('=', 50));
            Console.WriteLine("CONTACTOS EN FORMATO MATRICIAL");
            Console.WriteLine(new string('=', 50));

            string[,] matriz = agenda.ExportarAMatriz();

            if (matriz.GetLength(0) > 0)
            {
                Console.WriteLine($"\n{"Nombre",-20} | {"Teléfono",-12} | {"Email",-25} | {"Categoría",-12}");
                Console.WriteLine(new string('-', 80));

                for (int i = 0; i < matriz.GetLength(0); i++)
                {
                    Console.WriteLine($"{matriz[i, 0],-20} | {matriz[i, 1],-12} | {matriz[i, 2],-25} | {matriz[i, 4],-12}");
                }
            }

            Console.WriteLine("\n" + new string('=', 50));
        }

        // Carga datos de ejemplo para facilitar pruebas del sistema
        static void CargarDatosEjemplo()
        {
            agenda.AgregarContacto(new Contacto("María González", "0987654321", 
                "maria.gonzalez@email.com", "Av. Principal 123", "Familia"));
            
            agenda.AgregarContacto(new Contacto("Juan Pérez", "0991234567", 
                "juan.perez@empresa.com", "Calle Secundaria 456", "Trabajo"));
            
            agenda.AgregarContacto(new Contacto("Ana Rodríguez", "0998765432", 
                "ana.rodriguez@email.com", "Pasaje Los Rosales 789", "Amigos"));
            
            agenda.AgregarContacto(new Contacto("Carlos Martínez", "0992345678", 
                "carlos.martinez@email.com", "Urbanización El Sol 321", "Trabajo"));
            
            agenda.AgregarContacto(new Contacto("Laura Sánchez", "0993456789", 
                "laura.sanchez@email.com", "Conjunto Residencial 654", "Familia"));
        }
    }
}

// ============================================================================
// ANÁLISIS DE ESTRUCTURAS DE DATOS UTILIZADAS
// ============================================================================

/*
 * 1. LIST<CONTACTO> - Lista Genérica
 * ----------------------------------
 * Uso: Almacenamiento principal de contactos
 * 
 * Ventajas:
 * - Tamaño dinámico que crece automáticamente según necesidad
 * - Acceso indexado en O(1) para lectura por posición
 * - Inserción al final en O(1) amortizado
 * - Métodos integrados de ordenamiento y búsqueda
 * - Compatible con LINQ para consultas expresivas
 * 
 * Desventajas:
 * - Búsqueda lineal O(n) sin índices
 * - Eliminación en medio requiere desplazamiento O(n)
 * - Mayor consumo de memoria que arrays estáticos
 * 
 * Complejidad temporal:
 * - Agregar al final: O(1) amortizado
 * - Buscar elemento: O(n)
 * - Acceso por índice: O(1)
 * - Eliminar elemento: O(n)
 * 
 * ============================================================================
 * 
 * 2. DICTIONARY<STRING, CONTACTO> - Diccionario (Tabla Hash)
 * -----------------------------------------------------------
 * Uso: Índice para búsqueda rápida por número telefónico
 * 
 * Ventajas:
 * - Búsqueda en tiempo constante O(1) promedio mediante hash
 * - Inserción y eliminación en O(1) promedio
 * - Previene duplicados automáticamente (clave única)
 * - Uso eficiente de memoria con función hash
 * 
 * Desventajas:
 * - No mantiene orden de inserción de elementos
 * - Requiere claves únicas, no permite duplicados
 * - Overhead computacional de función hash
 * - Peor caso O(n) en colisiones extremas (muy raro)
 * 
 * Complejidad temporal:
 * - Búsqueda: O(1) promedio
 * - Inserción: O(1) promedio
 * - Eliminación: O(1) promedio
 * 
 * ============================================================================
 * 
 * 3. DICTIONARY<STRING, LIST<CONTACTO>> - Índice Agrupado
 * --------------------------------------------------------
 * Uso: Agrupar contactos por categoría para filtrado rápido
 * 
 * Ventajas:
 * - Acceso directo a grupo de categoría en O(1)
 * - Agrupa elementos relacionados eficientemente
 * - Facilita reportes y estadísticas por categoría
 * - Iteración optimizada solo sobre elementos relevantes
 * 
 * Desventajas:
 * - Duplicación de referencias (mayor uso de memoria)
 * - Requiere sincronización al modificar contactos
 * - Complejidad adicional en mantenimiento
 * 
 * Complejidad temporal:
 * - Acceso a categoría: O(1)
 * - Iterar contactos de categoría: O(k) donde k = contactos en categoría
 * 
 * ============================================================================
 * 
 * 4. ARRAYS BIDIMENSIONALES (MATRICES)
 * -------------------------------------
 * Uso: Exportación de datos en formato tabular
 * 
 * Ventajas:
 * - Acceso directo a cualquier elemento en O(1)
 * - Representación natural de datos tabulares
 * - Bajo overhead de memoria
 * - Eficiencia de caché por localidad espacial
 * 
 * Desventajas:
 * - Tamaño fijo definido en creación
 * - No puede crecer dinámicamente
 * - Requiere conocer dimensiones de antemano
 * 
 * Complejidad temporal:
 * - Acceso a elemento: O(1)
 * - Crear matriz: O(n*m) donde n=filas, m=columnas
 * 
 * ============================================================================
 * 
 * CONCLUSIÓN DEL ANÁLISIS:
 * 
 * La combinación de estas estructuras proporciona un sistema balanceado donde:
 * 
 * - List mantiene todos los contactos con orden y flexibilidad
 * - Dictionary optimiza búsquedas frecuentes por teléfono
 * - Dictionary de Lists facilita agrupación por categoría
 * - Arrays bidimensionales permiten exportación estructurada
 * 
 * Este diseño híbrido sacrifica algo de memoria (redundancia controlada)
 * para obtener rendimiento óptimo en las operaciones más frecuentes,
 * siguiendo el principio de optimización basada en uso real del sistema.
 */
