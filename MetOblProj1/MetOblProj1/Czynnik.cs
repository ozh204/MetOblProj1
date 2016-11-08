﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetOblProj1
{
    public struct Czynnik
    {
        public bool CzyZmienna;        // Czy ten czynnik to zmienna (x)?
        public double Wielkosc;        // Albo mnożnik dla zmiennych
        public int Wykladnik;          // Potęga (ważna dla zmiennych)
        public bool CzyMaWartosc;      // Pole pomocne przy wykrywaniu braku inicjalizacji

        public Czynnik(bool CzyZmienna, double Wielkosc, int Wykladnik)
        {
            this.CzyZmienna = CzyZmienna;
            this.Wielkosc = Wielkosc;
            this.Wykladnik = Wykladnik;
            this.CzyMaWartosc = true;
        }

        // Definicja operacji dodawania dwóch struktur Czynnik
        public static Czynnik operator +(Czynnik c1, Czynnik c2)
        {
            Czynnik wynik = c1;

            if (!c1.CzyMaWartosc || !c2.CzyMaWartosc)
            {
                wynik.Wielkosc += c2.Wielkosc;
                wynik.Wykladnik += c2.Wykladnik;
                wynik.CzyZmienna |= c2.CzyZmienna;
                wynik.CzyMaWartosc = true;
            }
            else if (!c1.CzyZmienna && !c2.CzyZmienna)
                wynik.Wielkosc += c2.Wielkosc;
            else if (c1.CzyZmienna && c2.CzyZmienna)
            {
                if (c1.Wykladnik == c2.Wykladnik)
                    wynik.Wielkosc += c2.Wielkosc;
            }

            return wynik;
        }
        // Definicja operacji mnożenia dwóch struktur Czynnik
        public static Czynnik operator *(Czynnik c1, Czynnik c2)
        {
            Czynnik wynik = c1;

            wynik.Wielkosc *= c2.Wielkosc;
            if (c1.CzyZmienna && c2.CzyZmienna)
                wynik.Wykladnik += c2.Wykladnik;
            else
            {
                if (c1.CzyZmienna || c2.CzyZmienna)
                    wynik.Wykladnik *= c2.Wykladnik;
                wynik.CzyZmienna |= c2.CzyZmienna;
            }

            return wynik;
        }
        // Definicja operacji mnożenia czynnika ze stałą
        public static Czynnik operator *(Czynnik c1, double s1)
        {
            Czynnik wynik = c1;
            wynik.Wielkosc *= s1;
            return wynik;
        }
    }
}