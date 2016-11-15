using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetOblProj1
{
    public class Nawias
    {
        // Lista wszystkich czynników (iksów czy stałych) w nawiasie
        private List<Czynnik?> WszystkieCzynniki = null;
        // Publiczna właściwość zawierająca ilość czynników
        public int IloscCzynnikow
        {
            get { return WszystkieCzynniki.Count; }
        }
        // Indekstator dla łatwiejszego dostępu do czynników
        public Czynnik this[int i]
        {
            get
            {
                Czynnik c = new Czynnik();
                if (i >= 0 && i < WszystkieCzynniki.Count)
                {
                    if (WszystkieCzynniki[i] != null)
                        c = (Czynnik)WszystkieCzynniki[i];
                }

                return c;
            }
            set { WszystkieCzynniki[i] = value; }
        }
        // Konstruktor tworzący nowe wyrażenie o 
        // podanej liczbie czynników
        public Nawias(int LiczbaCzynnikow)
        {
            WszystkieCzynniki = new List<Czynnik?>();

            for (int i = 0; i < LiczbaCzynnikow; i++)
                WszystkieCzynniki.Add(null);
        }
        // Konstruktor tworzący nowe wyrażenie, które
        // będzie zawierało dwa podane czynniki (zmienne/stałe)
        public Nawias(Czynnik c1, Czynnik c2)
        {
            WszystkieCzynniki = new List<Czynnik?>();
            WszystkieCzynniki.Add(c1);
            WszystkieCzynniki.Add(c2);
        }
        // Konwersja wyrażenia do stringa
        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            bool sbNiePusty = false;                    // Pozwala ominąć zbędne znaki na początku

            for (int i = 0; i < IloscCzynnikow; i++)
            {
                Czynnik c = WszystkieCzynniki[i] != null ? (Czynnik)WszystkieCzynniki[i] : new Czynnik(false, 0, 1);
                if (c.Wielkosc != 0.0)
                {
                    bool czyUjemny = c.Wielkosc < 0.0;
                    double wBezwzgledna = Math.Abs(c.Wielkosc);

                    if (sbNiePusty || czyUjemny)
                        sb.Append(czyUjemny ? " - " : " + ");
                    if (c.CzyZmienna)
                    {
                        if (wBezwzgledna != 1.0)
                            sb.Append(Math.Round(wBezwzgledna, 4));

                        sb.Append("x");

                        if (c.Wykladnik != 1.0)
                            sb.AppendFormat("^{0}", c.Wykladnik);
                    }
                    else sb.Append(Math.Round(wBezwzgledna, 4));

                    sbNiePusty = true;
                }
            }

            return sb.ToString();
        }
        // Definicja operacji dodawania nawiasu i liczby
        public static Nawias operator +(Nawias w1, double s1)
        {
            Nawias n = new Nawias(w1.IloscCzynnikow == 0 ? 1 : w1.IloscCzynnikow);

            for (int i = 0; i < w1.IloscCzynnikow; i++)
                n[i] = w1[i];

            n[n.IloscCzynnikow - 1] += new Czynnik(false, s1, 1);
            return n;
        }
        // Definicja operacji dodawania nawiasów
        public static Nawias operator +(Nawias w1, Nawias w2)
        {
            // Nowy nawias będzie miał tyle współczynników, ile najdłuższy z dwóch
            Nawias n = new Nawias(Math.Max(w1.IloscCzynnikow, w2.IloscCzynnikow));

            for (int i = 0; i < w1.IloscCzynnikow; i++)
                n[i + (n.IloscCzynnikow - w1.IloscCzynnikow)] += w1[i];
            for (int j = 0; j < w2.IloscCzynnikow; j++)
                n[j + (n.IloscCzynnikow - w2.IloscCzynnikow)] += w2[j];

            return n;
        }
        // Definicja operacji mnożenia na nawiasie i stałej.
        public static Nawias operator *(Nawias w1, double s1)
        {
            Nawias Iloczyn = new Nawias(w1.IloscCzynnikow);

            for (int i = 0; i < w1.IloscCzynnikow; i++)
                Iloczyn[i] = w1[i] * s1;

            return Iloczyn;
        }
        // Definicja operacji mnożenia na nawiasach
        // W nawiasach znajdują się TYLKO WIELOMIANY W POSTACI OGOLNEJ.
        public static Nawias operator *(Nawias w1, Nawias w2)
        {
            // Potrzebujemy informacji o najwyższym wykładniku,
            // który pojawi się w wynikowym wyrażeniu (bo potrzebujemy
            // zaalokować odpowiednio dużo miejsca)
            int NajwyzszyWykladnik = 1;

            for (int i = 0; i < w1.IloscCzynnikow; i++)
            {
                for (int j = 0; j < w2.IloscCzynnikow; j++)
                {
                    // Interesuje nas mnożenie zmiennych (bo mnożenie
                    // iksów podnosi ich wykładnik!)
                    if (w1[i].CzyZmienna && w2[j].CzyZmienna)
                    {
                        // x^a * x^b = x^(a+b)
                        int NowyWykladnik = w1[i].Wykladnik + w2[j].Wykladnik;
                        if (NowyWykladnik > NajwyzszyWykladnik)
                            NajwyzszyWykladnik = NowyWykladnik;
                    }
                }
            }
            // Tworzenie nowej struktury o n+1 czynnikach
            // (pamiętamy o stałych!)
            Nawias Iloczyn = new Nawias(NajwyzszyWykladnik + 1);

            for (int i = 0; i < w1.IloscCzynnikow; i++)
            {
                for (int j = 0; j < w2.IloscCzynnikow; j++)
                {
                    Czynnik c = w1[i] * w2[j];

                    int IndeksWTablicy = NajwyzszyWykladnik;
                    // Zmienne wstawiamy sortując od największej
                    // (na początku) do najmniejszej (na końcu) potęgi
                    if (c.CzyZmienna)
                        IndeksWTablicy = NajwyzszyWykladnik - c.Wykladnik;
                    // Sumujemy wynik mnożenia czynników z obu wyrażeń
                    // z odpowiednim czynnikiem z tablicy
                    Iloczyn[IndeksWTablicy] += c;
                }
            }

            return Iloczyn;
        }
    }
}