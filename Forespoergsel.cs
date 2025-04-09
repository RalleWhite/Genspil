using System;
using System.Collections.Generic;

public enum ForespoergselsStatus
{
    Afventer = 1,
    Afsluttet = 2,
    Annulleret = 3
}

internal class Forespoergsel
{
    public int ForespoergselNr { get; set; }
    public DateTime Dato { get; set; }
    public string SpilNavn { get; set; }
    public ForespoergselsStatus Status { get; set; }
    public Kunde Kunde { get; set; }
    public Medarbejder Medarbejder { get; set; }

    public Forespoergsel(int forespoergselNr, DateTime dato, string spilNavn, ForespoergselsStatus status, Kunde kunde, Medarbejder medarbejder)
    {
        ForespoergselNr = forespoergselNr;
        Dato = dato.Date;
        SpilNavn = spilNavn;
        Status = status;
        Kunde = kunde;
        Medarbejder = medarbejder;
    }
    public Forespoergsel() { }
}


internal class ForespoergselManager
{
    private static ForespoergselManager _instance;
    public static List<Forespoergsel> forespoergsler = new List<Forespoergsel>();
    private bool _defaultForespoergslersAdded = false;

    public static ForespoergselManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new ForespoergselManager();
            return _instance;
        }
    }
    public Forespoergsel OpretNyForespoergsel()
    {
        Console.Write("Indtast status: 1 = Afventer, 2 = Afsluttet, 3 = Annulleret: ");
        if (!int.TryParse(Console.ReadLine(), out int statusNum) || !Enum.IsDefined(typeof(ForespoergselsStatus), statusNum))
        {
            Console.WriteLine("Ugyldig status. Pr�v igen.");
            statusNum = (int)ForespoergselsStatus.Afventer;
        }
        ForespoergselsStatus status = (ForespoergselsStatus)statusNum;

        string promptBr�t = "Vil du v�lge eksisterende br�tspil eller oprette nyt br�tspil?";
        string[] menuPunkt = { "Eksisterende", "Opret nyt" };
        Menu ForV�lgSpilMenu = new Menu(promptBr�t, menuPunkt);
        int br�tspilValg = ForV�lgSpilMenu.K�r();
        string spilNavn = null;

        if (BraetspilManager.Instance.GetBraetspilCount() == 0 || br�tspilValg == 1)
        {
            Braetspil nytSpil = BraetspilManager.Instance.OpretNytSpil();
            BraetspilManager.Instance.TilfoejSpil(nytSpil);
            spilNavn = nytSpil.Navn;
        }
        else if (br�tspilValg == 0)
        {
            int index = BraetspilManager.Instance.VaelgBraetspil();
            Braetspil valgtSpil = BraetspilManager.Instance.HentBraetspil(index);
            spilNavn = valgtSpil.Navn;
        }

        string promptKunde = "Vil du v�lge eksisterende kunde eller oprette ny kunde?";
        string[] menuPunkt1 = { "Eksisterende", "Opret ny" };
        Menu V�lgKundeMenu = new Menu(promptKunde, menuPunkt1);
        int kundeValg = V�lgKundeMenu.K�r();
        Kunde kunde = null;

        if (KundeManager.Instance.GetCustomerCount() == 0 || kundeValg == 1)
        {
            Console.Write("Indtast kundens navn: ");
            string kundeNavn = Console.ReadLine();
            Console.Write("Indtast kundens telefonnummer: ");
            string kundeTelefon = Console.ReadLine();
            Console.Write("Indtast kundens adresse: ");
            string kundeAdresse = Console.ReadLine();
            Console.Write("Indtast kundens email: ");
            string kundeEmail = Console.ReadLine();
            kunde = KundeManager.Instance.OpretKunde(kundeNavn, kundeEmail, kundeAdresse, kundeTelefon);
        }
        else if (kundeValg == 0)
        {
            int index = KundeManager.Instance.VaelgKunde();
            kunde = KundeManager.Instance.HentKunde(index);
        }

        string promptMedarbejder = "Vil du v�lge eksisterende medarbejder eller oprette ny medarbejder?";
        Menu V�lgMedarbejderMenu = new Menu(promptMedarbejder, menuPunkt1);
        int medarbejderValg = V�lgMedarbejderMenu.K�r();
        Medarbejder medarbejder = null;

        if (MedarbejderManager.Instance.GetMedarbejderCount() == 0 || medarbejderValg == 1)
        {
            medarbejder = MedarbejderManager.Instance.Tilf�jNyMedarbejder();
        }
        else if (medarbejderValg == 0)
        {
            int index = MedarbejderManager.Instance.VaelgMedarbejder();
            medarbejder = MedarbejderManager.Instance.HentMedarbejder(index);
        }

        int forespoergselsNr = GenererForespoergselsnummer();
        DateTime dato = DateTime.Now;
        Forespoergsel forespoergsel = new Forespoergsel(forespoergselsNr, dato, spilNavn, status, kunde, medarbejder);
        return forespoergsel;
    }

    public void TilfoejForespoergsel(Forespoergsel forespoergsel)
    {
        forespoergsler.Add(forespoergsel);
        Console.WriteLine("foresp�rgsel oprettet");
    }

    private int GenererForespoergselsnummer()
    {
        if (forespoergsler.Count == 0) return 1;
        return forespoergsler.Max(f => f.ForespoergselNr) + 1;
    }
    public void VisForespoergsler()
    {
        if (forespoergsler.Count == 0)
        {
            Console.WriteLine("Der er ingen foresp�rgsler");
            return;
        }
        else
        {
            foreach (var forespoergsel in forespoergsler)
            {
                Console.WriteLine($"Foresp�rgselsnr: {forespoergsel.ForespoergselNr}");
                Console.WriteLine($"Dato: {forespoergsel.Dato.ToShortDateString()}");
                Console.WriteLine($"Spil: {forespoergsel.SpilNavn}");
                Console.WriteLine($"Kunde: {forespoergsel.Kunde.Navn}");
                Console.WriteLine($"Status: {forespoergsel.Status}");
            }
            Console.WriteLine("\nTryk p� enhver knap for at g� tilbage.");
        }
    }

    public void VisForespoergsel(Forespoergsel forespoergsel)
    {
        if (forespoergsler.Count == 0)
        {
            Console.WriteLine("Der er ingen foresp�rgsler");
            return;
        }
        else
        {
            Console.WriteLine($"Foresp�rgselsnr: {forespoergsel.ForespoergselNr}");
            Console.WriteLine($"Dato: {forespoergsel.Dato.ToShortDateString()}");
            Console.WriteLine($"Spil: {forespoergsel.SpilNavn}");
            Console.WriteLine($"Kunde: {forespoergsel.Kunde.Navn}");
            Console.WriteLine($"Status: {forespoergsel.Status}");
            Console.WriteLine("\nTryk p� enhver knap for at g� tilbage.");
        }
    }
    public void RedigerForespoergsel(Forespoergsel foresp�rgsel)
    {
        Console.WriteLine("Indtast ny status: 1 = Afventer, 2 = Afsluttet, 3 = Annulleret: ");

        if (int.TryParse(Console.ReadLine(), out int statusNum) && Enum.IsDefined(typeof(ForespoergselsStatus), statusNum))
        {
            foresp�rgsel.Status = (ForespoergselsStatus)statusNum;
            Console.WriteLine("Status for foresp�rgslen er opdateret!");
        }
        else
        {
            Console.WriteLine("Status for foresp�rgslen er ikke opdateret!");
        }
    }

    public void SletForespoergsel(int foresp�rgselsNr)
    {
        var foresp�rgsel = forespoergsler.Find(f => f.ForespoergselNr == foresp�rgselsNr);
        if (foresp�rgsel == null)
        {
            Console.WriteLine("Foresp�rgslen blev ikke fundet.");
        }
        else
        {
            Console.WriteLine($"Rediger foresp�rgsel: {foresp�rgsel.ForespoergselNr}");
            Console.WriteLine($"Dato: {foresp�rgsel.Dato.ToShortDateString()}");
            Console.WriteLine($"Spil: {foresp�rgsel.SpilNavn}");
            Console.WriteLine($"Kunde: {foresp�rgsel.Kunde.Navn}");
            Console.WriteLine($"Telefon: {foresp�rgsel.Kunde.TlfNr}");
            Console.WriteLine($"Adresse: {foresp�rgsel.Kunde.Adresse}");
            Console.WriteLine($"Email: {foresp�rgsel.Kunde.Email}");
            Console.WriteLine($"Nuv�rende status: {foresp�rgsel.Status}");

            Console.Write("Er du sikker p� at du vil slette foresp�rgslen? (ja/nej): ");
            string bekr�ftelse = Console.ReadLine();

            if (bekr�ftelse.ToLower() == "ja")
            {
                forespoergsler.Remove(foresp�rgsel);
                Console.WriteLine("Foresp�rgslen blev slettet!");
            }
            else
            {
                Console.WriteLine("Foresp�rgslen blev ikke slettet.");
            }
        }
    }

    public void LoadJSON()
    {
        AppData data = JSONHelper.LoadAppData();
        forespoergsler = data.Forespoergsler ?? new List<Forespoergsel>();
    }

    public void SaveJSON()
    {
        AppData data = JSONHelper.LoadAppData();
        data.Forespoergsler = forespoergsler;
        JSONHelper.SaveAppData(data);
    }

}
