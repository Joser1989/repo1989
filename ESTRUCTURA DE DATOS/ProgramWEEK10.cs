using System;
using System.Collections.Generic;
using System.Linq;

namespace CampanaVacunacion
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // ================================================================
            // GENERACIÓN DE DATOS FICTICIOS
            // ================================================================

            // Conjunto Universal: 500 ciudadanos
            HashSet<string> ciudadanos = new HashSet<string>();
            for (int i = 1; i <= 500; i++)
                ciudadanos.Add($"Ciudadano {i}");

            // Conjunto A: 75 ciudadanos vacunados con Pfizer (Ciudadano 1 al 75)
            HashSet<string> vacunadosPfizer = new HashSet<string>();
            for (int i = 1; i <= 75; i++)
                vacunadosPfizer.Add($"Ciudadano {i}");

            // Conjunto B: 75 ciudadanos vacunados con AstraZeneca (Ciudadano 51 al 125)
            // Se genera solapamiento intencional (Ciudadano 51-75) para representar
            // ciudadanos que recibieron ambas dosis (una dosis de cada vacuna).
            HashSet<string> vacunadosAstraZeneca = new HashSet<string>();
            for (int i = 51; i <= 125; i++)
                vacunadosAstraZeneca.Add($"Ciudadano {i}");

            // ================================================================
            // OPERACIONES DE TEORÍA DE CONJUNTOS
            // ================================================================

            // 1. COMPLEMENTO: Ciudadanos NO vacunados
            //    NoVacunados = Universo - (Pfizer ∪ AstraZeneca)
            HashSet<string> todosVacunados = new HashSet<string>(vacunadosPfizer);
            todosVacunados.UnionWith(vacunadosAstraZeneca);          // Pfizer ∪ AstraZeneca

            HashSet<string> noVacunados = new HashSet<string>(ciudadanos);
            noVacunados.ExceptWith(todosVacunados);                  // Universo \ (A ∪ B)

            // 2. INTERSECCIÓN: Ciudadanos con AMBAS dosis
            //    AmbaDosis = Pfizer ∩ AstraZeneca
            HashSet<string> ambasDosis = new HashSet<string>(vacunadosPfizer);
            ambasDosis.IntersectWith(vacunadosAstraZeneca);          // A ∩ B

            // 3. DIFERENCIA: Solo Pfizer
            //    SoloPfizer = Pfizer - AstraZeneca  (A \ B)
            HashSet<string> soloPfizer = new HashSet<string>(vacunadosPfizer);
            soloPfizer.ExceptWith(vacunadosAstraZeneca);             // A \ B

            // 4. DIFERENCIA: Solo AstraZeneca
            //    SoloAstraZeneca = AstraZeneca - Pfizer  (B \ A)
            HashSet<string> soloAstraZeneca = new HashSet<string>(vacunadosAstraZeneca);
            soloAstraZeneca.ExceptWith(vacunadosPfizer);             // B \ A

            // ================================================================
            // PRESENTACIÓN DE RESULTADOS
            // ================================================================

            Separador('=', 65);
            Console.WriteLine("   MINISTERIO DE SALUD - CAMPAÑA DE VACUNACIÓN COVID-19");
            Separador('=', 65);

            Console.WriteLine($"\n  Universo total de ciudadanos   : {ciudadanos.Count}");
            Console.WriteLine($"  Vacunados con Pfizer (A)       : {vacunadosPfizer.Count}");
            Console.WriteLine($"  Vacunados con AstraZeneca (B)  : {vacunadosAstraZeneca.Count}");
            Console.WriteLine($"  Total vacunados (A ∪ B)        : {todosVacunados.Count}");

            // --- Listado 1: No vacunados ---
            Separador('-', 65);
            Console.WriteLine($"\n  LISTADO 1 — Ciudadanos NO vacunados");
            Console.WriteLine($"  Operación: Universo \\ (A ∪ B)   | Total: {noVacunados.Count}");
            Separador('-', 65);
            ImprimirConjunto(noVacunados);

            // --- Listado 2: Ambas dosis ---
            Separador('-', 65);
            Console.WriteLine($"\n  LISTADO 2 — Ciudadanos con AMBAS dosis");
            Console.WriteLine($"  Operación: A ∩ B               | Total: {ambasDosis.Count}");
            Separador('-', 65);
            ImprimirConjunto(ambasDosis);

            // --- Listado 3: Solo Pfizer ---
            Separador('-', 65);
            Console.WriteLine($"\n  LISTADO 3 — Ciudadanos vacunados SOLO con Pfizer");
            Console.WriteLine($"  Operación: A \\ B               | Total: {soloPfizer.Count}");
            Separador('-', 65);
            ImprimirConjunto(soloPfizer);

            // --- Listado 4: Solo AstraZeneca ---
            Separador('-', 65);
            Console.WriteLine($"\n  LISTADO 4 — Ciudadanos vacunados SOLO con AstraZeneca");
            Console.WriteLine($"  Operación: B \\ A               | Total: {soloAstraZeneca.Count}");
            Separador('-', 65);
            ImprimirConjunto(soloAstraZeneca);

            // --- Verificación de integridad ---
            Separador('=', 65);
            Console.WriteLine("\n  VERIFICACIÓN DE INTEGRIDAD");
            Separador('-', 65);
            int sumaParticiones = noVacunados.Count + ambasDosis.Count
                                  + soloPfizer.Count + soloAstraZeneca.Count;
            Console.WriteLine($"  NoVacunados + AmbasDosis + SoloPfizer + SoloAstraZeneca");
            Console.WriteLine($"  = {noVacunados.Count} + {ambasDosis.Count} + {soloPfizer.Count} + {soloAstraZeneca.Count} = {sumaParticiones}");
            Console.WriteLine($"  Universo total                 : {ciudadanos.Count}");
            Console.WriteLine($"  ¿Partición válida?             : {(sumaParticiones == ciudadanos.Count ? "✔ SÍ" : "✘ NO")}");
            Separador('=', 65);
        }

        // ----------------------------------------------------------------
        // Métodos auxiliares
        // ----------------------------------------------------------------

        static void ImprimirConjunto(HashSet<string> conjunto)
        {
            if (conjunto.Count == 0)
            {
                Console.WriteLine("  (conjunto vacío)");
                return;
            }

            // Ordenar numéricamente para mejor legibilidad
            var ordenados = conjunto
                .OrderBy(c => int.Parse(c.Split(' ')[1]))
                .ToList();

            // Imprimir en columnas de 4
            for (int i = 0; i < ordenados.Count; i++)
            {
                Console.Write($"  {ordenados[i],-18}");
                if ((i + 1) % 4 == 0) Console.WriteLine();
            }
            if (ordenados.Count % 4 != 0) Console.WriteLine();
        }

        static void Separador(char caracter, int longitud)
        {
            Console.WriteLine(new string(caracter, longitud));
        }
    }
}