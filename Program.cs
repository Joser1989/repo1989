using System;

namespace FigurasGeometricas
{
    // Clase que representa un círculo y encapsula sus propiedades
    public class Circulo
    {
        // Campo privado para almacenar el radio del círculo
        private double radio;

        // Constructor que inicializa el círculo con un radio específico
        // Valida que el radio sea un valor positivo
        public Circulo(double radioInicial)
        {
            if (radioInicial <= 0)
            {
                throw new ArgumentException("El radio debe ser mayor a cero");
            }
            radio = radioInicial;
        }

        // Propiedad pública para acceder y modificar el radio de forma controlada
        public double Radio
        {
            get { return radio; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("El radio debe ser mayor a cero");
                }
                radio = value;
            }
        }

        // Método que calcula el área del círculo
        // Fórmula: π * radio²
        // Retorna un valor de tipo double con el resultado
        public double CalcularArea()
        {
            return Math.PI * radio * radio;
        }

        // Método que calcula el perímetro (circunferencia) del círculo
        // Fórmula: 2 * π * radio
        // Retorna un valor de tipo double con el resultado
        public double CalcularPerimetro()
        {
            return 2 * Math.PI * radio;
        }

        // Método que retorna información del círculo en formato de texto
        public string ObtenerInformacion()
        {
            return $"Círculo - Radio: {radio:F2}, Área: {CalcularArea():F2}, Perímetro: {CalcularPerimetro():F2}";
        }
    }

    // Clase que representa un rectángulo y encapsula sus dimensiones
    public class Rectangulo
    {
        // Campos privados para almacenar la base y altura del rectángulo
        private double baseRectangulo;
        private double altura;

        // Constructor que inicializa el rectángulo con base y altura específicas
        // Valida que ambos valores sean positivos
        public Rectangulo(double baseInicial, double alturaInicial)
        {
            if (baseInicial <= 0 || alturaInicial <= 0)
            {
                throw new ArgumentException("La base y altura deben ser mayores a cero");
            }
            baseRectangulo = baseInicial;
            altura = alturaInicial;
        }

        // Propiedad pública para acceder y modificar la base de forma controlada
        public double Base
        {
            get { return baseRectangulo; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("La base debe ser mayor a cero");
                }
                baseRectangulo = value;
            }
        }

        // Propiedad pública para acceder y modificar la altura de forma controlada
        public double Altura
        {
            get { return altura; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("La altura debe ser mayor a cero");
                }
                altura = value;
            }
        }

        // Método que calcula el área del rectángulo
        // Fórmula: base * altura
        // Retorna un valor de tipo double con el resultado
        public double CalcularArea()
        {
            return baseRectangulo * altura;
        }

        // Método que calcula el perímetro del rectángulo
        // Fórmula: 2 * (base + altura)
        // Retorna un valor de tipo double con el resultado
        public double CalcularPerimetro()
        {
            return 2 * (baseRectangulo + altura);
        }

        // Método que retorna información del rectángulo en formato de texto
        public string ObtenerInformacion()
        {
            return $"Rectángulo - Base: {baseRectangulo:F2}, Altura: {altura:F2}, Área: {CalcularArea():F2}, Perímetro: {CalcularPerimetro():F2}";
        }
    }

    // Clase principal para demostrar el uso de las figuras geométricas
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== DEMOSTRACIÓN DE FIGURAS GEOMÉTRICAS ===\n");

            // Crear una instancia de Círculo con radio 5
            Circulo miCirculo = new Circulo(5.0);
            Console.WriteLine(miCirculo.ObtenerInformacion());
            
            // Modificar el radio y mostrar nuevos cálculos
            miCirculo.Radio = 7.5;
            Console.WriteLine("Después de modificar el radio:");
            Console.WriteLine(miCirculo.ObtenerInformacion());

            Console.WriteLine("\n-----------------------------------\n");

            // Crear una instancia de Rectángulo con base 8 y altura 4
            Rectangulo miRectangulo = new Rectangulo(8.0, 4.0);
            Console.WriteLine(miRectangulo.ObtenerInformacion());
            
            // Modificar dimensiones y mostrar nuevos cálculos
            miRectangulo.Base = 10.0;
            miRectangulo.Altura = 6.0;
            Console.WriteLine("Después de modificar las dimensiones:");
            Console.WriteLine(miRectangulo.ObtenerInformacion());

            Console.WriteLine("\n=== FIN DE LA DEMOSTRACIÓN ===");
            Console.ReadKey();
        }
    }
}