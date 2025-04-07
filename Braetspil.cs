using System;
using System.Collections.Generic;
internal enum Lagerstatus
{
    PåLager,
    BestiltHjem,
    Udgået
}
internal enum SpilStand
{
    Ingen = 0,
    Fremragende = 1,
    God = 2,
    Okay = 3,
    Repareres = 4
}

class Braetspil
{
    public string Navn { get; set; }
    public string Version { get; set; }
    public double Pris { get; set; }
    public string Genre { get; set; }
    public string AntalSpillere { get; set; }
    public SpilStand Stand { get; set; }
    public Lagerstatus Status { get; set; }

    public Braetspil(string navn, string version, double pris, string genre, string antalSpillere, SpilStand stand, Lagerstatus lagerstatus)
    {
        Navn = navn;
        Version = version;
        Pris = pris;
        Genre = genre;
        AntalSpillere = antalSpillere;
        Stand = stand;
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

    public List<Braetspil> SoegSpil(string name = null, string genre = null, Lagerstatus? status = null, string version = null, SpilStand? stand = null, string antalSpillere = null)
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

        if (stand.HasValue)
        {
            result = result.Where(s => s.Stand == stand);
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

        var gamesData = new List<(string Name, string Version, SpilStand stand, double Price, string Genre, string PlayerCount)>
            {
                ("Catan", "Edition 1", (SpilStand)1, 350, "Strategi", "3-4"),
                ("Monopoly", "Edition 2", (SpilStand)1, 300, "Familie", "2-6"),
                ("Ticket to Ride", "Anniversary",(SpilStand)1, 400, "Strategi", "2-5"),
                ("Risk", "Edition 1", (SpilStand)3, 350, "Strategi", "2-6"),
                ("Pandemic", "Deluxe", (SpilStand)2, 300, "Samarbejde", "2-4"),
                ("Carcassonne", "Deluxe", (SpilStand)2, 250, "Strategi", "2-5"),
                ("Clue", "Classic", (SpilStand)1, 200, "Familie", "3-6"),
                ("Chess", "Standard", (SpilStand)0, 100, "Strategi", "2"),
                ("Scrabble", "Deluxe", (SpilStand)3, 250, "Ord", "2-4"),
                ("Battleship", "Classic",(SpilStand)2, 150, "Strategi", "2")
            };

        foreach (var gameData in gamesData)
        {
            Lagerstatus lagerstatus = (Lagerstatus)random.Next(0, 3);

            Braetspil game = new Braetspil(gameData.Name, gameData.Version, gameData.Price, gameData.Genre, gameData.PlayerCount, gameData.stand, lagerstatus);
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

        string prompt = "--- Vis Lager ---\n";
        string[] menuPunkter = { "Navn", "Pris", "Genre", "Antal Spillere", "Lagerstatus", "Version", "Stand" };
        Menu visLagerMenu = new Menu(prompt, menuPunkter);
        int indexValgt = visLagerMenu.Kør();

        switch (indexValgt)
        {
            case 0:
                lager = lager.OrderBy(s => s.Navn).ToList();
                break;
            case 1:
                lager = lager.OrderBy(s => s.Pris).ToList();
                break;
            case 2:
                lager = lager.OrderBy(s => s.Genre).ToList();
                break;
            case 3:
                lager = lager.OrderBy(s => s.AntalSpillere).ToList();
                break;
            case 4:
                lager = lager.OrderBy(s => s.Status).ToList();
                break;
            case 5:
                lager = lager.OrderBy(s => s.Version).ToList();
                break;
            case 6:
                lager = lager.OrderBy(s => s.Stand).ToList();
                break;
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


    public List<Braetspil> HentLager()
        {
            return lager;
        }

    public Braetspil OpretNytSpil()
    {
        Console.Write("Indtast navn på brætspil: ");
        string navn = Console.ReadLine();
        Console.Write("Indtast version: ");
        string version = Console.ReadLine();
        Console.Write("Indtast stand (0 = Ingen, 1 = Fremragende, 3 = God, 4 = Okay): \n");
        SpilStand stand = (SpilStand)int.Parse(Console.ReadLine());
        Console.Write("Indtast pris: ");
        double pris = Convert.ToDouble(Console.ReadLine());
        Console.Write("Indtast genre: ");
        string genre = Console.ReadLine();
        Console.Write("Indtast antal spillere: ");
        string antalSpillere = Console.ReadLine();
        Console.Write("Indtast lagerstatus: 1 = På lager, 2 = Bestilt hjem: ");
        int statusInt = int.Parse(Console.ReadLine());
        Lagerstatus lagerstatus = (Lagerstatus)(statusInt - 1);

        Braetspil braetspil = new Braetspil(navn, version, pris, genre, antalSpillere, stand, lagerstatus);
        return braetspil;

    }
    public void SpilUdgået()
    {
        foreach (var spil in BraetspilManager.Instance.HentLager())
        {
            Console.WriteLine(spil.Navn);
        }
        Console.WriteLine("Indtast navn på spil der skal fjernes:");
        string spilNavn = Console.ReadLine();
        BraetspilManager.Instance.FjernSpil(spilNavn);
    }

    public void SoegSpil()
    {
        Console.Write("Søg efter et brætspil: ");

        Console.Write("Indtast navnet på spillet (eller tryk Enter for at springe over): ");
        string searchTerm = Console.ReadLine();

        Console.Write("Indtast genre (eller tryk Enter for at springe over): ");
        string searchGenre = Console.ReadLine();

        Console.Write("Indtast version (eller tryk Enter for at springe over): ");
        string searchVersion = Console.ReadLine();

        Console.Write("Indtast stand (0 = Ingen, 1 = Fremragende, 3 = God, 4 = Okay, eller tryk Enter for at springe over): ");
        string standInput = Console.ReadLine();
        SpilStand? stand = null;

        Console.WriteLine("Indtast antal spillere (eller tryk Enter for at springe over): ");
        string searchAntalSpillere = Console.ReadLine();

        Console.WriteLine("Indtast lagerstatus (1 = PåLager, 2 = BestiltHjem, 3 = Udgået, eller tryk Enter for at springe over): ");
        string statusInput = Console.ReadLine();
        Lagerstatus? status = null;

        if (int.TryParse(statusInput, out int statusChoice))
        {
            status = (Lagerstatus)(statusChoice - 1);
        }

        List<Braetspil> searchResults = BraetspilManager.Instance.SoegSpil(searchTerm, searchGenre, status, searchVersion, stand, searchAntalSpillere);

        if (searchResults.Count > 0)
        {
            Console.WriteLine("Søgeresultater: \n");
            foreach (var spil in searchResults)
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
        }
        else
        {
            Console.WriteLine("Ingen spil fundet, der matcher søgningen.");
        }
        Console.ReadLine();
        Console.ResetColor();
    }
}
