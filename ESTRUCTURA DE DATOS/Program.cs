using System;
using System.Collections.Generic;
using System.Linq;

#pragma warning disable IDE0130 // El espacio de nombres no coincide con la estructura de carpetas
namespace EjerciciosListasTuplasTareaSemana05
#pragma warning restore IDE0130 // El espacio de nombres no coincide con la estructura de carpetas
{
    class Program
    {
        static void Main(string[] args)
        {
            bool continuar = true;
            
            while (continuar)
            {
                Console.Clear();
                Console.WriteLine("=== EJERCICIOS DE LISTAS Y TUPLAS ===\n");
                Console.WriteLine("1. Mostrar asignaturas");
                Console.WriteLine("2. Mostrar 'Yo estudio <asignatura>'");
                Console.WriteLine("3. Almacenar y mostrar notas");
                Console.WriteLine("4. Números de lotería ordenados");
                Console.WriteLine("5. Números del 1 al 10 en orden inverso");
                Console.WriteLine("0. Salir\n");
                Console.Write("Selecciona un ejercicio: ");
                
                string opcion = Console.ReadLine();
                Console.WriteLine();
                
                switch (opcion)
                {
                    case "1":
                        Ejercicio1();
                        break;
                    case "2":
                        Ejercicio2();
                        break;
                    case "3":
                        Ejercicio3();
                        break;
                    case "4":
                        Ejercicio4();
                        break;
                    case "5":
                        Ejercicio5();
                        break;
                    case "0":
                        continuar = false;
                        Console.WriteLine("¡Hasta luego!");
                        break;
                    default:
                        Console.WriteLine("Opción no válida");
                        break;
                }
                
                if (continuar && opcion != "0")
                {
                    Console.WriteLine("\nPresiona cualquier tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }
        
        // Ejercicio 1: Mostrar asignaturas
        static void Ejercicio1()
        {
            Console.WriteLine("--- EJERCICIO 1 ---");
            List<string> asignaturas = new List<string> 
            { 
                "Estructura de Datos", 
                "Metodologías de la investigación", 
                "Fundamentos de Sistemas Digitales", 
                "Administracion de Sistemas Operativos", 
                "Instalaciones Electricas y de Cableado Estructurado" 
            };
            
            Console.WriteLine("Asignaturas del curso:");
            foreach (string asignatura in asignaturas)
            {
                Console.WriteLine(asignatura);
            }
        }
        
        // Ejercicio 2: Mostrar "Yo estudio <asignatura>"
        static void Ejercicio2()
        {
            Console.WriteLine("--- EJERCICIO 2 ---");
            List<string> asignaturas = new List<string> 
            { 
                "Matemáticas", 
                "Física", 
                "Química", 
                "Historia", 
                "Lengua" 
            };
            
            foreach (string asignatura in asignaturas)
            {
                Console.WriteLine($"Yo estudio {asignatura}");
            }
        }
        
        // Ejercicio 3: Almacenar y mostrar notas
        static void Ejercicio3()
        {
            Console.WriteLine("--- EJERCICIO 3 ---");
            List<string> asignaturas = new List<string> 
            { 
                "Matemáticas", 
                "Física", 
                "Química", 
                "Historia", 
                "Lengua" 
            };
            
            Dictionary<string, double> notas = new Dictionary<string, double>();
            
            foreach (string asignatura in asignaturas)
            {
                Console.Write($"¿Qué nota has sacado en {asignatura}? ");
                double nota = Convert.ToDouble(Console.ReadLine());
                notas[asignatura] = nota;
            }
            
            Console.WriteLine("\n--- RESULTADOS ---");
            foreach (var item in notas)
            {
                Console.WriteLine($"En {item.Key} has sacado {item.Value}");
            }
        }
        
        // Ejercicio 4: Números de lotería ordenados
        static void Ejercicio4()
        {
            Console.WriteLine("--- EJERCICIO 4 ---");
            List<int> numerosLoteria = new List<int>();
            
            Console.WriteLine("Introduce los 6 números ganadores de la lotería primitiva:");
            for (int i = 1; i <= 6; i++)
            {
                Console.Write($"Número {i}: ");
                int numero = Convert.ToInt32(Console.ReadLine());
                numerosLoteria.Add(numero);
            }
            
            numerosLoteria.Sort();
            
            Console.WriteLine("\nNúmeros ganadores ordenados:");
            foreach (int numero in numerosLoteria)
            {
                Console.WriteLine(numero);
            }
        }
        
        // Ejercicio 5: Números del 1 al 10 en orden inverso
        static void Ejercicio5()
        {
            Console.WriteLine("--- EJERCICIO 5 ---");
            List<int> numeros = new List<int>();
            
            for (int i = 1; i <= 10; i++)
            {
                numeros.Add(i);
            }
            
            numeros.Reverse();
            
            Console.WriteLine("Números del 1 al 10 en orden inverso:");
            Console.WriteLine(string.Join(", ", numeros));
        }
    }
}