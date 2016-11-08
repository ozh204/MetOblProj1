﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetOblProj1
{
    class Program
    {
        private static int ilorazH;
        private static int ilorazL;
        private static string postacNewtona = "";
        //private static string postacOgolna = "";
        private static Nawias postacOgolna = new Nawias(0);
        private static int[] x;
        private static int[] y;
        private static int[] b;

        public static void Main()
        {
            Console.Write("Podaj n: ");
            int n = Convert.ToInt32(Console.ReadLine());

            podajXY(n);
            obliczB(n);
            utworzNewtona(n);
            Console.WriteLine("Postac newtona:\n" + postacNewtona);
            utworzOgolna();
            Console.WriteLine("Postac ogólna:\n" + postacOgolna);
            dodajNowyWezel();
            Console.ReadKey();
        }


        public static void podajXY(int n)
        {
            x = new int[n+1];
            y = new int[n+1];

            for (int i = 0; i <= n; i++)
            {
                Console.Write("Podaj x{0}: ", i);
                x[i] = Convert.ToInt32(Console.ReadLine());
            }
            Console.WriteLine();
            for (int i = 0; i <= n; i++)
            {
                Console.Write("Podaj y{0}: ", i);
                y[i] = Convert.ToInt32(Console.ReadLine());
            }
        }
        public static void obliczB(int n)
        {
            b = new int[n + 1];

            for (int i = 0; i < n + 1; i++)
            {
                b[i] = Iloraz(x, y, 0, i);
                Console.WriteLine("b_{0} = {1}", i, b[i]);
            }
        }
        public static int Iloraz(int[] X, int[] Y, int i, int n)
        {
            if (n >= 2)
            {
                ilorazH = Iloraz(X, Y, i + 1, n - 1);
                ilorazL = Iloraz(X, Y, i, n - 1);

                return (ilorazH - ilorazL) / (X[i + n] - X[i]);
            }
            else if (n == 1)
                return (Y[i + 1] - Y[i]) / (X[i + 1] - X[i]);
            else
                return Y[i];
        }
        public static void utworzNewtona(int n)
        {
            for (int i = 0; i < n + 1; i++)
            {
                if(b[i] > 0)
                    postacNewtona += b[i] + niewiadome(i);
                else if (b[i] < 0)
                    postacNewtona += "(" + b[i] + ")" + niewiadome(i);
                if (i < n) postacNewtona += " + ";
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
            // Wyznaczanie postaci ogolnej
            //Nawias postacOgolna = new Nawias(0);                          // 0 czynników, pusty!

            for (int i = 0; i < x.Count(); i++)
            {
                Nawias wn = new Nawias(1) + b[i];                 // 1 czynnik - b_n
                for (int m = 1; m <= i; m++)
                {
                    Czynnik c1 = new Czynnik(true, 1, 1),           // zmienna, 1x^1
                            c2 = new Czynnik(false, -x[m - 1], 1);  // stała,   -x_n
                    Nawias n1 = new Nawias(c1, c2);                // (x - x_n)

                    wn *= n1;
                }
                postacOgolna += wn;
            }
        }
        public static void dodajNowyWezel()
        {

        }
    }
}