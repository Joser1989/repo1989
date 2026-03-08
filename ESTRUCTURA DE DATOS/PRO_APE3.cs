using System;
using System.Collections.Generic;
using System.Linq;

namespace BibliotecaApp
{
    // ============================================================
    //  MODELO: Libro
    // ============================================================
    class Libro
    {
        public int    Id          { get; set; }
        public string Titulo      { get; set; }
        public string Autor       { get; set; }
        public string Genero      { get; set; }
        public int    Anio        { get; set; }
        public string ISBN        { get; set; }
        public bool   Disponible  { get; set; }

        public Libro(int id, string titulo, string autor,
                     string genero, int anio, string isbn)
        {
            Id         = id;
            Titulo     = titulo;
            Autor      = autor;
            Genero     = genero;
            Anio       = anio;
            ISBN       = isbn;
            Disponible = true;
        }

        public override string ToString() =>
            $"[{Id:D3}] \"{Titulo}\" — {Autor} | Género: {Genero} | Año: {Anio} | ISBN: {ISBN} | {(Disponible ? "✔ Disponible" : "✘ No disponible")}";
    }

    // ============================================================
    //  REPOSITORIO: maneja el conjunto (HashSet) y el mapa
    //  (Dictionary) principal
    // ============================================================
    class Biblioteca
    {
        // Conjunto de IDs registrados (evita duplicados)
        private readonly HashSet<int> _idsRegistrados = new();

        // Mapa principal: Id → Libro
        private readonly Dictionary<int, Libro> _catalogo = new();

        // Índice secundario: Autor → conjunto de Ids
        private readonly Dictionary<string, HashSet<int>> _porAutor = new();

        // Índice secundario: Género → conjunto de Ids
        private readonly Dictionary<string, HashSet<int>> _porGenero = new();

        // --------------------------------------------------------
        //  AGREGAR
        // --------------------------------------------------------
        public bool AgregarLibro(Libro libro)
        {
            if (!_idsRegistrados.Add(libro.Id))   // Add devuelve false si ya existe
            {
                Console.WriteLine($"  ⚠ El ID {libro.Id} ya está registrado.");
                return false;
            }

            _catalogo[libro.Id] = libro;

            // Índice por autor
            if (!_porAutor.ContainsKey(libro.Autor))
                _porAutor[libro.Autor] = new HashSet<int>();
            _porAutor[libro.Autor].Add(libro.Id);

            // Índice por género
            if (!_porGenero.ContainsKey(libro.Genero))
                _porGenero[libro.Genero] = new HashSet<int>();
            _porGenero[libro.Genero].Add(libro.Id);

            return true;
        }

        // --------------------------------------------------------
        //  ELIMINAR
        // --------------------------------------------------------
        public bool EliminarLibro(int id)
        {
            if (!_catalogo.TryGetValue(id, out Libro libro))
            {
                Console.WriteLine($"  ⚠ ID {id} no encontrado.");
                return false;
            }

            _catalogo.Remove(id);
            _idsRegistrados.Remove(id);
            _porAutor[libro.Autor].Remove(id);
            _porGenero[libro.Genero].Remove(id);
            Console.WriteLine($"  ✔ Libro ID {id} eliminado correctamente.");
            return true;
        }

        // --------------------------------------------------------
        //  BUSCAR POR ID
        // --------------------------------------------------------
        public Libro BuscarPorId(int id)
        {
            _catalogo.TryGetValue(id, out Libro libro);
            return libro;
        }

        // --------------------------------------------------------
        //  BUSCAR POR TÍTULO (contiene, sin distinción may/min)
        // --------------------------------------------------------
        public List<Libro> BuscarPorTitulo(string fragmento)
        {
            return _catalogo.Values
                .Where(l => l.Titulo.Contains(fragmento, StringComparison.OrdinalIgnoreCase))
                .OrderBy(l => l.Titulo)
                .ToList();
        }

        // --------------------------------------------------------
        //  BUSCAR POR AUTOR
        // --------------------------------------------------------
        public List<Libro> BuscarPorAutor(string autor)
        {
            if (_porAutor.TryGetValue(autor, out HashSet<int> ids))
                return ids.Select(id => _catalogo[id]).OrderBy(l => l.Titulo).ToList();
            return new List<Libro>();
        }

        // --------------------------------------------------------
        //  BUSCAR POR GÉNERO
        // --------------------------------------------------------
        public List<Libro> BuscarPorGenero(string genero)
        {
            if (_porGenero.TryGetValue(genero, out HashSet<int> ids))
                return ids.Select(id => _catalogo[id]).OrderBy(l => l.Titulo).ToList();
            return new List<Libro>();
        }

        // --------------------------------------------------------
        //  LISTAR TODO EL CATÁLOGO
        // --------------------------------------------------------
        public List<Libro> ObtenerTodos() =>
            _catalogo.Values.OrderBy(l => l.Id).ToList();

        // --------------------------------------------------------
        //  ESTADÍSTICAS
        // --------------------------------------------------------
        public void MostrarEstadisticas()
        {
            int total       = _catalogo.Count;
            int disponibles = _catalogo.Values.Count(l => l.Disponible);

            Console.WriteLine($"\n  📊 ESTADÍSTICAS DE LA BIBLIOTECA");
            Console.WriteLine($"  {new string('─', 38)}");
            Console.WriteLine($"  Total de libros registrados : {total}");
            Console.WriteLine($"  Libros disponibles          : {disponibles}");
            Console.WriteLine($"  Libros no disponibles       : {total - disponibles}");
            Console.WriteLine($"\n  Libros por autor:");
            foreach (var kv in _porAutor.OrderBy(k => k.Key))
                Console.WriteLine($"    • {kv.Key}: {kv.Value.Count} libro(s)");
            Console.WriteLine($"\n  Libros por género:");
            foreach (var kv in _porGenero.OrderBy(k => k.Key))
                Console.WriteLine($"    • {kv.Key}: {kv.Value.Count} libro(s)");
        }

        // --------------------------------------------------------
        //  MARCAR DISPONIBILIDAD
        // --------------------------------------------------------
        public bool CambiarDisponibilidad(int id, bool disponible)
        {
            if (!_catalogo.TryGetValue(id, out Libro libro))
                return false;
            libro.Disponible = disponible;
            return true;
        }

        // --------------------------------------------------------
        //  OPERACIONES DE CONJUNTOS entre autores
        // --------------------------------------------------------
        public void OperacionesConjunto(string autorA, string autorB)
        {
            HashSet<int> setA = _porAutor.ContainsKey(autorA)
                ? new HashSet<int>(_porAutor[autorA]) : new HashSet<int>();
            HashSet<int> setB = _porAutor.ContainsKey(autorB)
                ? new HashSet<int>(_porAutor[autorB]) : new HashSet<int>();

            // Unión
            var union = new HashSet<int>(setA); union.UnionWith(setB);
            // Intersección
            var inter = new HashSet<int>(setA); inter.IntersectWith(setB);
            // Diferencia A - B
            var diff  = new HashSet<int>(setA); diff.ExceptWith(setB);

            Console.WriteLine($"\n  🔵 Conjuntos — Autor A: \"{autorA}\"  |  Autor B: \"{autorB}\"");
            Console.WriteLine($"  Unión        ({union.Count} elem.): {FormatSet(union)}");
            Console.WriteLine($"  Intersección ({inter.Count} elem.): {FormatSet(inter)}");
            Console.WriteLine($"  Diferencia   ({diff.Count}  elem.): {FormatSet(diff)}");
        }

        private string FormatSet(HashSet<int> set) =>
            set.Count == 0 ? "∅" : "{ " + string.Join(", ", set.OrderBy(x => x)) + " }";
    }

    // ============================================================
    //  CLASE PRINCIPAL — Menú de consola
    // ============================================================
    class Program
    {
        static Biblioteca biblioteca = new();

        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            CargarDatosIniciales();

            bool salir = false;
            while (!salir)
            {
                MostrarMenu();
                string opcion = Console.ReadLine()?.Trim() ?? "";

                switch (opcion)
                {
                    case "1":  ListarTodos();           break;
                    case "2":  BuscarPorId();           break;
                    case "3":  BuscarPorTitulo();       break;
                    case "4":  BuscarPorAutor();        break;
                    case "5":  BuscarPorGenero();       break;
                    case "6":  AgregarLibro();          break;
                    case "7":  EliminarLibro();         break;
                    case "8":  CambiarDisponibilidad(); break;
                    case "9":  biblioteca.MostrarEstadisticas(); Pausa(); break;
                    case "10": DemoOperacionesConjunto();         break;
                    case "0":  salir = true;            break;
                    default:   Console.WriteLine("  Opción inválida."); break;
                }
            }

            Console.WriteLine("\n  Gracias por usar el Sistema de Biblioteca. ¡Hasta pronto!\n");
        }

        // --------------------------------------------------------
        static void MostrarMenu()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════════╗");
            Console.WriteLine("║    📚  SISTEMA DE GESTIÓN DE BIBLIOTECA          ║");
            Console.WriteLine("║        Teoría de Conjuntos y Mapas (C#)          ║");
            Console.WriteLine("╠══════════════════════════════════════════════════╣");
            Console.WriteLine("║  REPORTERÍA                                      ║");
            Console.WriteLine("║  [1] Listar todos los libros                     ║");
            Console.WriteLine("║  [2] Buscar por ID                               ║");
            Console.WriteLine("║  [3] Buscar por título                           ║");
            Console.WriteLine("║  [4] Buscar por autor                            ║");
            Console.WriteLine("║  [5] Buscar por género                           ║");
            Console.WriteLine("║                                                  ║");
            Console.WriteLine("║  GESTIÓN                                         ║");
            Console.WriteLine("║  [6] Agregar libro                               ║");
            Console.WriteLine("║  [7] Eliminar libro                              ║");
            Console.WriteLine("║  [8] Cambiar disponibilidad                      ║");
            Console.WriteLine("║                                                  ║");
            Console.WriteLine("║  ANÁLISIS                                        ║");
            Console.WriteLine("║  [9] Estadísticas generales                      ║");
            Console.WriteLine("║  [10] Demo operaciones de conjuntos              ║");
            Console.WriteLine("║                                                  ║");
            Console.WriteLine("║  [0] Salir                                       ║");
            Console.WriteLine("╚══════════════════════════════════════════════════╝");
            Console.Write("  Selecciona una opción: ");
        }

        // --------------------------------------------------------
        static void ListarTodos()
        {
            Console.Clear();
            Console.WriteLine("  📋 CATÁLOGO COMPLETO\n");
            var libros = biblioteca.ObtenerTodos();
            if (libros.Count == 0) { Console.WriteLine("  Sin registros."); }
            else foreach (var l in libros) Console.WriteLine("  " + l);
            Pausa();
        }

        static void BuscarPorId()
        {
            Console.Write("\n  Ingresa el ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var l = biblioteca.BuscarPorId(id);
                Console.WriteLine(l != null ? "  " + l : "  ⚠ No encontrado.");
            }
            Pausa();
        }

        static void BuscarPorTitulo()
        {
            Console.Write("\n  Fragmento del título: ");
            string txt = Console.ReadLine() ?? "";
            var resultados = biblioteca.BuscarPorTitulo(txt);
            ImprimirResultados(resultados);
            Pausa();
        }

        static void BuscarPorAutor()
        {
            Console.Write("\n  Nombre del autor: ");
            string autor = Console.ReadLine() ?? "";
            var resultados = biblioteca.BuscarPorAutor(autor);
            ImprimirResultados(resultados);
            Pausa();
        }

        static void BuscarPorGenero()
        {
            Console.Write("\n  Género (ej. Autoayuda, Novela): ");
            string genero = Console.ReadLine() ?? "";
            var resultados = biblioteca.BuscarPorGenero(genero);
            ImprimirResultados(resultados);
            Pausa();
        }

        static void AgregarLibro()
        {
            Console.WriteLine("\n  ➕ AGREGAR LIBRO");
            Console.Write("  ID       : "); int.TryParse(Console.ReadLine(), out int id);
            Console.Write("  Título   : "); string titulo  = Console.ReadLine() ?? "";
            Console.Write("  Autor    : "); string autor   = Console.ReadLine() ?? "";
            Console.Write("  Género   : "); string genero  = Console.ReadLine() ?? "";
            Console.Write("  Año      : "); int.TryParse(Console.ReadLine(), out int anio);
            Console.Write("  ISBN     : "); string isbn    = Console.ReadLine() ?? "";

            bool ok = biblioteca.AgregarLibro(new Libro(id, titulo, autor, genero, anio, isbn));
            Console.WriteLine(ok ? "  ✔ Libro agregado." : "  ✘ No se pudo agregar.");
            Pausa();
        }

        static void EliminarLibro()
        {
            Console.Write("\n  ID del libro a eliminar: ");
            if (int.TryParse(Console.ReadLine(), out int id))
                biblioteca.EliminarLibro(id);
            Pausa();
        }

        static void CambiarDisponibilidad()
        {
            Console.Write("\n  ID del libro: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Pausa(); return; }
            Console.Write("  ¿Disponible? (s/n): ");
            bool disp = (Console.ReadLine() ?? "s").ToLower() == "s";
            bool ok = biblioteca.CambiarDisponibilidad(id, disp);
            Console.WriteLine(ok ? "  ✔ Disponibilidad actualizada." : "  ⚠ ID no encontrado.");
            Pausa();
        }

        static void DemoOperacionesConjunto()
        {
            Console.WriteLine("\n  Ingresa dos autores para comparar sus conjuntos de libros:");
            Console.Write("  Autor A: "); string a = Console.ReadLine() ?? "";
            Console.Write("  Autor B: "); string b = Console.ReadLine() ?? "";
            biblioteca.OperacionesConjunto(a, b);
            Pausa();
        }

        static void ImprimirResultados(List<Libro> lista)
        {
            if (lista.Count == 0) Console.WriteLine("  ⚠ Sin resultados.");
            else foreach (var l in lista) Console.WriteLine("  " + l);
        }

        static void Pausa()
        {
            Console.Write("\n  Presiona Enter para continuar...");
            Console.ReadLine();
        }

        // --------------------------------------------------------
        //  DATOS INICIALES
        // --------------------------------------------------------
        static void CargarDatosIniciales()
        {
            var libros = new List<Libro>
            {
                // ── Carlos Cuauhtémoc Sánchez ──────────────────
                new(1,  "Juventud en éxtasis",              "Carlos Cuauhtémoc Sánchez", "Autoayuda / Novela",   1994, "978-968-7277-01-5"),
                new(2,  "La fuerza de Sheccid",             "Carlos Cuauhtémoc Sánchez", "Novela",               1995, "978-968-7277-03-9"),
                new(3,  "Un grito desesperado",             "Carlos Cuauhtémoc Sánchez", "Novela",               1992, "978-968-7277-00-8"),
                new(4,  "Volar sobre el pantano",           "Carlos Cuauhtémoc Sánchez", "Autoayuda",            1995, "978-968-7277-02-2"),
                new(5,  "Sangre de campeón",                "Carlos Cuauhtémoc Sánchez", "Juvenil",              1997, "978-968-7277-06-0"),

                // ── Marian Rojas Estapé ────────────────────────
                new(6,  "Cómo hacer que te pasen cosas buenas",  "Marian Rojas Estapé", "Autoayuda / Psicología", 2018, "978-84-08-18974-0"),
                new(7,  "Encuentra tu persona vitamina",         "Marian Rojas Estapé", "Psicología",             2021, "978-84-08-24149-6"),
                new(8,  "Cómo hacer que te pasen cosas buenas (edición especial)", "Marian Rojas Estapé", "Autoayuda", 2022, "978-84-08-26063-3"),

                // ── Sara Espejo ────────────────────────────────
                new(9,  "Eres extraordinaria",              "Sara Espejo",              "Autoayuda",            2020, "978-84-03-52057-2"),
                new(10, "El método Ikigai",                 "Sara Espejo",              "Desarrollo personal",  2021, "978-84-03-52179-1"),
                new(11, "Tú primero",                       "Sara Espejo",              "Autoayuda",            2022, "978-84-03-52301-6"),

                // ── Brian Tracy ────────────────────────────────
                new(12, "Trágate ese sapo",                 "Brian Tracy",              "Productividad",        2000, "978-607-07-0679-1"),
                new(13, "Máxima eficacia",                  "Brian Tracy",              "Productividad",        2001, "978-607-07-0752-1"),
                new(14, "Psicología del éxito",             "Brian Tracy",              "Autoayuda",            2003, "978-607-07-0810-8"),
                new(15, "El arte de cerrar la venta",       "Brian Tracy",              "Negocios",             2005, "978-607-07-0900-6"),
                new(16, "Metas",                            "Brian Tracy",              "Desarrollo personal",  2010, "978-607-07-0963-1"),

                // ── Otros autores de referencia ────────────────
                new(17, "Los cuatro acuerdos",              "Miguel Ruiz",              "Autoayuda",            1997, "978-1-878424-31-1"),
                new(18, "El poder del ahora",               "Eckhart Tolle",            "Espiritualidad",       1999, "978-1-57731-480-6"),
                new(19, "Hábitos atómicos",                 "James Clear",              "Productividad",        2018, "978-0-7352-1129-2"),
                new(20, "El monje que vendió su Ferrari",   "Robin Sharma",             "Autoayuda",            1997, "978-0-06-251619-2"),
            };

            // Marcar algunos como no disponibles
            libros[2].Disponible  = false;   // ID 3
            libros[11].Disponible = false;   // ID 12

            foreach (var l in libros) biblioteca.AgregarLibro(l);
        }
    }
}