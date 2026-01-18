using System;

namespace EjerciciosListasEnlazadasweek6
{
    // =====================================================================
    // EJERCICIO #1: SISTEMA DE ESTACIONAMIENTO DE VEHÍCULOS
    // =====================================================================
    
    public class NodoVehiculo
    {
        public string Placa { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Año { get; set; }
        public decimal Precio { get; set; }
        public NodoVehiculo Siguiente { get; set; }

        public NodoVehiculo(string placa, string marca, string modelo, int año, decimal precio)
        {
            Placa = placa;
            Marca = marca;
            Modelo = modelo;
            Año = año;
            Precio = precio;
            Siguiente = null;
        }

        public void MostrarInformacion()
        {
            Console.WriteLine($"\n{'=',-40}");
            Console.WriteLine($"Placa:   {Placa}");
            Console.WriteLine($"Marca:   {Marca}");
            Console.WriteLine($"Modelo:  {Modelo}");
            Console.WriteLine($"Año:     {Año}");
            Console.WriteLine($"Precio:  ${Precio:N2}");
            Console.WriteLine($"{'=',-40}");
        }
    }

    public class ListaVehiculos
    {
        private NodoVehiculo cabeza;

        public ListaVehiculos()
        {
            cabeza = null;
        }

        public void AgregarVehiculo(string placa, string marca, string modelo, int año, decimal precio)
        {
            NodoVehiculo nuevoNodo = new NodoVehiculo(placa, marca, modelo, año, precio);

            if (cabeza == null)
            {
                cabeza = nuevoNodo;
            }
            else
            {
                NodoVehiculo actual = cabeza;
                while (actual.Siguiente != null)
                {
                    actual = actual.Siguiente;
                }
                actual.Siguiente = nuevoNodo;
            }

            Console.WriteLine($"\n✓ Vehículo con placa {placa} agregado exitosamente.");
        }

        public void BuscarVehiculoPorPlaca(string placa)
        {
            if (cabeza == null)
            {
                Console.WriteLine("\n✗ El estacionamiento está vacío.");
                return;
            }

            NodoVehiculo actual = cabeza;
            while (actual != null)
            {
                if (actual.Placa.Equals(placa, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("\n✓ Vehículo encontrado:");
                    actual.MostrarInformacion();
                    return;
                }
                actual = actual.Siguiente;
            }

            Console.WriteLine($"\n✗ No se encontró ningún vehículo con la placa: {placa}");
        }

        public void VerVehiculosPorAño(int año)
        {
            if (cabeza == null)
            {
                Console.WriteLine("\n✗ El estacionamiento está vacío.");
                return;
            }

            NodoVehiculo actual = cabeza;
            bool encontrado = false;

            Console.WriteLine($"\n--- VEHÍCULOS DEL AÑO {año} ---");

            while (actual != null)
            {
                if (actual.Año == año)
                {
                    actual.MostrarInformacion();
                    encontrado = true;
                }
                actual = actual.Siguiente;
            }

            if (!encontrado)
            {
                Console.WriteLine($"\n✗ No hay vehículos del año {año} registrados.");
            }
        }

        public void VerTodosLosVehiculos()
        {
            if (cabeza == null)
            {
                Console.WriteLine("\n✗ El estacionamiento está vacío.");
                return;
            }

            Console.WriteLine("\n--- TODOS LOS VEHÍCULOS REGISTRADOS ---");
            NodoVehiculo actual = cabeza;
            int contador = 1;

            while (actual != null)
            {
                Console.WriteLine($"\nVehículo #{contador}:");
                actual.MostrarInformacion();
                actual = actual.Siguiente;
                contador++;
            }

            Console.WriteLine($"\nTotal de vehículos: {contador - 1}");
        }

        public void EliminarVehiculo(string placa)
        {
            if (cabeza == null)
            {
                Console.WriteLine("\n✗ El estacionamiento está vacío.");
                return;
            }

            if (cabeza.Placa.Equals(placa, StringComparison.OrdinalIgnoreCase))
            {
                cabeza = cabeza.Siguiente;
                Console.WriteLine($"\n✓ Vehículo con placa {placa} eliminado exitosamente.");
                return;
            }

            NodoVehiculo actual = cabeza;
            while (actual.Siguiente != null)
            {
                if (actual.Siguiente.Placa.Equals(placa, StringComparison.OrdinalIgnoreCase))
                {
                    actual.Siguiente = actual.Siguiente.Siguiente;
                    Console.WriteLine($"\n✓ Vehículo con placa {placa} eliminado exitosamente.");
                    return;
                }
                actual = actual.Siguiente;
            }

            Console.WriteLine($"\n✗ No se encontró ningún vehículo con la placa: {placa}");
        }
    }

    // =====================================================================
    // EJERCICIO #2: SISTEMA DE REGISTRO DE ESTUDIANTES - REDES III
    // =====================================================================

    public class NodoEstudiante
    {
        public string Cedula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public double NotaDefinitiva { get; set; }
        public NodoEstudiante Siguiente { get; set; }

        public NodoEstudiante(string cedula, string nombre, string apellido, string correo, double notaDefinitiva)
        {
            Cedula = cedula;
            Nombre = nombre;
            Apellido = apellido;
            Correo = correo;
            NotaDefinitiva = notaDefinitiva;
            Siguiente = null;
        }

        public bool EstaAprobado()
        {
            return NotaDefinitiva >= 7.0;
        }

        public void MostrarInformacion()
        {
            Console.WriteLine($"\n{'=',-50}");
            Console.WriteLine($"Cédula:          {Cedula}");
            Console.WriteLine($"Nombre:          {Nombre} {Apellido}");
            Console.WriteLine($"Correo:          {Correo}");
            Console.WriteLine($"Nota Definitiva: {NotaDefinitiva:F2}");
            Console.WriteLine($"Estado:          {(EstaAprobado() ? "APROBADO ✓" : "REPROBADO ✗")}");
            Console.WriteLine($"{'=',-50}");
        }
    }

    public class ListaEstudiantes
    {
        private NodoEstudiante cabeza;

        public ListaEstudiantes()
        {
            cabeza = null;
        }

        public void AgregarEstudiante(string cedula, string nombre, string apellido, string correo, double notaDefinitiva)
        {
            NodoEstudiante nuevoNodo = new NodoEstudiante(cedula, nombre, apellido, correo, notaDefinitiva);

            if (nuevoNodo.EstaAprobado())
            {
                nuevoNodo.Siguiente = cabeza;
                cabeza = nuevoNodo;
                Console.WriteLine($"\n✓ Estudiante {nombre} {apellido} agregado al inicio (APROBADO).");
            }
            else
            {
                if (cabeza == null)
                {
                    cabeza = nuevoNodo;
                }
                else
                {
                    NodoEstudiante actual = cabeza;
                    while (actual.Siguiente != null)
                    {
                        actual = actual.Siguiente;
                    }
                    actual.Siguiente = nuevoNodo;
                }
                Console.WriteLine($"\n✓ Estudiante {nombre} {apellido} agregado al final (REPROBADO).");
            }
        }

        public void BuscarEstudiantePorCedula(string cedula)
        {
            if (cabeza == null)
            {
                Console.WriteLine("\n✗ No hay estudiantes registrados.");
                return;
            }

            NodoEstudiante actual = cabeza;
            while (actual != null)
            {
                if (actual.Cedula.Equals(cedula, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("\n✓ Estudiante encontrado:");
                    actual.MostrarInformacion();
                    return;
                }
                actual = actual.Siguiente;
            }

            Console.WriteLine($"\n✗ No se encontró ningún estudiante con la cédula: {cedula}");
        }

        public void EliminarEstudiante(string cedula)
        {
            if (cabeza == null)
            {
                Console.WriteLine("\n✗ No hay estudiantes registrados.");
                return;
            }

            if (cabeza.Cedula.Equals(cedula, StringComparison.OrdinalIgnoreCase))
            {
                string nombreCompleto = cabeza.Nombre + " " + cabeza.Apellido;
                cabeza = cabeza.Siguiente;
                Console.WriteLine($"\n✓ Estudiante {nombreCompleto} (Cédula: {cedula}) eliminado exitosamente.");
                return;
            }

            NodoEstudiante actual = cabeza;
            while (actual.Siguiente != null)
            {
                if (actual.Siguiente.Cedula.Equals(cedula, StringComparison.OrdinalIgnoreCase))
                {
                    string nombreCompleto = actual.Siguiente.Nombre + " " + actual.Siguiente.Apellido;
                    actual.Siguiente = actual.Siguiente.Siguiente;
                    Console.WriteLine($"\n✓ Estudiante {nombreCompleto} (Cédula: {cedula}) eliminado exitosamente.");
                    return;
                }
                actual = actual.Siguiente;
            }

            Console.WriteLine($"\n✗ No se encontró ningún estudiante con la cédula: {cedula}");
        }

        public void TotalEstudiantesAprobados()
        {
            if (cabeza == null)
            {
                Console.WriteLine("\n✗ No hay estudiantes registrados.");
                return;
            }

            int contador = 0;
            NodoEstudiante actual = cabeza;

            Console.WriteLine("\n--- ESTUDIANTES APROBADOS ---");

            while (actual != null)
            {
                if (actual.EstaAprobado())
                {
                    contador++;
                    actual.MostrarInformacion();
                }
                actual = actual.Siguiente;
            }

            Console.WriteLine($"\nTotal de estudiantes aprobados: {contador}");
        }

        public void TotalEstudiantesReprobados()
        {
            if (cabeza == null)
            {
                Console.WriteLine("\n✗ No hay estudiantes registrados.");
                return;
            }

            int contador = 0;
            NodoEstudiante actual = cabeza;

            Console.WriteLine("\n--- ESTUDIANTES REPROBADOS ---");

            while (actual != null)
            {
                if (!actual.EstaAprobado())
                {
                    contador++;
                    actual.MostrarInformacion();
                }
                actual = actual.Siguiente;
            }

            Console.WriteLine($"\nTotal de estudiantes reprobados: {contador}");
        }

        public void MostrarTodosLosEstudiantes()
        {
            if (cabeza == null)
            {
                Console.WriteLine("\n✗ No hay estudiantes registrados.");
                return;
            }

            Console.WriteLine("\n--- TODOS LOS ESTUDIANTES ---");
            NodoEstudiante actual = cabeza;
            int contador = 1;

            while (actual != null)
            {
                Console.WriteLine($"\nEstudiante #{contador}:");
                actual.MostrarInformacion();
                actual = actual.Siguiente;
                contador++;
            }

            Console.WriteLine($"\nTotal de estudiantes: {contador - 1}");
        }
    }

    // =====================================================================
    // PROGRAMA PRINCIPAL
    // =====================================================================

    class Program
    {
        static ListaVehiculos estacionamiento = new ListaVehiculos();
        static ListaEstudiantes listaRedesIII = new ListaEstudiantes();

        static void Main(string[] args)
        {
            int opcion;

            do
            {
                MostrarMenuPrincipal();
                opcion = LeerOpcion();

                switch (opcion)
                {
                    case 1:
                        MenuEstacionamiento();
                        break;
                    case 2:
                        MenuRedesIII();
                        break;
                    case 3:
                        Console.WriteLine("\n¡Gracias por usar el sistema! Hasta luego.");
                        break;
                    default:
                        Console.WriteLine("\n✗ Opción inválida. Intente nuevamente.");
                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();
                        break;
                }

            } while (opcion != 3);
        }

        static void MostrarMenuPrincipal()
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════════════╗");
            Console.WriteLine("║       SISTEMA DE LISTAS ENLAZADAS - C#               ║");
            Console.WriteLine("║         Universidad - Ingeniería de Sistemas         ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════╝");
            Console.WriteLine("\n Seleccione el ejercicio que desea ejecutar:");
            Console.WriteLine("\n 1. EJERCICIO #1 - Sistema de Estacionamiento");
            Console.WriteLine(" 2. EJERCICIO #2 - Sistema de Redes III");
            Console.WriteLine(" 3. Salir del programa");
            Console.Write("\n Opción: ");
        }

        // =====================================================================
        // MENÚ EJERCICIO #1: ESTACIONAMIENTO
        // =====================================================================

        static void MenuEstacionamiento()
        {
            int opcion;

            do
            {
                MostrarMenuEstacionamiento();
                opcion = LeerOpcion();

                switch (opcion)
                {
                    case 1:
                        AgregarVehiculoMenu();
                        break;
                    case 2:
                        BuscarVehiculoMenu();
                        break;
                    case 3:
                        VerVehiculosPorAñoMenu();
                        break;
                    case 4:
                        estacionamiento.VerTodosLosVehiculos();
                        break;
                    case 5:
                        EliminarVehiculoMenu();
                        break;
                    case 6:
                        Console.WriteLine("\nRegresando al menú principal...");
                        break;
                    default:
                        Console.WriteLine("\n✗ Opción inválida. Intente nuevamente.");
                        break;
                }

                if (opcion != 6)
                {
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                }

            } while (opcion != 6);
        }

        static void MostrarMenuEstacionamiento()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════╗");
            Console.WriteLine("║  EJERCICIO #1: ESTACIONAMIENTO - ING. SISTEMAS    ║");
            Console.WriteLine("╚════════════════════════════════════════════════════╝");
            Console.WriteLine("\n a) Agregar vehículo");
            Console.WriteLine(" b) Buscar vehículo por placa");
            Console.WriteLine(" c) Ver vehículos por año");
            Console.WriteLine(" d) Ver todos los vehículos registrados");
            Console.WriteLine(" e) Eliminar vehículo");
            Console.WriteLine(" 6. Volver al menú principal");
            Console.Write("\n Seleccione una opción (1-6): ");
        }

        static void AgregarVehiculoMenu()
        {
            Console.WriteLine("\n--- AGREGAR VEHÍCULO ---");
            
            Console.Write("Placa: ");
            string placa = Console.ReadLine();
            
            Console.Write("Marca: ");
            string marca = Console.ReadLine();
            
            Console.Write("Modelo: ");
            string modelo = Console.ReadLine();
            
            Console.Write("Año: ");
            int año = int.Parse(Console.ReadLine());
            
            Console.Write("Precio: ");
            decimal precio = decimal.Parse(Console.ReadLine());

            estacionamiento.AgregarVehiculo(placa, marca, modelo, año, precio);
        }

        static void BuscarVehiculoMenu()
        {
            Console.WriteLine("\n--- BUSCAR VEHÍCULO ---");
            Console.Write("Ingrese la placa a buscar: ");
            string placa = Console.ReadLine();
            estacionamiento.BuscarVehiculoPorPlaca(placa);
        }

        static void VerVehiculosPorAñoMenu()
        {
            Console.WriteLine("\n--- VER VEHÍCULOS POR AÑO ---");
            Console.Write("Ingrese el año: ");
            int año = int.Parse(Console.ReadLine());
            estacionamiento.VerVehiculosPorAño(año);
        }

        static void EliminarVehiculoMenu()
        {
            Console.WriteLine("\n--- ELIMINAR VEHÍCULO ---");
            Console.Write("Ingrese la placa del vehículo a eliminar: ");
            string placa = Console.ReadLine();
            estacionamiento.EliminarVehiculo(placa);
        }

        // =====================================================================
        // MENÚ EJERCICIO #2: REDES III
        // =====================================================================

        static void MenuRedesIII()
        {
            int opcion;

            do
            {
                MostrarMenuRedesIII();
                opcion = LeerOpcion();

                switch (opcion)
                {
                    case 1:
                        AgregarEstudianteMenu();
                        break;
                    case 2:
                        BuscarEstudianteMenu();
                        break;
                    case 3:
                        EliminarEstudianteMenu();
                        break;
                    case 4:
                        listaRedesIII.TotalEstudiantesAprobados();
                        break;
                    case 5:
                        listaRedesIII.TotalEstudiantesReprobados();
                        break;
                    case 6:
                        listaRedesIII.MostrarTodosLosEstudiantes();
                        break;
                    case 7:
                        Console.WriteLine("\nRegresando al menú principal...");
                        break;
                    default:
                        Console.WriteLine("\n✗ Opción inválida. Intente nuevamente.");
                        break;
                }

                if (opcion != 7)
                {
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                }

            } while (opcion != 7);
        }

        static void MostrarMenuRedesIII()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════╗");
            Console.WriteLine("║   EJERCICIO #2: SISTEMA DE REDES III           ║");
            Console.WriteLine("╚════════════════════════════════════════════════╝");
            Console.WriteLine("\n a) Agregar estudiante");
            Console.WriteLine(" b) Buscar estudiante por cédula");
            Console.WriteLine(" c) Eliminar estudiante");
            Console.WriteLine(" d) Total estudiantes aprobados");
            Console.WriteLine(" e) Total estudiantes reprobados");
            Console.WriteLine(" 6. Mostrar todos los estudiantes");
            Console.WriteLine(" 7. Volver al menú principal");
            Console.Write("\n Seleccione una opción (1-7): ");
        }

        static void AgregarEstudianteMenu()
        {
            Console.WriteLine("\n--- AGREGAR ESTUDIANTE ---");
            
            Console.Write("Cédula: ");
            string cedula = Console.ReadLine();
            
            Console.Write("Nombre: ");
            string nombre = Console.ReadLine();
            
            Console.Write("Apellido: ");
            string apellido = Console.ReadLine();
            
            Console.Write("Correo: ");
            string correo = Console.ReadLine();
            
            Console.Write("Nota Definitiva (1-10): ");
            double nota = double.Parse(Console.ReadLine());

            if (nota < 1 || nota > 10)
            {
                Console.WriteLine("\n✗ La nota debe estar entre 1 y 10.");
                return;
            }

            listaRedesIII.AgregarEstudiante(cedula, nombre, apellido, correo, nota);
        }

        static void BuscarEstudianteMenu()
        {
            Console.WriteLine("\n--- BUSCAR ESTUDIANTE ---");
            Console.Write("Ingrese la cédula a buscar: ");
            string cedula = Console.ReadLine();
            listaRedesIII.BuscarEstudiantePorCedula(cedula);
        }

        static void EliminarEstudianteMenu()
        {
            Console.WriteLine("\n--- ELIMINAR ESTUDIANTE ---");
            Console.Write("Ingrese la cédula del estudiante a eliminar: ");
            string cedula = Console.ReadLine();
            listaRedesIII.EliminarEstudiante(cedula);
        }

        // =====================================================================
        // UTILIDADES
        // =====================================================================

        static int LeerOpcion()
        {
            try
            {
                return int.Parse(Console.ReadLine());
            }
            catch
            {
                return -1;
            }
        }
    }
}