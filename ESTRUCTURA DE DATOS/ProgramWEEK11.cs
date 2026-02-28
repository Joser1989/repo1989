using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

class Traductor
{
    // Diccionario bidireccional: inglés -> español y español -> inglés
    static Dictionary<string, string> enEs = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        { "time",       "tiempo"    },
        { "person",     "persona"   },
        { "year",       "año"       },
        { "way",        "camino"    },
        { "day",        "día"       },
        { "thing",      "cosa"      },
        { "man",        "hombre"    },
        { "world",      "mundo"     },
        { "life",       "vida"      },
        { "hand",       "mano"      },
        { "part",       "parte"     },
        { "child",      "niño"      },
        { "eye",        "ojo"       },
        { "woman",      "mujer"     },
        { "place",      "lugar"     },
        { "work",       "trabajo"   },
        { "week",       "semana"    },
        { "case",       "caso"      },
        { "point",      "punto"     },
        { "government", "gobierno"  },
        { "company",    "empresa"   }
    };

    static Dictionary<string, string> esEn = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        { "tiempo",   "time"       },
        { "persona",  "person"     },
        { "año",      "year"       },
        { "camino",   "way"        },
        { "día",      "day"        },
        { "cosa",     "thing"      },
        { "hombre",   "man"        },
        { "mundo",    "world"      },
        { "vida",     "life"       },
        { "mano",     "hand"       },
        { "parte",    "part"       },
        { "niño",     "child"      },
        { "ojo",      "eye"        },
        { "mujer",    "woman"      },
        { "lugar",    "place"      },
        { "trabajo",  "work"       },
        { "semana",   "week"       },
        { "caso",     "case"       },
        { "punto",    "point"      },
        { "gobierno", "government" },
        { "empresa",  "company"    }
    };

    // ---------------------------------------------------------------
    // Traduce una frase preservando puntuación y mayúsculas/minúsculas
    // ---------------------------------------------------------------
    static string TraducirFrase(string frase, Dictionary<string, string> diccionario)
    {
        // Separamos los tokens: palabras y no-palabras (espacios, signos, etc.)
        string[] tokens = Regex.Split(frase, @"(\W+)");
        StringBuilder sb = new StringBuilder();

        foreach (string token in tokens)
        {
            if (diccionario.TryGetValue(token.Trim(), out string traduccion))
            {
                // Preservar la capitalización original
                if (token.Length > 0 && char.IsUpper(token[0]))
                    traduccion = char.ToUpper(traduccion[0]) + traduccion.Substring(1);

                sb.Append(traduccion);
            }
            else
            {
                sb.Append(token);
            }
        }

        return sb.ToString();
    }

    // ---------------------------------------------------------------
    // Menú principal
    // ---------------------------------------------------------------
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding  = Encoding.UTF8;

        int opcion;

        do
        {
            Console.WriteLine();
            Console.WriteLine("==================== MENÚ ====================");
            Console.WriteLine("1. Traducir una frase");
            Console.WriteLine("2. Agregar palabras al diccionario");
            Console.WriteLine("0. Salir");
            Console.Write("Seleccione una opción: ");

            string entrada = Console.ReadLine();

            if (!int.TryParse(entrada, out opcion))
            {
                Console.WriteLine("⚠  Opción no válida. Intente de nuevo.");
                continue;
            }

            switch (opcion)
            {
                // ── 1. TRADUCIR ─────────────────────────────────────
                case 1:
                    Console.WriteLine();
                    Console.WriteLine("Seleccione dirección de traducción:");
                    Console.WriteLine("  1. Inglés  → Español");
                    Console.WriteLine("  2. Español → Inglés");
                    Console.Write("Opción: ");
                    string dir = Console.ReadLine();

                    Dictionary<string, string> dicActivo;
                    string idiomaOrigen, idiomaDestino;

                    if (dir == "2")
                    {
                        dicActivo      = esEn;
                        idiomaOrigen   = "Español";
                        idiomaDestino  = "Inglés";
                    }
                    else
                    {
                        dicActivo      = enEs;
                        idiomaOrigen   = "Inglés";
                        idiomaDestino  = "Español";
                    }

                    Console.Write($"\nEscriba la frase en {idiomaOrigen}: ");
                    string frase = Console.ReadLine();

                    string resultado = TraducirFrase(frase, dicActivo);
                    Console.WriteLine($"\n✔ Traducción ({idiomaDestino}): {resultado}");
                    break;

                // ── 2. AGREGAR PALABRA ───────────────────────────────
                case 2:
                    Console.WriteLine();
                    Console.Write("Palabra en inglés  : ");
                    string palabraEn = Console.ReadLine()?.Trim();

                    Console.Write("Equivalente en español: ");
                    string palabraEs = Console.ReadLine()?.Trim();

                    if (string.IsNullOrEmpty(palabraEn) || string.IsNullOrEmpty(palabraEs))
                    {
                        Console.WriteLine("⚠  Las palabras no pueden estar vacías.");
                        break;
                    }

                    // Agregar / actualizar en ambas direcciones
                    enEs[palabraEn] = palabraEs;
                    esEn[palabraEs] = palabraEn;

                    Console.WriteLine($"✔ Agregado: \"{palabraEn}\" ↔ \"{palabraEs}\"");
                    break;

                // ── 0. SALIR ─────────────────────────────────────────
                case 0:
                    Console.WriteLine("\nHasta luego. / Goodbye!");
                    break;

                default:
                    Console.WriteLine("⚠  Opción no válida. Intente de nuevo.");
                    break;
            }

        } while (opcion != 0);
    }
}