﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetOblProj1
{
    class Program
    {
        private static string postacNewtona;
        private static Nawias postacOgolna;
        private static List<double> x = new List<double>();
        private static List<double> y = new List<double>();
        private static double[] b;

        public static void Main()
        {
            int n = (-1);

            while (true)
            {
                Console.Write("Podaj liczbę węzłów n (n >= 1): ");
                try
                {
                    n = Convert.ToInt32(Console.ReadLine());
                }
                catch (FormatException e)
                {
                }

                if (n <= 0)
                    Console.WriteLine("Błędna liczba węzłów, spróbuj ponownie.");
                else
                    break;
            }

            podajXY(n);

            while (true)
            {
                obliczB();
                utworzNewtona();
                Console.WriteLine("Postać Newtona:\n" + postacNewtona);
                utworzOgolna();
                Console.WriteLine("Postać ogólna:\n" + postacOgolna);

                // Dodawanie nowego wezla
                Console.Write("Czy dodać kolejny węzeł? (T/N): ");

                char wybor = Console.ReadKey().KeyChar;
                if ((wybor & 0xDF) == 'T')
                    podajXY(1);
                else
                    break;
            }
        }


        public static void podajXY(int n)
        {
            int indeksStartowy = x.Count;

            Console.WriteLine();

            for (int i = 0; i < n; i++)
            {
                while (true)
                {
                    try
                    {
                        Console.Write("Podaj x{0}: ", i + indeksStartowy);
                        double wezelX = Convert.ToDouble(Console.ReadLine());

                        Console.Write("Podaj y{0}: ", i + indeksStartowy);
                        double wartoscY = Convert.ToDouble(Console.ReadLine());

                        if (x.IndexOf(wezelX) == (-1))
                        {
                            int punktWstawiania = 0;

                            for (; punktWstawiania < x.Count; punktWstawiania++)
                            {
                                if (x[punktWstawiania] > wezelX) break;
                            }

                            x.Insert(punktWstawiania, wezelX);
                            y.Insert(punktWstawiania, wartoscY);
                            break;                                          // wyjdz z petli while(true)
                        }
                        else Console.WriteLine("Węzeł {0} został już podany, spróbuj ponownie.", wezelX);
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine("Podano błędnie sformatowaną liczbę, spróbuj ponownie.");
                    }
                }
            }
        }

        public static void obliczB()
        {
            Console.WriteLine("Współczynniki b_n:");

            b = new double[x.Count];

            for (int i = 0; i < x.Count; i++)
            {
                b[i] = Iloraz(x, y, 0, i);
                Console.WriteLine("\tb_{0} = {1}", i, Math.Round(b[i], 4));
            }

            Console.Write("\r\n");
        }

        public static double Iloraz(List<double> X, List<double> Y, int i, int n)
        {
            if (n >= 2)
            {
                // NIE przenosić poza funkcję - to jest funkcja
                // REKURENCYJNA - kolejne wywołania nadpiszą
                // ilorazH i ilorazL!
                double ilorazH = Iloraz(X, Y, i + 1, n - 1);
                double ilorazL = Iloraz(X, Y, i, n - 1);

                return (ilorazH - ilorazL) / (X[i + n] - X[i]);
            }
            else if (n == 1)
                return (Y[i + 1] - Y[i]) / (X[i + 1] - X[i]);
            else
                return Y[i];
        }

        public static void utworzNewtona()
        {
            for (int i = 0; i < x.Count; i++)
            {
                if (i > 0) postacNewtona += " +\r\n";

                if(b[i] >= 0)
                    postacNewtona += Math.Round(b[i], 4) + niewiadome(i);
                else if (b[i] < 0)
                    postacNewtona += "(" + Math.Round(b[i], 4) + ")" + niewiadome(i);
            }
        }

        public static string niewiadome(int i)
        {
            string tmp = "";

            for(int j=0; j < i;j++)
            {
                if(x[j] > 0)
                    tmp += "(x-"+ Math.Round(x[j], 4) + ")";
                else if(x[j] < 0)
                    tmp += "(x-(" + Math.Round(x[j], 4) + "))";
            }
            return tmp;
        }

        public static void utworzOgolna()
        {
            postacOgolna = new Nawias(0);

            for (int i = 0; i < x.Count; i++)
            {
                Nawias wn = new Nawias(1) + b[i];                   // 1 czynnik - b_n
                for (int m = 1; m <= i; m++)
                {
                    Czynnik c1 = new Czynnik(true, 1, 1),           // zmienna, 1x^1
                            c2 = new Czynnik(false, -x[m - 1], 1);  // stała,   -x_n
                    Nawias n1 = new Nawias(c1, c2);                 // (x - x_n)

                    wn *= n1;
                }
                postacOgolna += wn;
            }
        }
    }
}