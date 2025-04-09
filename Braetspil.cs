using System;
using System.Collections.Generic;
using System.Reflection;
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
    public Braetspil() { }
}
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

        var gamesData = new List<(string Name, string Version, double Price, string Genre, string PlayerCount)>
            {
                ("Ticket to Ride", "First Journey", 99, "Familiespil", "2-4"),
                ("Ticket to Ride", "Berlin", 99, "Familiespil", "2-4"),
                ("7 Wonders", "2nd edition - dansk", 99, "Strategispil", "3-7"),
                ("7 Wonders", "Architects - dansk", 99, "Strategispil", "3-7"),
                ("Alverdens", "Original", 99, "Quiz spil", "2- "),
                ("A la Carte", "Dessert", 99, "Familiespil", "2-4"),
                ("Bad People", "Original", 99, "Selskabsspil", "3 -"),
                ("Sequence", "Junior", 99, "Strategispil", "2-12"),
                ("Sequence", "Rejsespil", 99, "Strategispil", "2-12"),
            };

        foreach (var gameData in gamesData)
        {
            Lagerstatus lagerstatus = (Lagerstatus)random.Next(0, 3);
            SpilStand spilStand = (SpilStand)random.Next(0, 4);

            Braetspil game = new Braetspil(gameData.Name, gameData.Version, gameData.Price, gameData.Genre, gameData.PlayerCount, spilStand, lagerstatus);
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
        string[] menuPunkter = { "Navn", "Genre", "Pris", "Status" };
        Menu visLagerMenu = new Menu(prompt, menuPunkter);
        int indexValgt = visLagerMenu.Kør();

        switch (indexValgt)
        {
            case 0:
                lager = lager.OrderBy(s => s.Navn).ToList();
                break;
            case 1:
                lager = lager.OrderBy(s => s.Genre).ToList();
                break;
            case 2:
                lager = lager.OrderBy(s => s.Pris).ToList();
                break;
            case 3:
                lager = lager.OrderBy(s => s.Status).ToList();
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
        Console.Write("Indtast stand (0 = Ingen, 1 = Fremragende, 3 = God, 4 = Okay): ");
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
            Console.WriteLine($"{spil.Navn}, {spil.Version}, {spil.Stand}, {spil.Pris} kr, {spil.Genre}, {spil.AntalSpillere} ({spil.Status})");
        }
        Console.Write("\nIndtast navn på spil der skal fjernes: ");
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

        Console.Write("Indtast antal spillere (eller tryk Enter for at springe over): ");
        string searchAntalSpillere = Console.ReadLine();

        Console.Write("Indtast lagerstatus (1 = PåLager, 2 = BestiltHjem, 3 = Udgået, eller tryk Enter for at springe over): ");
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

    public Braetspil OpretSpilOpdatering()
    {
        Console.Write("Indtast navnet på spillet der skal opdateres: ");
        string navn = Console.ReadLine();

        var spil = lager.FirstOrDefault(s => s.Navn.Equals(navn, StringComparison.OrdinalIgnoreCase));
        if (spil == null)
        {
            Console.WriteLine("Spillet blev ikke fundet.");
            return null;
        }

        Console.WriteLine("--- Opdater felter (tryk Enter for at springe over) ---");

        Console.Write($"Nyt navn ({spil.Navn}): ");
        string nytNavn = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(nytNavn)) spil.Navn = nytNavn;

        Console.Write($"Ny version ({spil.Version}): ");
        string nyVersion = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(nyVersion)) spil.Version = nyVersion;

        Console.Write($"Ny pris ({spil.Pris}): ");
        string nyPris = Console.ReadLine();
        if (double.TryParse(nyPris, out double prisVal)) spil.Pris = prisVal;

        Console.Write($"Ny genre ({spil.Genre}): ");
        string nyGenre = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(nyGenre)) spil.Genre = nyGenre;

        Console.Write($"Nyt antal spillere ({spil.AntalSpillere}): ");
        string nytAntal = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(nytAntal)) spil.AntalSpillere = nytAntal;

        Console.Write($"Ny stand ({(int)spil.Stand}): ");
        string nyStand = Console.ReadLine();
        if (int.TryParse(nyStand, out int standVal)) spil.Stand = (SpilStand)standVal;

        Console.Write($"Ny lagerstatus ({(int)spil.Status + 1}): ");
        string nyStatus = Console.ReadLine();
        if (int.TryParse(nyStatus, out int statusVal)) spil.Status = (Lagerstatus)(statusVal - 1);

        return spil;
    }


    public void OpdaterSpil(Braetspil spil)
    {
        if (spil != null)
        {
            var eksisterende = lager.Find(s => s.Navn.Equals(spil.Navn, StringComparison.OrdinalIgnoreCase));

            if (eksisterende != null)
            {
                eksisterende.Navn = spil.Navn;
                eksisterende.Version = spil.Version;
                eksisterende.Pris = spil.Pris;
                eksisterende.Genre = spil.Genre;
                eksisterende.AntalSpillere = spil.AntalSpillere;
                eksisterende.Stand = spil.Stand;
                eksisterende.Status = spil.Status;

                Console.WriteLine("Spiloplysninger opdateret!");
            }
            else
            {
                Console.WriteLine("Spillet blev ikke fundet.");
            }
        }
    }

    public int VaelgBraetspil()
    {
        var lager = BraetspilManager.Instance.HentLager();
        if (lager.Count == 0)
        {
            Console.WriteLine("Ingen brætspil tilgængelige. Tilføj et brætspil først.");
            return -1;
        }

        int index = -1;
        bool gyldigtValg = false;

        while (!gyldigtValg)
        {
            Console.WriteLine("Vælg et brætspil:");
            for (int i = 0; i < lager.Count; i++)
            {
                var spil = lager[i];
                Console.WriteLine($"{i + 1}. {spil.Navn} ({spil.Version}) - {spil.Stand}, {spil.Pris} kr - {spil.Status}");
            }
            Console.Write("Indtast nummer på brætspil: ");
            if (int.TryParse(Console.ReadLine(), out int valg) &&
                valg > 0 && valg <= lager.Count)
            {
                index = valg - 1;
                gyldigtValg = true;
            }
            else
            {
                Console.WriteLine("Ugyldigt valg. Prøv igen.");
            }
        }

        return index;
    }
    public Braetspil HentBraetspil(int index)
    {
        var lager = BraetspilManager.Instance.HentLager();
        if (index >= 0 && index < lager.Count)
        {
            return lager[index];
        }
        return null;
    }
    public int GetBraetspilCount()
    {
        return lager.Count;
    }

    public void LoadJSON()
    {
        AppData data = JSONHelper.LoadAppData();
        lager = data.Lager ?? new List<Braetspil>();
    }

    public void SaveJSON()
    {
        AppData data = JSONHelper.LoadAppData();
        data.Lager = lager;
        JSONHelper.SaveAppData(data);
    }
}
