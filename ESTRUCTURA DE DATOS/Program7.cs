using System;
using System.Collections.Generic;

namespace EjerciciosPilas
{
    /// <summary>
    /// Clase principal que contiene el menú de opciones
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            bool continuar = true;

            while (continuar)
            {
                Console.Clear();
                Console.WriteLine("╔═══════════════════════════════════╗");
                Console.WriteLine("║   EJERCICIOS DE PILAS (STACKS)    ║");
                Console.WriteLine("╚═══════════════════════════════════╝\n");
                Console.WriteLine("Seleccione una opción:");
                Console.WriteLine("1. Verificar paréntesis balanceados");
                Console.WriteLine("2. Torres de Hanoi");
                Console.WriteLine("3. Salir\n");
                Console.Write("Opción: ");

                string opcion = Console.ReadLine();

                Console.WriteLine();

                switch (opcion)
                {
                    case "1":
                        VerificadorExpresiones.EjecutarPrueba();
                        break;
                    case "2":
                        TorresHanoi.EjecutarDemo();
                        break;
                    case "3":
                        continuar = false;
                        Console.WriteLine("¡Hasta luego!");
                        continue;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }

                if (continuar)
                {
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                }
            }
        }
    }

    /// <summary>
    /// Clase para verificar el balanceo de paréntesis, llaves y corchetes
    /// </summary>
    class VerificadorExpresiones
    {
        /// <summary>
        /// Verifica si una expresión matemática tiene sus símbolos de agrupación balanceados
        /// </summary>
        /// <param name="expresion">La expresión matemática a verificar</param>
        /// <returns>true si está balanceada, false en caso contrario</returns>
        public static bool EstaBalanceada(string expresion)
        {
            // Creamos una pila para almacenar los símbolos de apertura
            Stack<char> pila = new Stack<char>();

            // Recorremos cada carácter de la expresión
            foreach (char caracter in expresion)
            {
                // Si encontramos un símbolo de apertura, lo agregamos a la pila
                if (caracter == '(' || caracter == '{' || caracter == '[')
                {
                    pila.Push(caracter);
                }
                // Si encontramos un símbolo de cierre, verificamos que coincida
                else if (caracter == ')' || caracter == '}' || caracter == ']')
                {
                    // Si la pila está vacía, no hay apertura correspondiente
                    if (pila.Count == 0)
                    {
                        return false;
                    }

                    // Extraemos el último símbolo de apertura
                    char ultimoAbierto = pila.Pop();

                    // Verificamos que el cierre corresponda con la apertura
                    if (!CoincidenSimbolos(ultimoAbierto, caracter))
                    {
                        return false;
                    }
                }
            }

            // La expresión está balanceada solo si la pila quedó vacía
            return pila.Count == 0;
        }

        /// <summary>
        /// Verifica si un símbolo de apertura coincide con su cierre
        /// </summary>
        private static bool CoincidenSimbolos(char apertura, char cierre)
        {
            return (apertura == '(' && cierre == ')') ||
                   (apertura == '{' && cierre == '}') ||
                   (apertura == '[' && cierre == ']');
        }

        /// <summary>
        /// Ejecuta pruebas del verificador de expresiones
        /// </summary>
        public static void EjecutarPrueba()
        {
            Console.WriteLine("=== VERIFICADOR DE PARÉNTESIS BALANCEADOS ===\n");

            // Caso de prueba proporcionado
            string expresion = "{7 + (8 * 5) - [(9 - 7) + (4 + 1)]}";
            
            Console.WriteLine($"Expresión: {expresion}");
            
            if (EstaBalanceada(expresion))
            {
                Console.WriteLine("Resultado: Fórmula balanceada\n");
            }
            else
            {
                Console.WriteLine("Resultado: Fórmula NO balanceada\n");
            }

            // Casos adicionales de prueba
            string[] casosPrueba = {
                "(2 + 3) * 5",
                "((1 + 2)",
                "{[()]}", 
                "{[(])}"
            };

            Console.WriteLine("Pruebas adicionales:");
            foreach (string caso in casosPrueba)
            {
                bool resultado = EstaBalanceada(caso);
                Console.WriteLine($"{caso} -> {(resultado ? "Balanceada" : "NO balanceada")}");
            }
        }
    }

    /// <summary>
    /// Clase que implementa la solución del problema de las Torres de Hanoi
    /// </summary>
    class TorresHanoi
    {
        // Representamos cada torre como una pila de enteros (cada número representa un disco)
        private Stack<int> torreA;
        private Stack<int> torreB;
        private Stack<int> torreC;
        private int contadorMovimientos;

        /// <summary>
        /// Constructor que inicializa las tres torres
        /// </summary>
        /// <param name="numeroDiscos">Cantidad de discos a utilizar</param>
        public TorresHanoi(int numeroDiscos)
        {
            torreA = new Stack<int>();
            torreB = new Stack<int>();
            torreC = new Stack<int>();
            contadorMovimientos = 0;

            // Colocamos los discos en la torre A (del más grande al más pequeño)
            // El disco 1 es el más pequeño, el disco n es el más grande
            for (int i = numeroDiscos; i >= 1; i--)
            {
                torreA.Push(i);
            }
        }

        /// <summary>
        /// Resuelve el problema de las Torres de Hanoi de forma recursiva
        /// </summary>
        /// <param name="n">Número de discos a mover</param>
        /// <param name="origen">Torre de origen</param>
        /// <param name="destino">Torre de destino</param>
        /// <param name="auxiliar">Torre auxiliar</param>
        public void Resolver(int n, char origen, char destino, char auxiliar)
        {
            // Caso base: si solo hay un disco, lo movemos directamente
            if (n == 1)
            {
                MoverDisco(origen, destino);
                return;
            }

            // Paso 1: Mover n-1 discos desde origen hasta auxiliar usando destino
            Resolver(n - 1, origen, auxiliar, destino);

            // Paso 2: Mover el disco más grande desde origen hasta destino
            MoverDisco(origen, destino);

            // Paso 3: Mover los n-1 discos desde auxiliar hasta destino usando origen
            Resolver(n - 1, auxiliar, destino, origen);
        }

        /// <summary>
        /// Realiza el movimiento físico de un disco entre dos torres
        /// </summary>
        private void MoverDisco(char desde, char hacia)
        {
            // Obtenemos las pilas correspondientes a cada torre
            Stack<int> pilaOrigen = ObtenerTorre(desde);
            Stack<int> pilaDestino = ObtenerTorre(hacia);

            // Extraemos el disco de la torre origen
            int disco = pilaOrigen.Pop();

            // Lo colocamos en la torre destino
            pilaDestino.Push(disco);

            // Incrementamos el contador e imprimimos el movimiento
            contadorMovimientos++;
            Console.WriteLine($"Movimiento {contadorMovimientos}: Mover disco {disco} de Torre {desde} a Torre {hacia}");
            
            // Mostramos el estado actual de las torres
            MostrarEstado();
        }

        /// <summary>
        /// Retorna la pila correspondiente a una torre específica
        /// </summary>
        private Stack<int> ObtenerTorre(char nombreTorre)
        {
            switch (nombreTorre)
            {
                case 'A': return torreA;
                case 'B': return torreB;
                case 'C': return torreC;
                default: throw new ArgumentException("Torre no válida");
            }
        }

        /// <summary>
        /// Muestra el estado actual de las tres torres
        /// </summary>
        private void MostrarEstado()
        {
            Console.WriteLine($"Torre A: {MostrarPila(torreA)}");
            Console.WriteLine($"Torre B: {MostrarPila(torreB)}");
            Console.WriteLine($"Torre C: {MostrarPila(torreC)}");
            Console.WriteLine(new string('-', 40));
        }

        /// <summary>
        /// Convierte una pila a string para visualización
        /// </summary>
        private string MostrarPila(Stack<int> pila)
        {
            if (pila.Count == 0)
                return "vacía";

            // Convertimos la pila a array para mostrarla sin modificarla
            int[] discos = pila.ToArray();
            Array.Reverse(discos); // Invertimos para mostrar de abajo hacia arriba
            return string.Join(", ", discos);
        }

        /// <summary>
        /// Método estático para ejecutar una demostración completa
        /// </summary>
        public static void EjecutarDemo()
        {
            Console.WriteLine("=== TORRES DE HANOI ===\n");
            Console.Write("Ingrese el número de discos: ");
            
            int numeroDiscos;
            while (!int.TryParse(Console.ReadLine(), out numeroDiscos) || numeroDiscos < 1)
            {
                Console.Write("Por favor ingrese un número válido mayor a 0: ");
            }

            Console.WriteLine($"\nResolviendo Torres de Hanoi con {numeroDiscos} disco(s)...\n");
            Console.WriteLine("Estado inicial:");
            
            TorresHanoi juego = new TorresHanoi(numeroDiscos);
            juego.MostrarEstado();
            
            Console.WriteLine("\nIniciando solución...\n");
            
            // Resolver: mover todos los discos de Torre A a Torre C usando Torre B como auxiliar
            juego.Resolver(numeroDiscos, 'A', 'C', 'B');
            
            Console.WriteLine($"\n¡Problema resuelto en {juego.contadorMovimientos} movimientos!");
            Console.WriteLine($"Número mínimo de movimientos: {Math.Pow(2, numeroDiscos) - 1}");
        }
    }
}