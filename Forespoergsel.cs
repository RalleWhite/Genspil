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
        Console.WriteLine("Vil du: 1. V�lge eksisterende spil, 2. Oprette nyt spil?");
        string spilValg = Console.ReadLine();
        string spilNavn = null;

        if (BraetspilManager.Instance.GetBraetspilCount() == 0 || spilValg == "2")
        {
            Braetspil nytSpil = BraetspilManager.Instance.OpretNytSpil();
            BraetspilManager.Instance.TilfoejSpil(nytSpil);
            spilNavn = nytSpil.Navn;
        }
        else if (spilValg == "1")
        {
            int index = BraetspilManager.Instance.VaelgBraetspil();
            Braetspil valgtSpil = BraetspilManager.Instance.HentBraetspil(index);
            spilNavn = valgtSpil.Navn;
        }
        else
        {
            Console.WriteLine("Ugyldigt valg. Der v�lges ingen spil.");
        }


        Console.WriteLine("Vil du: 1. V�lge eksisterende kunde, 2. Oprette ny kunde?");
        string kundeValg = Console.ReadLine();
        Kunde kunde = null;

        if (KundeManager.Instance.GetCustomerCount() == 0 || kundeValg == "2")
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
        else if (kundeValg == "1")
        {
            int index = KundeManager.Instance.VaelgKunde();
            kunde = KundeManager.Instance.HentKunde(index);
        }
        else
        {
            Console.WriteLine("Ugyldigt valg. Der v�lges ingen kunde.");
        }

        Console.Write("Indtast status: 1 = Afventer, 2 = Afsluttet, 3 = Annulleret: ");
        if (!int.TryParse(Console.ReadLine(), out int statusNum) || !Enum.IsDefined(typeof(ForespoergselsStatus), statusNum))
        {
            Console.WriteLine("Ugyldig status. Pr�v igen.");
            statusNum = (int)ForespoergselsStatus.Afventer;
        }
        ForespoergselsStatus status = (ForespoergselsStatus)statusNum;

        int forespoergselsNr = GenererForespoergselsnummer();
        DateTime dato = DateTime.Now;

        Console.WriteLine("Vil du: 1. V�lge eksisterende medarbejder, 2. Oprette ny medarbejder?");
        string medarbejderValg = Console.ReadLine();
        Medarbejder medarbejder = null;

        if (MedarbejderManager.Instance.GetMedarbejderCount() == 0 || medarbejderValg == "2")
        {
            medarbejder = MedarbejderManager.Instance.Tilf�jNyMedarbejder();
        }
        else if (medarbejderValg == "1")
        {
            int index = MedarbejderManager.Instance.VaelgMedarbejder();
            medarbejder = MedarbejderManager.Instance.HentMedarbejder(index);
        }
        else
        {
            Console.WriteLine("Ugyldigt valg. Der v�lges ingen medarbejder.");
        }

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
                Console.WriteLine($"foresp�rgsel: {forespoergsel.ForespoergselNr}");
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
            Console.WriteLine($"foresp�rgsel: {forespoergsel.ForespoergselNr}");
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

}
