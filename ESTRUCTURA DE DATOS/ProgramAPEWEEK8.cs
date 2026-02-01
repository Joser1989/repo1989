using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaColaParqueDiversiones
{
    /// <summary>
    /// Clase que representa a una persona en la cola del parque de atracciones.
    /// </summary>
    public class Persona
    {
        public string Nombre { get; set; }
        public int NumeroTicket { get; set; }
        public DateTime HoraLlegada { get; set; }

        public Persona(string nombre, int numeroTicket)
        {
            Nombre = nombre;
            NumeroTicket = numeroTicket;
            HoraLlegada = DateTime.Now;
        }

        public override string ToString()
        {
            return $"Ticket #{NumeroTicket}: {Nombre} (Llegada: {HoraLlegada:HH:mm:ss})";
        }
    }

    /// <summary>
    /// Clase que gestiona la cola de la atracción del parque
    /// Implementa el principio FIFO (First In, First Out)
    /// </summary>
    public class ColaAtraccion
    {
        private Queue<Persona> cola;
        private const int CAPACIDAD_ATRACCION = 30;
        private int contadorTickets;
        private int ciclosCompletados;
        private int totalPersonasAtendidas;

        public ColaAtraccion()
        {
            cola = new Queue<Persona>();
            contadorTickets = 1;
            ciclosCompletados = 0;
            totalPersonasAtendidas = 0;
        }

        /// <summary>
        /// Agrega una nueva persona a la cola
        /// Complejidad: O(1)
        /// </summary>
        public void AgregarPersona(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                Console.WriteLine("\n❌ Error: El nombre no puede estar vacío.");
                return;
            }

            Persona nuevaPersona = new Persona(nombre, contadorTickets++);
            cola.Enqueue(nuevaPersona);
            
            Console.WriteLine($"\n✅ {nuevaPersona.Nombre} agregado a la cola.");
            Console.WriteLine($"   Ticket asignado: #{nuevaPersona.NumeroTicket}");
            Console.WriteLine($"   Posición en la cola: {cola.Count}");
        }

        /// <summary>
        /// Procesa un grupo de personas (hasta 30) para subirlas a la atracción
        /// Complejidad: O(n) donde n = CAPACIDAD_ATRACCION (constante = 30)
        /// </summary>
        public void ProcesarGrupo()
        {
            if (cola.Count < CAPACIDAD_ATRACCION)
            {
                Console.WriteLine($"\n⚠️  No hay suficientes personas en la cola.");
                Console.WriteLine($"   Se necesitan {CAPACIDAD_ATRACCION} personas, pero solo hay {cola.Count}.");
                Console.WriteLine("   Agregue más personas antes de iniciar el ciclo.");
                return;
            }

            Console.WriteLine("\n" + new string('=', 70));
            Console.WriteLine($"🎢 INICIANDO CICLO #{ciclosCompletados + 1} DE LA ATRACCIÓN");
            Console.WriteLine(new string('=', 70));
            Console.WriteLine($"Capacidad: {CAPACIDAD_ATRACCION} asientos");
            Console.WriteLine();

            List<Persona> grupoAtendido = new List<Persona>();

            for (int i = 0; i < CAPACIDAD_ATRACCION; i++)
            {
                Persona persona = cola.Dequeue();
                grupoAtendido.Add(persona);
                totalPersonasAtendidas++;
            }

            // Mostrar las personas que suben a la atracción
            Console.WriteLine("📋 PERSONAS ABORDANDO LA ATRACCIÓN:");
            Console.WriteLine(new string('-', 70));
            
            for (int i = 0; i < grupoAtendido.Count; i++)
            {
                TimeSpan tiempoEspera = DateTime.Now - grupoAtendido[i].HoraLlegada;
                Console.WriteLine($"  Asiento {i + 1,2}: {grupoAtendido[i].ToString()} " +
                                $"- Esperó: {tiempoEspera.TotalSeconds:F1}s");
            }

            ciclosCompletados++;
            Console.WriteLine(new string('-', 70));
            Console.WriteLine($"✅ Ciclo completado exitosamente!");
            Console.WriteLine($"   Personas restantes en cola: {cola.Count}");
            Console.WriteLine(new string('=', 70));
        }

        /// <summary>
        /// Muestra el estado actual de la cola
        /// Complejidad: O(n) donde n = cantidad de elementos en la cola
        /// </summary>
        public void MostrarCola()
        {
            Console.WriteLine("\n" + new string('=', 70));
            Console.WriteLine("📊 ESTADO ACTUAL DE LA COLA");
            Console.WriteLine(new string('=', 70));

            if (cola.Count == 0)
            {
                Console.WriteLine("   La cola está vacía.");
            }
            else
            {
                Console.WriteLine($"   Total de personas en espera: {cola.Count}");
                Console.WriteLine($"   Personas necesarias para llenar atracción: " +
                                $"{Math.Max(0, CAPACIDAD_ATRACCION - cola.Count)}");
                Console.WriteLine();
                Console.WriteLine("   Personas en la cola:");
                Console.WriteLine("   " + new string('-', 66));

                int posicion = 1;
                foreach (Persona persona in cola)
                {
                    TimeSpan tiempoEspera = DateTime.Now - persona.HoraLlegada;
                    string indicador = posicion <= CAPACIDAD_ATRACCION ? "🎫" : "⏳";
                    Console.WriteLine($"   {indicador} Pos {posicion,3}: {persona.ToString()} " +
                                    $"- Esperando: {tiempoEspera.TotalSeconds:F1}s");
                    posicion++;
                }

                if (cola.Count >= CAPACIDAD_ATRACCION)
                {
                    Console.WriteLine("\n   ✅ Hay suficientes personas para iniciar el siguiente ciclo!");
                }
                else
                {
                    Console.WriteLine($"\n   ⏳ Esperando {CAPACIDAD_ATRACCION - cola.Count} personas más...");
                }
            }

            Console.WriteLine(new string('=', 70));
        }

        /// <summary>
        /// Muestra estadísticas detalladas del sistema
        /// Complejidad: O(1)
        /// </summary>
        public void MostrarEstadisticas()
        {
            Console.WriteLine("\n" + new string('=', 70));
            Console.WriteLine("📈 ESTADÍSTICAS DEL SISTEMA");
            Console.WriteLine(new string('=', 70));
            Console.WriteLine($"   Ciclos completados:          {ciclosCompletados}");
            Console.WriteLine($"   Total personas atendidas:    {totalPersonasAtendidas}");
            Console.WriteLine($"   Personas en cola actual:     {cola.Count}");
            Console.WriteLine($"   Tickets emitidos:            {contadorTickets - 1}");
            Console.WriteLine($"   Capacidad por ciclo:         {CAPACIDAD_ATRACCION} personas");
            
            if (ciclosCompletados > 0)
            {
                double promedioPorCiclo = (double)totalPersonasAtendidas / ciclosCompletados;
                Console.WriteLine($"   Promedio por ciclo:          {promedioPorCiclo:F2} personas");
            }

            Console.WriteLine(new string('=', 70));
        }

        /// <summary>
        /// Análisis de complejidad temporal de las operaciones
        /// </summary>
        public void MostrarAnalisisComplejidad()
        {
            Console.WriteLine("\n" + new string('=', 70));
            Console.WriteLine("⚡ ANÁLISIS DE COMPLEJIDAD TEMPORAL");
            Console.WriteLine(new string('=', 70));
            Console.WriteLine("   Operación              | Complejidad | Justificación");
            Console.WriteLine("   " + new string('-', 66));
            Console.WriteLine("   AgregarPersona()       | O(1)        | Enqueue() opera en tiempo constante");
            Console.WriteLine("   ProcesarGrupo()        | O(n)        | Ejecuta n=30 operaciones Dequeue()");
            Console.WriteLine("   MostrarCola()          | O(n)        | Itera sobre n elementos de la cola");
            Console.WriteLine("   MostrarEstadisticas()  | O(1)        | Acceso directo a variables contadoras");
            Console.WriteLine(new string('=', 70));
            Console.WriteLine("\n   📌 Ventajas de usar Queue<T>:");
            Console.WriteLine("      • Implementación optimizada por .NET Framework");
            Console.WriteLine("      • Operaciones de encolado/desencolado en O(1)");
            Console.WriteLine("      • Gestión automática de memoria dinámica");
            Console.WriteLine("      • Type-safe gracias a los genéricos");
            Console.WriteLine("\n   📌 Desventajas:");
            Console.WriteLine("      • No permite acceso aleatorio a elementos intermedios");
            Console.WriteLine("      • No es eficiente para búsquedas por valor (requiere O(n))");
            Console.WriteLine("      • Consume más memoria que arrays para colecciones pequeñas");
            Console.WriteLine(new string('=', 70));
        }

        public int ObtenerCantidadEnCola()
        {
            return cola.Count;
        }

        public int ObtenerCiclosCompletados()
        {
            return ciclosCompletados;
        }
    }

    /// <summary>
    /// Clase principal que ejecuta el programa
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            ColaAtraccion sistema = new ColaAtraccion();
            bool continuar = true;

            MostrarBienvenida();

            while (continuar)
            {
                MostrarMenu();
                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        AgregarPersonasInteractivo(sistema);
                        break;

                    case "2":
                        sistema.ProcesarGrupo();
                        break;

                    case "3":
                        sistema.MostrarCola();
                        break;

                    case "4":
                        sistema.MostrarEstadisticas();
                        break;

                    case "5":
                        sistema.MostrarAnalisisComplejidad();
                        break;

                    case "6":
                        CargarDatosDePrueba(sistema);
                        break;

                    case "7":
                        SimulacionAutomatica(sistema);
                        break;

                    case "0":
                        continuar = false;
                        MostrarDespedida(sistema);
                        break;

                    default:
                        Console.WriteLine("\n❌ Opción inválida. Por favor, intente nuevamente.");
                        break;
                }

                if (continuar)
                {
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }

        static void MostrarBienvenida()
        {
            Console.Clear();
            Console.WriteLine(new string('=', 70));
            Console.WriteLine("🎢 SISTEMA DE GESTIÓN DE COLA - PARQUE DE DIVERSIONES 🎢");
            Console.WriteLine(new string('=', 70));
            Console.WriteLine("   Implementación de estructura de datos: COLA (Queue)");
            Console.WriteLine("   Principio FIFO: First In, First Out");
            Console.WriteLine("   Capacidad de atracción: 30 asientos por ciclo");
            Console.WriteLine(new string('=', 70));
            Console.WriteLine();
        }

        static void MostrarMenu()
        {
            Console.WriteLine("\n" + new string('-', 70));
            Console.WriteLine("MENÚ PRINCIPAL");
            Console.WriteLine(new string('-', 70));
            Console.WriteLine("   1. Agregar persona(s) a la cola");
            Console.WriteLine("   2. Procesar grupo (iniciar ciclo de atracción)");
            Console.WriteLine("   3. Ver estado de la cola");
            Console.WriteLine("   4. Ver estadísticas del sistema");
            Console.WriteLine("   5. Ver análisis de complejidad temporal");
            Console.WriteLine("   6. Cargar datos de prueba (50 personas)");
            Console.WriteLine("   7. Simulación automática completa");
            Console.WriteLine("   0. Salir del sistema");
            Console.WriteLine(new string('-', 70));
            Console.Write("   Seleccione una opción: ");
        }

        static void AgregarPersonasInteractivo(ColaAtraccion sistema)
        {
            Console.Write("\n¿Cuántas personas desea agregar? ");
            if (int.TryParse(Console.ReadLine(), out int cantidad) && cantidad > 0)
            {
                for (int i = 1; i <= cantidad; i++)
                {
                    Console.Write($"\nNombre de la persona {i}: ");
                    string nombre = Console.ReadLine();
                    sistema.AgregarPersona(nombre);
                }
                Console.WriteLine($"\n✅ Se agregaron {cantidad} persona(s) exitosamente!");
            }
            else
            {
                Console.WriteLine("\n❌ Cantidad inválida.");
            }
        }

        static void CargarDatosDePrueba(ColaAtraccion sistema)
        {
            string[] nombres = {
                "Ana García", "Carlos Pérez", "María López", "Juan Rodríguez", "Laura Martínez",
                "Pedro Sánchez", "Carmen Fernández", "José González", "Isabel Torres", "Miguel Ramírez",
                "Elena Díaz", "Francisco Morales", "Patricia Jiménez", "Alberto Ruiz", "Sofía Hernández",
                "Daniel Castro", "Lucía Ortiz", "Javier Romero", "Marta Navarro", "Roberto Domínguez",
                "Cristina Vázquez", "Antonio Ramos", "Rosa Gil", "Manuel Serrano", "Beatriz Molina",
                "Ángel Blanco", "Teresa Suárez", "Raúl Prieto", "Pilar Pascual", "Sergio Santana",
                "Alicia Méndez", "Fernando Cruz", "Dolores Herrera", "Enrique Peña", "Silvia Campos",
                "Pablo Iglesias", "Amparo Garrido", "Luis Cortés", "Consuelo Cabrera", "Víctor Márquez",
                "Inmaculada León", "Diego Guerrero", "Montserrat Medina", "Rubén Castillo", "Nuria Vargas",
                "Óscar Santos", "Gloria Carmona", "Alejandro Lozano", "Remedios Benítez", "Ignacio Gil"
            };

            Console.WriteLine("\n🔄 Cargando datos de prueba...");
            foreach (string nombre in nombres)
            {
                sistema.AgregarPersona(nombre);
            }
            Console.WriteLine($"\n✅ Se cargaron {nombres.Length} personas de prueba exitosamente!");
        }

        static void SimulacionAutomatica(ColaAtraccion sistema)
        {
            Console.WriteLine("\n🎬 INICIANDO SIMULACIÓN AUTOMÁTICA");
            Console.WriteLine(new string('=', 70));
            
            // Cargar 75 personas
            CargarDatosDePrueba(sistema);
            
            Console.WriteLine("\nPresione una tecla para ver la cola inicial...");
            Console.ReadKey();
            sistema.MostrarCola();

            // Procesar 2 ciclos completos
            for (int i = 1; i <= 2; i++)
            {
                Console.WriteLine($"\n\nPresione una tecla para procesar el ciclo {i}...");
                Console.ReadKey();
                sistema.ProcesarGrupo();
            }

            Console.WriteLine("\n\nPresione una tecla para ver el estado final...");
            Console.ReadKey();
            sistema.MostrarCola();

            Console.WriteLine("\n\nPresione una tecla para ver las estadísticas finales...");
            Console.ReadKey();
            sistema.MostrarEstadisticas();

            Console.WriteLine("\n✅ Simulación completada!");
        }

        static void MostrarDespedida(ColaAtraccion sistema)
        {
            Console.Clear();
            Console.WriteLine(new string('=', 70));
            Console.WriteLine("👋 GRACIAS POR USAR EL SISTEMA DE GESTIÓN DE COLA RAM BROS");
            Console.WriteLine(new string('=', 70));
            Console.WriteLine("\n📊 RESUMEN FINAL:");
            sistema.MostrarEstadisticas();
            Console.WriteLine("\n¡Hasta pronto!");
            Console.WriteLine(new string('=', 70));
        }
    }
}

