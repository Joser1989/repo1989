using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace VuelosBaratos
{
    // ─────────────────────────────────────────────
    // Modelo de datos
    // ─────────────────────────────────────────────
    class Vuelo
    {
        public string Origen      { get; set; }
        public string Destino     { get; set; }
        public string Aerolinea   { get; set; }
        public double Precio      { get; set; }
        public string Horario     { get; set; }
        public int    Duracion    { get; set; }  // minutos

        public override string ToString() =>
            $"{Origen,-4} → {Destino,-4}  |  {Aerolinea,-12}  |  ${Precio,8:F2}  |  {Horario}  |  {Duracion} min";
    }

    // Nodo de prioridad para Dijkstra
    class NodoPrioridad : IComparable<NodoPrioridad>
    {
        public string Ciudad  { get; set; }
        public double Costo   { get; set; }
        public string Anterior{ get; set; }
        public string VueloUsado { get; set; }

        public int CompareTo(NodoPrioridad otro) => Costo.CompareTo(otro.Costo);
    }

    // ─────────────────────────────────────────────
    // Grafo de vuelos (lista de adyacencia)
    // ─────────────────────────────────────────────
    class GrafoVuelos
    {
        // ciudad → lista de vuelos disponibles desde allí
        private Dictionary<string, List<Vuelo>> _adyacencia = new();

        public IEnumerable<string> Ciudades => _adyacencia.Keys;

        public void AgregarVuelo(Vuelo v)
        {
            if (!_adyacencia.ContainsKey(v.Origen))
                _adyacencia[v.Origen] = new List<Vuelo>();
            _adyacencia[v.Origen].Add(v);

            // Asegurar que el destino también aparezca como nodo
            if (!_adyacencia.ContainsKey(v.Destino))
                _adyacencia[v.Destino] = new List<Vuelo>();
        }

        public List<Vuelo> VuelosDesde(string ciudad) =>
            _adyacencia.ContainsKey(ciudad) ? _adyacencia[ciudad] : new List<Vuelo>();

        // ─── Dijkstra por precio más bajo ───────────────────
        public (double costoTotal, List<Vuelo> ruta) Dijkstra(string origen, string destino)
        {
            var dist     = new Dictionary<string, double>();
            var anterior = new Dictionary<string, string>();
            var vueloUsado = new Dictionary<string, Vuelo>();
            var visitado = new HashSet<string>();
            var cola     = new SortedSet<NodoPrioridad>(Comparer<NodoPrioridad>.Create(
                (a, b) => a.Costo != b.Costo ? a.Costo.CompareTo(b.Costo)
                                              : string.Compare(a.Ciudad, b.Ciudad, StringComparison.Ordinal)));

            foreach (var c in _adyacencia.Keys)
                dist[c] = double.MaxValue;

            dist[origen] = 0;
            cola.Add(new NodoPrioridad { Ciudad = origen, Costo = 0 });

            while (cola.Count > 0)
            {
                var actual = cola.Min;
                cola.Remove(actual);

                if (visitado.Contains(actual.Ciudad)) continue;
                visitado.Add(actual.Ciudad);

                if (actual.Ciudad == destino) break;

                foreach (var vuelo in VuelosDesde(actual.Ciudad))
                {
                    double nuevoCosto = dist[actual.Ciudad] + vuelo.Precio;
                    if (nuevoCosto < dist[vuelo.Destino])
                    {
                        dist[vuelo.Destino]    = nuevoCosto;
                        anterior[vuelo.Destino] = actual.Ciudad;
                        vueloUsado[vuelo.Destino] = vuelo;
                        cola.Add(new NodoPrioridad { Ciudad = vuelo.Destino, Costo = nuevoCosto });
                    }
                }
            }

            // Reconstruir ruta
            if (dist[destino] == double.MaxValue)
                return (double.MaxValue, null);

            var ruta  = new List<Vuelo>();
            var ciudad = destino;
            while (vueloUsado.ContainsKey(ciudad))
            {
                ruta.Insert(0, vueloUsado[ciudad]);
                ciudad = anterior[ciudad];
            }
            return (dist[destino], ruta);
        }

        // ─── Todos los vuelos directos ───────────────────────
        public List<Vuelo> TodosLosVuelos()
        {
            var todos = new List<Vuelo>();
            foreach (var lista in _adyacencia.Values)
                todos.AddRange(lista);
            return todos.OrderBy(v => v.Precio).ToList();
        }

        // ─── Vuelos desde una ciudad ─────────────────────────
        public List<Vuelo> ConsultarDesde(string ciudad) =>
            VuelosDesde(ciudad).OrderBy(v => v.Precio).ToList();

        // ─── Top N más baratos a un destino (directos) ───────
        public List<Vuelo> TopBaratos(string destino, int n) =>
            TodosLosVuelos()
                .Where(v => v.Destino.Equals(destino, StringComparison.OrdinalIgnoreCase))
                .OrderBy(v => v.Precio)
                .Take(n)
                .ToList();
    }

    // ─────────────────────────────────────────────
    // Base de datos ficticia
    // ─────────────────────────────────────────────
    static class BaseDatosVuelos
    {
        public static GrafoVuelos Cargar()
        {
            var g = new GrafoVuelos();
            var vuelos = new List<Vuelo>
            {
                // UIO = Quito, GYE = Guayaquil, BOG = Bogotá, LIM = Lima
                // MIA = Miami,  MAD = Madrid,   MEX = Ciudad de México
                new Vuelo { Origen="UIO", Destino="GYE", Aerolinea="LATAM",      Precio= 89.99,  Horario="06:00", Duracion= 45 },
                new Vuelo { Origen="UIO", Destino="GYE", Aerolinea="Avianca",     Precio= 79.50,  Horario="08:30", Duracion= 45 },
                new Vuelo { Origen="UIO", Destino="GYE", Aerolinea="Viva Air",    Precio= 55.00,  Horario="11:00", Duracion= 50 },
                new Vuelo { Origen="UIO", Destino="BOG", Aerolinea="LATAM",      Precio=180.00,  Horario="07:15", Duracion=120 },
                new Vuelo { Origen="UIO", Destino="BOG", Aerolinea="Avianca",     Precio=165.75,  Horario="09:00", Duracion=120 },
                new Vuelo { Origen="UIO", Destino="LIM", Aerolinea="LATAM",      Precio=210.00,  Horario="10:00", Duracion=150 },
                new Vuelo { Origen="UIO", Destino="MIA", Aerolinea="American",    Precio=540.00,  Horario="23:00", Duracion=480 },
                new Vuelo { Origen="GYE", Destino="BOG", Aerolinea="Avianca",     Precio=140.00,  Horario="08:00", Duracion=110 },
                new Vuelo { Origen="GYE", Destino="MIA", Aerolinea="American",    Precio=490.00,  Horario="22:30", Duracion=460 },
                new Vuelo { Origen="GYE", Destino="LIM", Aerolinea="LATAM",      Precio=195.00,  Horario="12:00", Duracion=140 },
                new Vuelo { Origen="BOG", Destino="MIA", Aerolinea="Avianca",     Precio=320.00,  Horario="14:00", Duracion=300 },
                new Vuelo { Origen="BOG", Destino="MAD", Aerolinea="Iberia",      Precio=680.00,  Horario="22:00", Duracion=600 },
                new Vuelo { Origen="BOG", Destino="MEX", Aerolinea="Aeromexico",  Precio=410.00,  Horario="16:00", Duracion=390 },
                new Vuelo { Origen="LIM", Destino="BOG", Aerolinea="LATAM",      Precio=175.00,  Horario="09:30", Duracion=130 },
                new Vuelo { Origen="LIM", Destino="MIA", Aerolinea="American",    Precio=520.00,  Horario="21:00", Duracion=510 },
                new Vuelo { Origen="LIM", Destino="MAD", Aerolinea="Air Europa",  Precio=720.00,  Horario="23:30", Duracion=660 },
                new Vuelo { Origen="MIA", Destino="MAD", Aerolinea="Iberia",      Precio=490.00,  Horario="19:00", Duracion=510 },
                new Vuelo { Origen="MIA", Destino="MEX", Aerolinea="Aeromexico",  Precio=280.00,  Horario="08:00", Duracion=210 },
                new Vuelo { Origen="MEX", Destino="MAD", Aerolinea="Iberia",      Precio=550.00,  Horario="20:00", Duracion=630 },
                new Vuelo { Origen="MAD", Destino="BOG", Aerolinea="Avianca",     Precio=640.00,  Horario="12:00", Duracion=600 },
            };

            foreach (var v in vuelos) g.AgregarVuelo(v);
            return g;
        }
    }

    // ─────────────────────────────────────────────
    // Menú principal
    // ─────────────────────────────────────────────
    class Program
    {
        static GrafoVuelos grafo = BaseDatosVuelos.Cargar();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            bool salir = false;

            while (!salir)
            {
                MostrarMenu();
                string opcion = Console.ReadLine()?.Trim();
                Console.WriteLine();

                switch (opcion)
                {
                    case "1": ListarTodosLosVuelos();      break;
                    case "2": BuscarVuelosDesde();         break;
                    case "3": BuscarRutaMasBarata();       break;
                    case "4": TopVuelosBaratos();          break;
                    case "5": AnalizarTiempoEjecucion();   break;
                    case "6": salir = true; break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("  Opcion invalida. Intente de nuevo.");
                        Console.ResetColor();
                        break;
                }

                if (!salir)
                {
                    Console.WriteLine("\n  Presione ENTER para continuar...");
                    Console.ReadLine();
                }
            }

            Console.WriteLine("\n  Gracias por usar el Sistema de Vuelos Baratos. ¡Buen viaje!\n");
        }

        // ─── Menú ────────────────────────────────────────────
        static void MostrarMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════════════════╗");
            Console.WriteLine("║       SISTEMA DE BÚSQUEDA DE VUELOS BARATOS      ║");
            Console.WriteLine("║         Universidad Estatal Amazónica - TI       ║");
            Console.WriteLine("╚══════════════════════════════════════════════════╝");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("  [1]  Listar todos los vuelos (reporte completo)");
            Console.WriteLine("  [2]  Consultar vuelos desde una ciudad");
            Console.WriteLine("  [3]  Ruta más barata entre dos ciudades (Dijkstra)");
            Console.WriteLine("  [4]  Top N vuelos más baratos hacia un destino");
            Console.WriteLine("  [5]  Análisis de tiempo de ejecución");
            Console.WriteLine("  [6]  Salir");
            Console.WriteLine();
            Console.Write("  Seleccione una opcion: ");
        }

        // ─── Opción 1 ────────────────────────────────────────
        static void ListarTodosLosVuelos()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("═══════════════════ TODOS LOS VUELOS (ordenado por precio) ═══════════════════");
            Console.ResetColor();

            var lista = grafo.TodosLosVuelos();
            int i = 1;
            foreach (var v in lista)
                Console.WriteLine($"  {i++,2}. {v}");

            Console.WriteLine($"\n  Total de vuelos en la base de datos: {lista.Count}");
        }

        // ─── Opción 2 ────────────────────────────────────────
        static void BuscarVuelosDesde()
        {
            Console.Write("  Ciudad de origen (UIO/GYE/BOG/LIM/MIA/MAD/MEX): ");
            string origen = Console.ReadLine()?.Trim().ToUpper();

            var vuelos = grafo.ConsultarDesde(origen);
            if (vuelos.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"  No se encontraron vuelos desde '{origen}'.");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n  ═══ Vuelos desde {origen} ═══");
            Console.ResetColor();

            int i = 1;
            foreach (var v in vuelos)
                Console.WriteLine($"  {i++,2}. {v}");
        }

        // ─── Opción 3 ────────────────────────────────────────
        static void BuscarRutaMasBarata()
        {
            Console.Write("  Ciudad de origen  (UIO/GYE/BOG/LIM/MIA/MAD/MEX): ");
            string origen  = Console.ReadLine()?.Trim().ToUpper();
            Console.Write("  Ciudad de destino (UIO/GYE/BOG/LIM/MIA/MAD/MEX): ");
            string destino = Console.ReadLine()?.Trim().ToUpper();

            var sw = Stopwatch.StartNew();
            var (costo, ruta) = grafo.Dijkstra(origen, destino);
            sw.Stop();

            if (ruta == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  No existe ruta disponible entre esas ciudades.");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n  ═══ Ruta más barata: {origen} → {destino} ═══");
            Console.ResetColor();

            int seg = 1;
            foreach (var v in ruta)
                Console.WriteLine($"    Segmento {seg++}: {v}");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n  ✈  COSTO TOTAL:  ${costo:F2}");
            Console.WriteLine($"     Conexiones:   {ruta.Count}");
            Console.WriteLine($"     Tiempo Dijkstra: {sw.Elapsed.TotalMilliseconds:F4} ms");
            Console.ResetColor();
        }

        // ─── Opción 4 ────────────────────────────────────────
        static void TopVuelosBaratos()
        {
            Console.Write("  Ciudad de destino (UIO/GYE/BOG/LIM/MIA/MAD/MEX): ");
            string destino = Console.ReadLine()?.Trim().ToUpper();
            Console.Write("  ¿Cuántos vuelos mostrar? (ej. 3): ");
            if (!int.TryParse(Console.ReadLine(), out int n)) n = 3;

            var lista = grafo.TopBaratos(destino, n);
            if (lista.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"  No hay vuelos directos hacia '{destino}'.");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n  ═══ Top {n} vuelos más baratos hacia {destino} ═══");
            Console.ResetColor();

            int i = 1;
            foreach (var v in lista)
                Console.WriteLine($"  {i++}. {v}");
        }

        // ─── Opción 5 ────────────────────────────────────────
        static void AnalizarTiempoEjecucion()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("═══════════════════ ANÁLISIS DE TIEMPO DE EJECUCIÓN ═══════════════════");
            Console.ResetColor();

            string[] pares = {
                "UIO-GYE", "UIO-MIA", "UIO-MAD", "GYE-MAD", "LIM-MAD", "UIO-MEX"
            };

            Console.WriteLine($"\n  {"Par origen-destino",-15}  {"Costo ($)",10}  {"Tiempo (ms)",12}  {"Saltos",6}");
            Console.WriteLine(new string('─', 55));

            foreach (var par in pares)
            {
                var partes  = par.Split('-');
                string o    = partes[0], d = partes[1];

                var sw = Stopwatch.StartNew();
                var (costo, ruta) = grafo.Dijkstra(o, d);
                sw.Stop();

                string costoStr = costo == double.MaxValue ? "N/A" : $"{costo:F2}";
                int saltos = ruta?.Count ?? 0;
                Console.WriteLine($"  {par,-15}  {costoStr,10}  {sw.Elapsed.TotalMilliseconds,12:F4}  {saltos,6}");
            }

            // Comparar con búsqueda lineal
            Console.WriteLine("\n  ─── Comparativa: Dijkstra vs. Búsqueda lineal (vuelos directos) ───");

            var swLineal = Stopwatch.StartNew();
            var directos = grafo.TodosLosVuelos()
                .Where(v => v.Origen == "UIO" && v.Destino == "MIA")
                .OrderBy(v => v.Precio)
                .ToList();
            swLineal.Stop();

            var swDijkstra = Stopwatch.StartNew();
            grafo.Dijkstra("UIO", "MIA");
            swDijkstra.Stop();

            Console.WriteLine($"\n  Búsqueda lineal  UIO→MIA: {swLineal.Elapsed.TotalMilliseconds:F6} ms");
            Console.WriteLine($"  Dijkstra         UIO→MIA: {swDijkstra.Elapsed.TotalMilliseconds:F6} ms");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("  NOTA: Dijkstra garantiza la ruta óptima incluso con escalas.");
            Console.WriteLine("        Complejidad: O((V + E) log V)  donde V=ciudades, E=vuelos.");
            Console.ResetColor();
        }
    }
}