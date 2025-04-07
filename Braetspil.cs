using System;
using System.Collections.Generic;
internal enum Lagerstatus
{
    PåLager,
    BestiltHjem,
    Udgået
}

internal class Braetspil
{
    public string Navn { get; set; }
    public string Version { get; set; }
    public string Stand { get; set; }
    public double Pris { get; set; }
    public string Genre { get; set; }
    public string AntalSpillere { get; set; }
    public Lagerstatus Status { get; set; }

    public Braetspil(string navn, string version, string stand, double pris, string genre, string antalSpillere, Lagerstatus lagerstatus)
    {
        Navn = navn;
        Version = version;
        Stand = stand;
        Pris = pris;
        Genre = genre;
        AntalSpillere = antalSpillere;
        Status = lagerstatus;
    }
}

// Brug enum til lagerstand :)
// Brug enum til stand på brætspil
// Søgefunktion måske contain??
internal class BraetspilManager
    {
        private static BraetspilManager _instance;
        private List<Braetspil> lager = new List<Braetspil>();
        private bool _defaultGamesAdded = false;


    public static BraetspilManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new BraetspilManager();
                return _instance;
            }
        }

    public List<Braetspil> SoegSpil(string name = null, string genre = null, Lagerstatus? status = null, string version = null, string stand = null, string antalSpillere = null)
    {
        var result = lager.AsQueryable();

        if (!string.IsNullOrEmpty(name))
        {
            result = result.Where(s => s.Navn.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(genre))
        {
            result = result.Where(s => s.Genre.Contains(genre, StringComparison.OrdinalIgnoreCase));
        }

        if (status.HasValue)
        {
            result = result.Where(s => s.Status == status);
        }

        if (!string.IsNullOrEmpty(version))
        {
            result = result.Where(s => s.Version.Contains(version, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(stand))
        {
            result = result.Where(s => s.Stand.Contains(stand, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(antalSpillere))
        {
            result = result.Where(s => s.AntalSpillere.Contains(antalSpillere, StringComparison.OrdinalIgnoreCase));
        }

        return result.ToList();
    }


    public void TilfoejDefaultSpil()
        {
            if (_defaultGamesAdded) return;

            Random random = new Random();

            var gamesData = new List<(string Name, string Version, string Stand, double Price, string Genre, string PlayerCount)>
            {
                ("Catan", "Edition 1", "Som Ny", 350, "Strategi", "3-4"),
                ("Monopoly", "Edition 2", "OK", 300, "Familie", "2-6"),
                ("Ticket to Ride", "Anniversary", "OK", 400, "Strategi", "2-5"),
                ("Risk", "Edition 1", "Som Ny", 350, "Strategi", "2-6"),
                ("Pandemic", "Deluxe", "Som Ny", 300, "Samarbejde", "2-4"),
                ("Carcassonne", "Deluxe", "Som Ny", 250, "Strategi", "2-5"),
                ("Clue", "Classic", "OK", 200, "Familie", "3-6"),
                ("Chess", "Standard", "OK", 100, "Strategi", "2"),
                ("Scrabble", "Deluxe", "New", 250, "Ord", "2-4"),
                ("Battleship", "Classic", "OK", 150, "Strategi", "2")
            };

            foreach (var gameData in gamesData)
            {
                Lagerstatus lagerstatus = (Lagerstatus)random.Next(0, 3);

                Braetspil game = new Braetspil(gameData.Name, gameData.Version, gameData.Stand, gameData.Price, gameData.Genre, gameData.PlayerCount, lagerstatus);
                lager.Add(game);
            }
        }

        public void TilfoejSpil(Braetspil spil)
                    {
                        lager.Add(spil);
                        Console.WriteLine("Spil tilføjet til lager!");
                    }

        public void FjernSpil(string navn)
        {
            var spil = lager.Find(s => s.Navn == navn);
            if (spil != null)
            {
                spil.Status = Lagerstatus.Udgået;
                Console.WriteLine("Spil markeret som udgået!");
            }
            else
            {
                Console.WriteLine("Spillet blev ikke fundet.");
            }
        }

        public void VisLager()
        {
            if (lager.Count == 0)
            {
                Console.WriteLine("Lageret er tomt.");
                return;
            }

            Console.WriteLine("Hvordan vil du sortere lageret?");
            Console.WriteLine("1. Navn");
            Console.WriteLine("2. Pris");
            Console.WriteLine("3. Genre");
            Console.WriteLine("4. Antal spillere");
            Console.WriteLine("5. Lagerstatus");
            Console.WriteLine("6. Version");
            Console.WriteLine("7. Stand");
            Console.WriteLine("Indtast nummeret for sortering:");

            if (int.TryParse(Console.ReadLine(), out int sortChoice))
            {
                switch (sortChoice)
                {
                    case 1:
                        lager = lager.OrderBy(s => s.Navn).ToList();
                        break;
                    case 2:
                        lager = lager.OrderBy(s => s.Pris).ToList();
                        break;
                    case 3:
                        lager = lager.OrderBy(s => s.Genre).ToList();
                        break;
                    case 4:
                        lager = lager.OrderBy(s => s.AntalSpillere).ToList();
                        break;
                    case 5:
                        lager = lager.OrderBy(s => s.Status).ToList();
                        break;
                    case 6:
                        lager = lager.OrderBy(s => s.Version).ToList();
                        break;
                    case 7:
                        lager = lager.OrderBy(s => s.Stand).ToList();
                        break;
                    default:
                        Console.WriteLine("Ugyldigt valg. Ingen sortering udført.");
                        return;
                }

                Console.WriteLine("Lagerstatus:");
                foreach (var spil in lager)
                {
                    switch (spil.Status)
                    {
                        case Lagerstatus.PåLager:
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        case Lagerstatus.BestiltHjem:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                        case Lagerstatus.Udgået:
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                    }

                    Console.WriteLine($"{spil.Navn}, {spil.Version}, {spil.Stand}, {spil.Pris} kr, {spil.Genre}, {spil.AntalSpillere} ({spil.Status})");
                }
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("Ugyldigt input. Ingen sortering udført.");
            }
        }


    public List<Braetspil> HentLager()
        {
            return lager;
        }
}
