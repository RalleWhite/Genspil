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
}

internal class ReservationManager
{
    private static ReservationManager _instance;
    public static List<Reservation> reservationer = new List<Reservation>();
    private bool _defaultReservationsAdded = false;

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
        Console.Write("Indtast dato for afhentning (dd-mm-åååå): ");
        string inputDato = Console.ReadLine();
        DateTime afhentningsDato;

        while (!DateTime.TryParse(inputDato, out afhentningsDato))
        {
            Console.WriteLine("Ugyldig dato. Prøv igen.");
            inputDato = Console.ReadLine();
        }

        int afhentningsNr = GenererAfhentningsNr();
        DateTime reservationsDato = DateTime.Now;

        Console.Write("Indtast status (1. Oprettet | 2. Klar | 3. Afhentet | 4. Annulleret): \n");
        ReservationStatus status = (ReservationStatus)int.Parse(Console.ReadLine());

        Console.WriteLine("Vil du: 1. Vælge eksisterende brætspil, 2. Oprette nyt brætspil?");
        string braetspilValg = Console.ReadLine();
        Braetspil braetspil = null;

        if (BraetspilManager.Instance.GetBraetspilCount() == 0 || braetspilValg == "2")
        {
            if (BraetspilManager.Instance.GetBraetspilCount() == 0)
            {
                Console.WriteLine("Ingen eksisterende brætspil, venligst opret et ny.");
            }
            braetspil = BraetspilManager.Instance.OpretNytSpil();
            BraetspilManager.Instance.TilfoejSpil(braetspil);
        }
        else if (braetspilValg == "1")
        {
            int index = BraetspilManager.Instance.VaelgBraetspil();
            braetspil = BraetspilManager.Instance.HentBraetspil(index);
        }
        else
        {
            Console.WriteLine("Ugyldigt valg. Der vælges ingen brætspil.");
        }

        Console.WriteLine("Vil du: 1. Vælge eksisterende kunde, 2. Oprette ny kunde?");
        string kundeValg = Console.ReadLine();
        Kunde kunde = null;

        if (KundeManager.Instance.GetCustomerCount() == 0 || kundeValg == "2")
        {
            if (KundeManager.Instance.GetCustomerCount() == 0)
            {
                Console.WriteLine("Ingen eksisterende kunder, venligst opret en ny.");
            }
            var (navn, email, adresse, tlfNr) = KundeManager.Instance.OpretNyKunde();
            kunde = KundeManager.Instance.OpretKunde(navn, email, adresse, tlfNr);
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

        Console.WriteLine("Vil du: 1. Vælge eksisterende medarbejder, 2. Oprette ny medarbejder?");
        string medarbejderValg = Console.ReadLine();

        Medarbejder medarbejder = null;

        if (MedarbejderManager.Instance.GetMedarbejderCount() == 0 || medarbejderValg == "2")
        {
            if (MedarbejderManager.Instance.GetMedarbejderCount() == 0)
            {
                Console.WriteLine("Ingen eksisterende medarbejdere, venligst opret en ny.");
            }
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
                Console.WriteLine($"Reservation: {reservation.AfhentningsNr}");
                Console.WriteLine($"Reservationsdato: {reservation.ReservationsDato.ToShortDateString()}");
                Console.WriteLine($"Afhentningsdato: {reservation.AfhentningsDato.ToShortDateString()}");
                Console.WriteLine($"Spil: {reservation.Braetspil.Navn}");
                Console.WriteLine($"Spil version: {reservation.Braetspil.Version}");
                Console.WriteLine($"Kunde: {reservation.Kunde.Navn}");
                Console.WriteLine($"Status: {reservation.Status}");
                Console.WriteLine($"Medarbejder: {reservation.Medarbejder?.Navn}");
            }
            Console.WriteLine("\nTryk på enhver knap for at gå tilbage.");
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
            Console.WriteLine($"Reservation: {reservation.AfhentningsNr}");
            Console.WriteLine($"Reservationsdato: {reservation.ReservationsDato.ToShortDateString()}");
            Console.WriteLine($"Afhentningsdato: {reservation.AfhentningsDato.ToShortDateString()}");
            Console.WriteLine($"Spil: {reservation.Braetspil.Navn}");
            Console.WriteLine($"Spil version: {reservation.Braetspil.Version}");
            Console.WriteLine($"Kunde: {reservation.Kunde.Navn}");
            Console.WriteLine($"Status: {reservation.Status}");
            Console.WriteLine($"Medarbejder: {reservation.Medarbejder?.Navn}");
            Console.WriteLine("\nTryk på enhver knap for at gå tilbage.");
        }
    }

    public void RedigerReservation(Reservation reservation)
    {
        Console.WriteLine("Indtast ny status: 1 = Oprettet,  2 = Klar, 3 = Afhentet, 4 = Annulleret \n");

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
            Console.Write("Er du sikker på at du vil slette reservationen? (ja/nej): ");
            string bekræftelse = Console.ReadLine();

            if (bekræftelse.ToLower() == "ja")
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
}
  
