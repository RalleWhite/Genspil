using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

public enum ReservationStatus
{
    Oprettet = 1,
    Klar = 2,
    Afhentet = 3,
    Annulleret = 4
}

internal class Reservation
{
    public int AfhentningsNr { get; set; }
    public DateTime AfhentningsDato { get; set; }
    public DateTime ReservationsDato { get; set; }
    public ReservationStatus Status;
    public Kunde Kunde { get; set; }
    public Braetspil Braetspil { get; set; }
    public Medarbejder Medarbejder { get; set; }

    public Reservation(int afhentningsNr, DateTime reservationsDato, DateTime afhentningsDato, Kunde kunde, Braetspil braetspil, ReservationStatus status, Medarbejder medarbejder)
    {
        AfhentningsNr = afhentningsNr;
        ReservationsDato = reservationsDato;
        AfhentningsDato = afhentningsDato;
        Kunde = kunde;
        Braetspil = braetspil;
        Status = status;
        Medarbejder = medarbejder;
    }
    public Reservation() { }
}

internal class ReservationManager
{
    private static ReservationManager _instance;
    public static List<Reservation> reservationer = new List<Reservation>();

    public static ReservationManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new ReservationManager();
            return _instance;
        }
    }
    public Reservation OpretNyReservation()
    {
        Console.Write("Indtast dato for afhentning (dd-mm-����): ");
        string inputDato = Console.ReadLine();
        DateTime afhentningsDato;
        while (!DateTime.TryParse(inputDato, out afhentningsDato))
        {
            Console.WriteLine("Ugyldig dato. Pr�v igen.");
            inputDato = Console.ReadLine();
        }
        Console.Write("Indtast status (1. Oprettet | 2. Klar | 3. Afhentet | 4. Annulleret): ");
        ReservationStatus status = (ReservationStatus)int.Parse(Console.ReadLine());

        string promptBr�t = "Vil du v�lge eksisterende br�tspil eller oprette nyt br�tspil?";
        string[] menuPunkt= { "Eksisterende", "Opret nyt" };
        Menu V�lgSpilMenu = new Menu(promptBr�t, menuPunkt);
        int br�tspilValg = V�lgSpilMenu.K�r();
        Braetspil braetspil = null;

        if (BraetspilManager.Instance.GetBraetspilCount() == 0 || br�tspilValg == 1)
        {
            if (BraetspilManager.Instance.GetBraetspilCount() == 0)
            {
                Console.WriteLine("Ingen eksisterende br�tspil, venligst opret et ny.");
            }
            braetspil = BraetspilManager.Instance.OpretNytSpil();
            BraetspilManager.Instance.TilfoejSpil(braetspil);
        }
        else if (br�tspilValg == 0)
        {
            int index = BraetspilManager.Instance.VaelgBraetspil();
            braetspil = BraetspilManager.Instance.HentBraetspil(index);
        }

        string promptKunde = "Vil du v�lge eksisterende kunde eller oprette ny kunde?";
        string[] menuPunkt1 = { "Eksisterende", "Opret ny" };
        Menu V�lgKundeMenu = new Menu(promptKunde, menuPunkt1);
        int kundeValg = V�lgKundeMenu.K�r();
        Kunde kunde = null;

        if (KundeManager.Instance.GetCustomerCount() == 0 || kundeValg == 1)
        {
            if (KundeManager.Instance.GetCustomerCount() == 0)
            {
                Console.WriteLine("Ingen eksisterende kunder, venligst opret en ny.");
            }
            var (navn, email, adresse, tlfNr) = KundeManager.Instance.OpretNyKunde();
            kunde = KundeManager.Instance.OpretKunde(navn, email, adresse, tlfNr);
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
            if (MedarbejderManager.Instance.GetMedarbejderCount() == 0)
            {
                Console.WriteLine("Ingen eksisterende medarbejdere, venligst opret en ny.");
            }
            medarbejder = MedarbejderManager.Instance.Tilf�jNyMedarbejder();
        }
        else if (medarbejderValg == 0)
        {
            int index = MedarbejderManager.Instance.VaelgMedarbejder();
            medarbejder = MedarbejderManager.Instance.HentMedarbejder(index);
        }

        int afhentningsNr = GenererAfhentningsNr();
        DateTime reservationsDato = DateTime.Now;
        Reservation reservation = new Reservation(afhentningsNr, reservationsDato, afhentningsDato, kunde, braetspil, status, medarbejder);
        return reservation;
    }


    public void TilfoejReservation(Reservation reservation)
    {
        reservationer.Add(reservation);
        Console.WriteLine("Reservation oprettet");
    }

    private int GenererAfhentningsNr()
    {
        if (reservationer.Count == 0) return 1;
        return reservationer.Max(f => f.AfhentningsNr) + 1;
    }

    public void VisReservationer()
    {
        if (reservationer.Count == 0)
        {
            Console.WriteLine("Der er ingen reservationer");
            return;
        }
        else
        {
            foreach (var reservation in reservationer)
            {
                Console.WriteLine($"Afhentningsnr: {reservation.AfhentningsNr}");
                Console.WriteLine($"Reservationsdato: {reservation.ReservationsDato.ToShortDateString()}");
                Console.WriteLine($"Afhentningsdato: {reservation.AfhentningsDato.ToShortDateString()}");
                Console.WriteLine($"Spil: {reservation.Braetspil.Navn}");
                Console.WriteLine($"Spil version: {reservation.Braetspil.Version}");
                Console.WriteLine($"Kunde: {reservation.Kunde.Navn}");
                Console.WriteLine($"Status: {reservation.Status}");
                Console.WriteLine($"Medarbejder: {reservation.Medarbejder?.Navn}");
            }
            Console.WriteLine("\nTryk p� enhver knap for at g� tilbage.");
        }
    }

    public void VisReservation(Reservation reservation)
    {
        if (reservationer.Count == 0)
        {
            Console.WriteLine("Der er ingen reservationer");
            return;
        }
        else
        {
            Console.WriteLine($"Afhentningsnr: {reservation.AfhentningsNr}");
            Console.WriteLine($"Reservationsdato: {reservation.ReservationsDato.ToShortDateString()}");
            Console.WriteLine($"Afhentningsdato: {reservation.AfhentningsDato.ToShortDateString()}");
            Console.WriteLine($"Spil: {reservation.Braetspil.Navn}");
            Console.WriteLine($"Spil version: {reservation.Braetspil.Version}");
            Console.WriteLine($"Kunde: {reservation.Kunde.Navn}");
            Console.WriteLine($"Status: {reservation.Status}");
            Console.WriteLine($"Medarbejder: {reservation.Medarbejder?.Navn}");
            Console.WriteLine("\nTryk p� enhver knap for at g� tilbage.");
        }
    }

    public void OpdaterReservationStatus(Reservation reservation)
    {
        Console.Write("Indtast ny status (1 = Oprettet,  2 = Klar, 3 = Afhentet, 4 = Annulleret): ");

        if (int.TryParse(Console.ReadLine(), out int statusNum) && Enum.IsDefined(typeof(ReservationStatus), statusNum))
        {
            reservation.Status = (ReservationStatus)statusNum;
            Console.WriteLine("Status for reservationen er opdateret!");
        }
        else
        {
            Console.WriteLine("Status for reservationen er ikke opdateret!");
        }
    }
    public void SletReservation(int afhentningsNr)
    {
        var reservation = reservationer.Find(f => f.AfhentningsNr == afhentningsNr);
        if (reservation == null)
        {
            Console.WriteLine("Reservationen blev ikke fundet.");
        }
        else
        {
            Console.Write("Er du sikker p� at du vil slette reservationen? (ja/nej): ");
            string bekr�ftelse = Console.ReadLine();

            if (bekr�ftelse.ToLower() == "ja")
            {
                reservationer.Remove(reservation);
                Console.WriteLine("Reservationen blev slettet!");
            }
            else
            {
                Console.WriteLine("Reservationen blev ikke slettet.");
            }
        }
    }
    public void LoadJSON()
    {
        AppData data = JSONHelper.LoadAppData();
        reservationer = data.Reservationer ?? new List<Reservation>();
    }

    public void SaveJSON()
    {
        AppData data = JSONHelper.LoadAppData();
        data.Reservationer = reservationer;
        JSONHelper.SaveAppData(data);
    }
}
  
