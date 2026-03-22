using System;

// ============================================================
//  CLASE NODO
// ============================================================
class Nodo
{
    public int Valor;
    public Nodo? Izquierdo;
    public Nodo? Derecho;

    public Nodo(int valor)
    {
        Valor = valor;
        Izquierdo = null;
        Derecho = null;
    }
}

// ============================================================
//  CLASE BST (Árbol Binario de Búsqueda)
// ============================================================
class BST
{
    private Nodo? raiz;

    // ─── INSERTAR ───────────────────────────────────────────
    public void Insertar(int valor)
    {
        raiz = InsertarRec(raiz, valor);
    }

    private Nodo InsertarRec(Nodo? nodo, int valor)
    {
        if (nodo == null)
            return new Nodo(valor);

        if (valor < nodo.Valor)
            nodo.Izquierdo = InsertarRec(nodo.Izquierdo, valor);
        else if (valor > nodo.Valor)
            nodo.Derecho = InsertarRec(nodo.Derecho, valor);
        else
            Console.WriteLine($"  [!] El valor {valor} ya existe en el árbol.");

        return nodo;
    }

    // ─── BUSCAR ─────────────────────────────────────────────
    public bool Buscar(int valor)
    {
        return BuscarRec(raiz, valor);
    }

    private bool BuscarRec(Nodo? nodo, int valor)
    {
        if (nodo == null) return false;
        if (valor == nodo.Valor) return true;
        return valor < nodo.Valor
            ? BuscarRec(nodo.Izquierdo, valor)
            : BuscarRec(nodo.Derecho, valor);
    }

    // ─── ELIMINAR ───────────────────────────────────────────
    public void Eliminar(int valor)
    {
        if (!Buscar(valor))
        {
            Console.WriteLine($"  [!] El valor {valor} no existe en el árbol.");
            return;
        }
        raiz = EliminarRec(raiz, valor);
        Console.WriteLine($"  [✓] Valor {valor} eliminado correctamente.");
    }

    private Nodo? EliminarRec(Nodo? nodo, int valor)
    {
        if (nodo == null) return null;

        if (valor < nodo.Valor)
            nodo.Izquierdo = EliminarRec(nodo.Izquierdo, valor);
        else if (valor > nodo.Valor)
            nodo.Derecho = EliminarRec(nodo.Derecho, valor);
        else
        {
            // Caso 1: hoja o un hijo
            if (nodo.Izquierdo == null) return nodo.Derecho;
            if (nodo.Derecho == null)   return nodo.Izquierdo;

            // Caso 2: dos hijos → sucesor inorden (mínimo del subárbol derecho)
            Nodo sucesor = MinNodo(nodo.Derecho);
            nodo.Valor = sucesor.Valor;
            nodo.Derecho = EliminarRec(nodo.Derecho, sucesor.Valor);
        }
        return nodo;
    }

    // ─── RECORRIDOS ─────────────────────────────────────────
    public void Preorden()
    {
        if (EstaVacio()) { MsgVacio(); return; }
        Console.Write("  Preorden  (Raíz-Izq-Der): ");
        PreordenRec(raiz);
        Console.WriteLine();
    }

    private void PreordenRec(Nodo? nodo)
    {
        if (nodo == null) return;
        Console.Write($"{nodo.Valor} ");
        PreordenRec(nodo.Izquierdo);
        PreordenRec(nodo.Derecho);
    }

    public void Inorden()
    {
        if (EstaVacio()) { MsgVacio(); return; }
        Console.Write("  Inorden   (Izq-Raíz-Der): ");
        InordenRec(raiz);
        Console.WriteLine();
    }

    private void InordenRec(Nodo? nodo)
    {
        if (nodo == null) return;
        InordenRec(nodo.Izquierdo);
        Console.Write($"{nodo.Valor} ");
        InordenRec(nodo.Derecho);
    }

    public void Postorden()
    {
        if (EstaVacio()) { MsgVacio(); return; }
        Console.Write("  Postorden (Izq-Der-Raíz): ");
        PostordenRec(raiz);
        Console.WriteLine();
    }

    private void PostordenRec(Nodo? nodo)
    {
        if (nodo == null) return;
        PostordenRec(nodo.Izquierdo);
        PostordenRec(nodo.Derecho);
        Console.Write($"{nodo.Valor} ");
    }

    // ─── MÍNIMO ─────────────────────────────────────────────
    public void MostrarMinimo()
    {
        if (EstaVacio()) { MsgVacio(); return; }
        Console.WriteLine($"  Valor mínimo: {MinNodo(raiz!).Valor}");
    }

    private Nodo MinNodo(Nodo nodo)
    {
        while (nodo.Izquierdo != null)
            nodo = nodo.Izquierdo;
        return nodo;
    }

    // ─── MÁXIMO ─────────────────────────────────────────────
    public void MostrarMaximo()
    {
        if (EstaVacio()) { MsgVacio(); return; }
        Nodo actual = raiz!;
        while (actual.Derecho != null)
            actual = actual.Derecho;
        Console.WriteLine($"  Valor máximo: {actual.Valor}");
    }

    // ─── ALTURA ─────────────────────────────────────────────
    public void MostrarAltura()
    {
        if (EstaVacio()) { MsgVacio(); return; }
        Console.WriteLine($"  Altura del árbol: {AlturaRec(raiz)}");
    }

    private int AlturaRec(Nodo? nodo)
    {
        if (nodo == null) return 0;
        return 1 + Math.Max(AlturaRec(nodo.Izquierdo), AlturaRec(nodo.Derecho));
    }

    // ─── LIMPIAR ────────────────────────────────────────────
    public void Limpiar()
    {
        raiz = null;
        Console.WriteLine("  [✓] Árbol limpiado completamente.");
    }

    // ─── UTILIDADES ─────────────────────────────────────────
    public bool EstaVacio() => raiz == null;

    private void MsgVacio() =>
        Console.WriteLine("  [!] El árbol está vacío.");
}

// ============================================================
//  PROGRAMA PRINCIPAL — MENÚ INTERACTIVO
// ============================================================
class Program
{
    static BST arbol = new BST();

    static void Main()
    {
        bool salir = false;

        while (!salir)
        {
            MostrarMenu();
            string? opcion = Console.ReadLine()?.Trim();

            Console.WriteLine();
            switch (opcion)
            {
                case "1": OpInsertar();   break;
                case "2": OpBuscar();     break;
                case "3": OpEliminar();   break;
                case "4": OpRecorridos(); break;
                case "5": OpEstadisticas(); break;
                case "6": OpLimpiar();    break;
                case "0":
                    Console.WriteLine("  Saliendo... ¡Hasta luego!\n");
                    salir = true;
                    break;
                default:
                    Console.WriteLine("  [!] Opción inválida. Intente de nuevo.");
                    break;
            }

            if (!salir)
            {
                Console.WriteLine("\n  Presione cualquier tecla para continuar...");
                Console.ReadKey();
            }
        }
    }

    // ─── MENÚ ───────────────────────────────────────────────
    static void MostrarMenu()
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════════════╗");
        Console.WriteLine("║    ÁRBOL BINARIO DE BÚSQUEDA (BST) C#    ║");
        Console.WriteLine("╠══════════════════════════════════════════╣");
        Console.WriteLine("║  1. Insertar valor                       ║");
        Console.WriteLine("║  2. Buscar valor                         ║");
        Console.WriteLine("║  3. Eliminar valor                       ║");
        Console.WriteLine("║  4. Mostrar recorridos                   ║");
        Console.WriteLine("║  5. Mínimo, Máximo y Altura              ║");
        Console.WriteLine("║  6. Limpiar árbol                        ║");
        Console.WriteLine("║  0. Salir                                ║");
        Console.WriteLine("╚══════════════════════════════════════════╝");
        Console.Write("  Seleccione una opción: ");
    }

    // ─── OPERACIONES ────────────────────────────────────────
    static void OpInsertar()
    {
        Console.Write("  Ingrese el valor a insertar: ");
        if (int.TryParse(Console.ReadLine(), out int val))
        {
            arbol.Insertar(val);
            Console.WriteLine($"  [✓] Valor {val} insertado.");
        }
        else
            Console.WriteLine("  [!] Valor inválido. Ingrese un número entero.");
    }

    static void OpBuscar()
    {
        Console.Write("  Ingrese el valor a buscar: ");
        if (int.TryParse(Console.ReadLine(), out int val))
        {
            bool encontrado = arbol.Buscar(val);
            Console.WriteLine(encontrado
                ? $"  [✓] El valor {val} SÍ existe en el árbol."
                : $"  [✗] El valor {val} NO existe en el árbol.");
        }
        else
            Console.WriteLine("  [!] Valor inválido.");
    }

    static void OpEliminar()
    {
        Console.Write("  Ingrese el valor a eliminar: ");
        if (int.TryParse(Console.ReadLine(), out int val))
            arbol.Eliminar(val);
        else
            Console.WriteLine("  [!] Valor inválido.");
    }

    static void OpRecorridos()
    {
        Console.WriteLine("  ── Recorridos del árbol ──");
        arbol.Preorden();
        arbol.Inorden();
        arbol.Postorden();
    }

    static void OpEstadisticas()
    {
        Console.WriteLine("  ── Estadísticas del árbol ──");
        arbol.MostrarMinimo();
        arbol.MostrarMaximo();
        arbol.MostrarAltura();
    }

    static void OpLimpiar()
    {
        Console.Write("  ¿Está seguro que desea limpiar el árbol? (s/n): ");
        if (Console.ReadLine()?.Trim().ToLower() == "s")
            arbol.Limpiar();
        else
            Console.WriteLine("  Operación cancelada.");
    }
}