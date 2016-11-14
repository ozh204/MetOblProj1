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
        private static List<int> x = new List<int>();
        private static List<int> y = new List<int>();
        private static int[] b;

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
                Console.WriteLine("Postać newtona:\n" + postacNewtona);
                utworzOgolna();
                Console.WriteLine("Postać ogólna:\n" + postacOgolna);

                // Dodawanie nowego wezla
                Console.Write("Czy dodać kolejny węzeł? (T/N): ");

                char wybor = Console.ReadKey().KeyChar;
                if ((wybor ^ 0x20) == 'T')
                    podajXY(1);
                else
                    break;
            }
        }


        public static void podajXY(int n)
        {
            Console.WriteLine();

            for (int i = 0; i < n; i++)
            {
                while (true)
                {
                    try
                    {
                        Console.Write("Podaj x{0}: ", i);
                        int wezelX = Convert.ToInt32(Console.ReadLine());

                        if (x.Contains(wezelX) == false)
                        {
                            if (x.Count == 0 || x.Last() < wezelX)
                            {
                                x.Add(wezelX);
                                break;                                          // wyjdz z petli while(true)
                            }
                            else Console.WriteLine("Dodawany węzeł musi być większy od ostatniego ({0}), spróbuj ponownie.", x.Last());
                        }
                        else Console.WriteLine("Węzeł {0} został już podany, spróbuj ponownie.", wezelX);
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine("Podano błędnie sformatowaną liczbę, spróbuj ponownie.");
                    }
                }
            }

            Console.WriteLine();

            for (int i = 0; i < n; i++)
            {
                try
                {
                    Console.Write("Podaj y{0}: ", i);
                    y.Add(Convert.ToInt32(Console.ReadLine()));
                }
                catch (FormatException e)
                {
                    Console.WriteLine("Podano błędnie sformatowaną liczbę, spróbuj ponownie.");
                }
            }
        }

        public static void obliczB()
        {
            b = new int[x.Count];

            for (int i = 0; i < x.Count; i++)
            {
                b[i] = Iloraz(x, y, 0, i);
                Console.WriteLine("b_{0} = {1}", i, b[i]);
            }
        }

        public static int Iloraz(List<int> X, List<int> Y, int i, int n)
        {
            if (n >= 2)
            {
                // NIE przenosić poza funkcję - to jest funkcja
                // REKURENCYJNA - kolejne wywołania nadpiszą
                // ilorazH i ilorazL!
                int ilorazH = Iloraz(X, Y, i + 1, n - 1);
                int ilorazL = Iloraz(X, Y, i, n - 1);

                return (ilorazH - ilorazL) / (X[i + n] - X[i]);
            }
            else if (n == 1)
                return (Y[i + 1] - Y[i]) / (X[i + 1] - X[i]);
            else
                return Y[i];
        }

        public static void utworzNewtona()
        {
            postacNewtona = "";

            for (int i = 0; i < x.Count; i++)
            {
                if(b[i] > 0)
                    postacNewtona += b[i] + niewiadome(i);
                else if (b[i] < 0)
                    postacNewtona += "(" + b[i] + ")" + niewiadome(i);
                if (i < x.Count) postacNewtona += " + ";
            }
        }

        public static string niewiadome(int i)
        {
            string tmp = "";

            for(int j=0; j < i;j++)
            {
                if(x[j] > 0)
                    tmp += "(x-"+ x[j] + ")";
                else if(x[j] < 0)
                    tmp += "(x-(" + x[j] + "))";
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