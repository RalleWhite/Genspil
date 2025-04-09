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

        Braetspil braetspil = BraetspilManager.Instance.OpretNytSpil();
        BraetspilManager.Instance.TilfoejSpil(braetspil);

        Console.Write("Indtast kundens navn: ");
        string kundeNavn = Console.ReadLine();
        Console.Write("Indtast kundens telefonnummer: ");
        string kundeTelefon = Console.ReadLine();
        Console.Write("Indtast kundens adresse: ");
        string kundeAdresse = Console.ReadLine();
        Console.Write("Indtast kundens email: ");
        string kundeEmail = Console.ReadLine();
        var kunde = KundeManager.Instance.OpretKunde(kundeNavn, kundeEmail, kundeAdresse, kundeTelefon);

        Medarbejder medarbejder = MedarbejderManager.Instance.TilføjNyMedarbejder();

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
  
