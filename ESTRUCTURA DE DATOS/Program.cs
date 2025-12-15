    // Programa para registrar informacion de estudiantes UEA 
using System;

namespace RegistroEstudiantes
{
    // Clase que representa a un estudiante con sus datos personales
    class Estudiante
    {
        // Propiedades públicas para acceder a los datos
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Direccion { get; set; }
        
        // Array para almacenar hasta 3 números telefónicos
        public string[] Telefonos { get; set; }

        // Constructor que inicializa el array de teléfonos
        public Estudiante()
        {
            Telefonos = new string[3];
        }

        // Método para mostrar la información completa del estudiante
        public void MostrarInformacion()
        {
            Console.WriteLine("\n===== INFORMACIÓN DEL ESTUDIANTE =====");
            Console.WriteLine($"ID: {Id}");
            Console.WriteLine($"Nombres: {Nombres}");
            Console.WriteLine($"Apellidos: {Apellidos}");
            Console.WriteLine($"Dirección: {Direccion}");
            
            Console.WriteLine("\nTeléfonos:");
            for (int i = 0; i < Telefonos.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(Telefonos[i]))
                {
                    Console.WriteLine($"  {i + 1}. {Telefonos[i]}");
                }
            }
            Console.WriteLine("=====================================");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== SISTEMA DE REGISTRO DE ESTUDIANTES ===");
            Console.WriteLine();

            // Crear una instancia del estudiante
            Estudiante estudiante = new Estudiante();

            // Solicitar ID del estudiante
            Console.Write("Ingrese el ID del estudiante: ");
            estudiante.Id = int.Parse(Console.ReadLine());

            // Solicitar nombres
            Console.Write("Ingrese los nombres: ");
            estudiante.Nombres = Console.ReadLine();

            // Solicitar apellidos
            Console.Write("Ingrese los apellidos: ");
            estudiante.Apellidos = Console.ReadLine();

            // Solicitar dirección
            Console.Write("Ingrese la dirección: ");
            estudiante.Direccion = Console.ReadLine();

            // Solicitar los tres números telefónicos usando un bucle
            Console.WriteLine("\nIngrese los números telefónicos:");
            for (int i = 0; i < 3; i++)
            {
                Console.Write($"Teléfono {i + 1}: ");
                estudiante.Telefonos[i] = Console.ReadLine();
            }

            // Mostrar la información completa del estudiante
            estudiante.MostrarInformacion();

            // Esperar a que el usuario presione una tecla
            Console.WriteLine("\nPresione cualquier tecla para salir...");
            Console.ReadKey();
        }
    }
}
