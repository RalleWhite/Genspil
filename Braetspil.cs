using System;
using System.Collections.Generic;

internal class Braetspil
{
    public string Navn { get; set; }
    public string Version { get; set; }
    public string Stand { get; set; }
    public double Pris { get; set; }
    public string Genre { get; set; }
    public string AntalSpillere { get; set; }

    public Braetspil(string navn, string version, string stand, double pris, string genre, string antalSpillere)
    {
        Navn = navn;
        Version = version;
        Stand = stand;
        Pris = pris;
        Genre = genre;
        AntalSpillere = antalSpillere;
    }
}

// Brug enum til lagerstand :)
// Brug enum til stand på brætspil
// Søgefunktion måske contain??
internal class BraetspilManager
    {
        private static BraetspilManager _instance;
        private List<Braetspil> lager = new List<Braetspil>();

        public static BraetspilManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new BraetspilManager();
                return _instance;
            }
        }
        public void TilfoejSpil(Braetspil spil)
        {
            lager.Add(spil);
            Console.WriteLine("Spil tilføjet til lager!");
        }

        public void FjernSpil(string navn)
        {
            int beforeCount = lager.Count;
            lager.RemoveAll(s => s.Navn == navn);
            Console.WriteLine(beforeCount > lager.Count ? "Spil fjernet!" : "Spillet blev ikke fundet.");
        }

        public void VisLager()
        {
            if (lager.Count == 0)
            {
                Console.WriteLine("Lageret er tomt.");
            }
            else
            {
                Console.WriteLine("Lagerstatus:");
                foreach (var spil in lager)
                {
                    Console.WriteLine($"{spil.Navn}, {spil.Version}, {spil.Stand}, {spil.Pris} kr, {spil.Genre}, {spil.AntalSpillere}");
                }
            }
        }
        public List<Braetspil> HentLager()
        {
            return lager;
        }
    }
