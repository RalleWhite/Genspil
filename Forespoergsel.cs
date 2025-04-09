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
        Console.WriteLine("Vil du: 1. Vælge eksisterende spil, 2. Oprette nyt spil?");
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
            Console.WriteLine("Ugyldigt valg. Der vælges ingen spil.");
        }


        Console.WriteLine("Vil du: 1. Vælge eksisterende kunde, 2. Oprette ny kunde?");
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
            Console.WriteLine("Ugyldigt valg. Der vælges ingen kunde.");
        }

        Console.Write("Indtast status: 1 = Afventer, 2 = Afsluttet, 3 = Annulleret: ");
        if (!int.TryParse(Console.ReadLine(), out int statusNum) || !Enum.IsDefined(typeof(ForespoergselsStatus), statusNum))
        {
            Console.WriteLine("Ugyldig status. Prøv igen.");
            statusNum = (int)ForespoergselsStatus.Afventer;
        }
        ForespoergselsStatus status = (ForespoergselsStatus)statusNum;

        int forespoergselsNr = GenererForespoergselsnummer();
        DateTime dato = DateTime.Now;

        Console.WriteLine("Vil du: 1. Vælge eksisterende medarbejder, 2. Oprette ny medarbejder?");
        string medarbejderValg = Console.ReadLine();
        Medarbejder medarbejder = null;

        if (MedarbejderManager.Instance.GetMedarbejderCount() == 0 || medarbejderValg == "2")
        {
            medarbejder = MedarbejderManager.Instance.TilføjNyMedarbejder();
        }
        else if (medarbejderValg == "1")
        {
            int index = MedarbejderManager.Instance.VaelgMedarbejder();
            medarbejder = MedarbejderManager.Instance.HentMedarbejder(index);
        }
        else
        {
            Console.WriteLine("Ugyldigt valg. Der vælges ingen medarbejder.");
        }

        Forespoergsel forespoergsel = new Forespoergsel(forespoergselsNr, dato, spilNavn, status, kunde, medarbejder);
        return forespoergsel;
    }


    public void TilfoejForespoergsel(Forespoergsel forespoergsel)
    {
        forespoergsler.Add(forespoergsel);
        Console.WriteLine("forespørgsel oprettet");
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
            Console.WriteLine("Der er ingen forespørgsler");
            return;
        }
        else
        {
            foreach (var forespoergsel in forespoergsler)
            {
                Console.WriteLine($"forespørgsel: {forespoergsel.ForespoergselNr}");
                Console.WriteLine($"Dato: {forespoergsel.Dato.ToShortDateString()}");
                Console.WriteLine($"Spil: {forespoergsel.SpilNavn}");
                Console.WriteLine($"Kunde: {forespoergsel.Kunde.Navn}");
                Console.WriteLine($"Status: {forespoergsel.Status}");
            }
            Console.WriteLine("\nTryk på enhver knap for at gå tilbage.");
        }
    }

    public void VisForespoergsel(Forespoergsel forespoergsel)
    {
        if (forespoergsler.Count == 0)
        {
            Console.WriteLine("Der er ingen forespørgsler");
            return;
        }
        else
        {
            Console.WriteLine($"forespørgsel: {forespoergsel.ForespoergselNr}");
            Console.WriteLine($"Dato: {forespoergsel.Dato.ToShortDateString()}");
            Console.WriteLine($"Spil: {forespoergsel.SpilNavn}");
            Console.WriteLine($"Kunde: {forespoergsel.Kunde.Navn}");
            Console.WriteLine($"Status: {forespoergsel.Status}");
            Console.WriteLine("\nTryk på enhver knap for at gå tilbage.");
        }
    }
    public void RedigerForespoergsel(Forespoergsel forespørgsel)
    {
        Console.WriteLine("Indtast ny status: 1 = Afventer, 2 = Afsluttet, 3 = Annulleret: ");

        if (int.TryParse(Console.ReadLine(), out int statusNum) && Enum.IsDefined(typeof(ForespoergselsStatus), statusNum))
        {
            forespørgsel.Status = (ForespoergselsStatus)statusNum;
            Console.WriteLine("Status for forespørgslen er opdateret!");
        }
        else
        {
            Console.WriteLine("Status for forespørgslen er ikke opdateret!");
        }
    }

    public void SletForespoergsel(int forespørgselsNr)
    {
        var forespørgsel = forespoergsler.Find(f => f.ForespoergselNr == forespørgselsNr);
        if (forespørgsel == null)
        {
            Console.WriteLine("Forespørgslen blev ikke fundet.");
        }
        else
        {
            Console.WriteLine($"Rediger forespørgsel: {forespørgsel.ForespoergselNr}");
            Console.WriteLine($"Dato: {forespørgsel.Dato.ToShortDateString()}");
            Console.WriteLine($"Spil: {forespørgsel.SpilNavn}");
            Console.WriteLine($"Kunde: {forespørgsel.Kunde.Navn}");
            Console.WriteLine($"Telefon: {forespørgsel.Kunde.TlfNr}");
            Console.WriteLine($"Adresse: {forespørgsel.Kunde.Adresse}");
            Console.WriteLine($"Email: {forespørgsel.Kunde.Email}");
            Console.WriteLine($"Nuværende status: {forespørgsel.Status}");

            Console.Write("Er du sikker på at du vil slette forespørgslen? (ja/nej): ");
            string bekræftelse = Console.ReadLine();

            if (bekræftelse.ToLower() == "ja")
            {
                forespoergsler.Remove(forespørgsel);
                Console.WriteLine("Forespørgslen blev slettet!");
            }
            else
            {
                Console.WriteLine("Forespørgslen blev ikke slettet.");
            }
        }
    }

}
