using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Tecnicas_EaseSolutions
{
    internal class Program
    {   

        //AUTOR: JOAN SEBASTIAN JIMENEZ MENDIVELSO
        //CORREO: josebas981016@gmail.com
        //UBICACION: BOGOTA, COLOMBIA.
        //FECHA: 24/03/2023

        #region [Variables Estaticas]
        static int[,] datos;
        static bool[,] busqueda;
        static List<int> Resultado = new List<int>();
        static int maxDistancia = 0;
        static int maxCaida = 0;
        static int altMaxCaida = 0;
        static int inicioX, inicioY;
        #endregion

        #region [Main]
        static void Main(string[] args)
        {
            //ingresar ruta del archivo txt
            Console.WriteLine("Ingrese la ruta del archivo:");
            string archivo = Console.ReadLine();

            //busqueda del archivo
            StreamReader file = new StreamReader(archivo.ToString().Trim());

            //leer primera linea para saber tamaño
            string primeraLinea = file.ReadLine();
            int posicion = primeraLinea.IndexOf(" ");
            int n = Int32.Parse(primeraLinea.Substring(0, posicion));

            //leer todo el archivo
            string[] lineas = File.ReadAllLines(archivo);
            int filas = lineas.Length - 1;
            int columnas = lineas[1].Split(' ').Length;

            busqueda = new bool[n, n];
            datos = new int[filas, columnas];

            //Añadir al arreglo datos[] todos los valores como enteros
            for (int i = 1; i < lineas.Length; i++)
            {
                string[] valores = lineas[i].Split(' ');

                for (int j = 0; j < columnas; j++)
                {
                    datos[j, i - 1] = Int32.Parse(valores[j]);
                }
            }

            //Recorrer el arreglo comprobando direcciones
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    encontrarCaminoMasLargo(i, j, i, j, 1, datos[i, j]);
                }
            }

            //Imprimir la longitud y la caída de la ruta más larga y empinada encontrada
            Console.WriteLine("Longitud: " + maxDistancia);
            Console.WriteLine("Caída: " + maxCaida);
            Console.WriteLine("Posiciones de la bajada: ");
            Console.Write("{ ");
            foreach (int item in Resultado.OrderByDescending(x => x))
            {
                Console.Write($"{item} ");
            }
            Console.Write("}");
            Console.ReadLine(); 
        }
        #endregion

        #region [Encontrar Ruta Mas Larga]
        //Búsquedas con (DFS)
        static void encontrarCaminoMasLargo(int xInit, int yInit, int x, int y, int distancia, int alturaInicio)
        {
            busqueda[x, y] = true;
            int maxLength = datos.GetLength(0) - 1;
            //Comprobar las 4 direcciones posibles
            //Buscar izquierda
            if (x > 0 && !busqueda[x - 1, y] && datos[x - 1, y] < alturaInicio)
            {
                encontrarCaminoMasLargo(xInit, yInit, x - 1, y, distancia + 1, datos[x - 1, y]);
            }
            //Buscar arriba
            if (y > 0 && !busqueda[x, y - 1] && datos[x, y - 1] < alturaInicio)
            {
                encontrarCaminoMasLargo(xInit, yInit, x, y - 1, distancia + 1, datos[x, y - 1]);
            }
            //busca derecha
            if (x < maxLength && !busqueda[x + 1, y] && datos[x + 1, y] < alturaInicio)
            {
                encontrarCaminoMasLargo(xInit, yInit, x + 1, y, distancia + 1, datos[x + 1, y]);
            }
            //busca abajo
            if (y < maxLength && !busqueda[x, y + 1] && datos[x, y + 1] < alturaInicio)
            {
                encontrarCaminoMasLargo(xInit, yInit, x, y + 1, distancia + 1, datos[x, y + 1]);
            }

            // Actualizar la ruta más larga y empinada encontrada
            if (distancia > maxDistancia)
            {
                maxDistancia = distancia;
                maxCaida = datos[xInit, yInit] - datos[x, y];
                calcularResultados(x, y);
            }
            else if (distancia == maxDistancia && datos[xInit, yInit] - datos[x, y] > maxCaida)
            {
                maxCaida = datos[xInit, yInit] - datos[x, y];
                calcularResultados(x, y);
            }

            busqueda[x, y] = false;
        }

        #endregion

        #region [Calcular Resultados]

        private static void calcularResultados(int x, int y)
        {
            Resultado = new List<int>();
            for (int i = 0; i < datos.GetLength(0); i++)
            {
                for (int j = 0; j < datos.GetLength(0); j++)
                {
                    if (busqueda[i, j])
                    {
                        Resultado.Add(datos[i, j]);
                    }
                }
            }
            inicioX = x;
            inicioY = y;
        }

        #endregion

    }
}
