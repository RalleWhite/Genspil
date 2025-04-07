using System;
using System.Reflection.Emit;
using System.Reflection;
using System.Runtime.CompilerServices;

class GenspilManagementSystem
{
    private static bool loggedIn = false;
    private static List<Forespoergsel> forespoergsler = new List<Forespoergsel>();
    private static List<Reservation> reservationer = new List<Reservation>();
    static KundeManager kundeManager = new KundeManager();

    public void Kør()
    {
        KørHovedMenu();
        Console.ReadKey(true);
    }

    private void KørHovedMenu()
    {
        string prompt = "--- Velkommen til Genspil Management System ---\nTryk på piletasterne for at navigere og tryk på enter for at vælge.\n";
        string[] menuPunkter = { "Start", "Om Genspil Management System", "Afslut" };
        Menu hovedMenu = new Menu(prompt, menuPunkter);
        int indexValgt = hovedMenu.Kør();

        switch (indexValgt)
        {
            case 0:
                KørStartMenu();
                break;
            case 1:
                VisOmGMS();
                break;
            case 2:
                AfslutGMS();
                break;
        }
    }

    private void KørStartMenu()
    {
        string prompt = "--- Genspil Management System ---\n";
        string[] menuPunkter = { "Lagerstatus", "Reservationer", "Forespørgsler", "Kundedata", "Butikslogin", "Afslut" };
        Menu startMenu = new Menu(prompt, menuPunkter);
        int indexValgt = startMenu.Kør();

        switch (indexValgt)
        {
            case 0:
                if (loggedIn)
                {
                    KørLagerMenu();
                    break;
                }
                else
                {
                    Console.WriteLine("Du er ikke logged ind!");
                    Thread.Sleep(2000);
                    KørStartMenu();
                    break;
                }
            case 1:
                if (loggedIn)
                {
                    KørReservationMenu();
                    break;
                }
                else
                {
                    Console.WriteLine("Du er ikke logged ind!");
                    Thread.Sleep(2000);
                    KørStartMenu();
                    break;
                }
            case 2:
                if (loggedIn)
                {
                    KørForespoergselMenu();
                    break;
                }
                else
                {
                    Console.WriteLine("Du er ikke logged ind!");
                    Thread.Sleep(2000);
                    KørStartMenu();
                    break;
                }
            case 3:
                if (loggedIn)
                {
                    Console.WriteLine("KundeMenu HER");
                    KørKundeMenu();
                    break;
                }
                else
                {
                    Console.WriteLine("Du er ikke logged ind!");
                    Thread.Sleep(2000);
                    KørStartMenu();
                    break;
                }
            case 4:
                if (loggedIn)
                {
                    MedarbejderLogud();
                    break;
                }
                else
                {
                    MedarbejderLogin();
                    KørStartMenu();
                    break;
                }
            case 5:
                Console.WriteLine("\nProgrammet afsluttes...");
                Thread.Sleep(1000);
                Environment.Exit(0);
                break;
        }
    }

    private void AfslutGMS()
    {
        Console.WriteLine("\n Tryk på enhver tast for at afslutte programmet...");
        Console.ReadKey();
        Environment.Exit(0);
    }

    private void VisOmGMS()
    {
        Console.Clear();
        Console.WriteLine("Dette C# projekt er lavet af UCL-studerende Anders Vincent Danielsen, Cecilia Mølgaard Hafezan, Ilham Abbas Hashi, Rasmus Malmberg Christensen og Yousof Mohamed Fathi Ibrahim.");
        Console.WriteLine("Dato: 09-04-2025");
        Console.WriteLine("\n \nTryk på enhver tast for at vende tilbage til hovedmenuen.");
        Console.ReadKey(true);
        KørHovedMenu();
    }
    private void KørLagerMenu()
    {
        string prompt = "--- Lager ---\n";
        string[] menuPunkter = { "Tilføj spil", "Fjern spil", "Vis lager", "Søg i lager", "Tilbage" };
        Menu lagerMenu = new Menu(prompt, menuPunkter);
        int indexValgt = lagerMenu.Kør(); 

        switch (indexValgt)
            {
            case 0:
                Braetspil braetspil = BraetspilManager.Instance.OpretNytSpil();
                BraetspilManager.Instance.TilfoejSpil(braetspil);
                Thread.Sleep(2000);
                KørLagerMenu();
                break;

            case 1:
                BraetspilManager.Instance.SpilUdgået();
                Thread.Sleep(2000);
                KørLagerMenu();
                break;

            case 2:
                BraetspilManager.Instance.VisLager();
                Console.ReadKey(true);
                KørLagerMenu();
                break;

            case 3:
                BraetspilManager.Instance.SoegSpil();
                Console.ReadKey(true);
                KørLagerMenu();
                break;

            case 4:
                KørLagerMenu();
                break;
        }
        
    }

    public void MedarbejderLogin()
    {
        Console.WriteLine("Indtast adgangskode:");
        string kode = Console.ReadLine();
        if (kode == "1")
        {
            loggedIn = true;
            Console.WriteLine("Login succesfuldt!");
        }
        else
        {
            Console.WriteLine("Forkert adgangskode!");
        }
    }
    public void MedarbejderLogud()
    {
        loggedIn = false;
        Console.WriteLine("Logget ud!");
    }

    public void KørForespoergselMenu()
    {
        string prompt = "--- Forespørgsler ---\n";
        string[] menuPunkter = { "Tilføj forespørgsel", "Vis forespørgsler", "Rediger/Slet forespørgsel", "Tilbage" };
        Menu forespørgslerMenu = new Menu(prompt, menuPunkter);
        int indexValgt = forespørgslerMenu.Kør();

        switch (indexValgt)
        {
            case 0:
                TilføjForespørgsel();
                Thread.Sleep(2000);
                KørForespoergselMenu();
                break;
            case 1:
                VisForespoergsler();
                Console.ReadKey(true);
                KørForespoergselMenu();
                break;
            case 2:
                RedigerEllerSletForespoergsel();
                KørForespoergselMenu();
                break;
            case 3:
                KørStartMenu();
                break;

        }
    }

    private int GenererForespoergselsnummer()
    {
        if (forespoergsler.Count == 0) return 1;
        return forespoergsler.Max(f => f.ForespoergselNr) + 1;
    }
    private void TilføjForespørgsel()
    {
        Console.WriteLine("--- Tilføj En Forespørgsel --- \n\n");
        Console.Write("Indtast dato for forespørgsel (dd-mm-yyyy): ");
        string inputDato = Console.ReadLine();
        DateTime dato;

        while (!DateTime.TryParse(inputDato, out dato))
        {
            Console.WriteLine("Ugyldig dato. Prøv igen.");
            inputDato = Console.ReadLine();
        }

        Console.Write("Indtast ønsket spil: ");
        string spilNavn = Console.ReadLine();
        Console.Write("Indtast kundens navn: ");
        string kundeNavn = Console.ReadLine();
        Console.Write("Indtast kundens telefonnummer: ");
        string kundeTelefon = Console.ReadLine();
        Console.Write("Indtast kundens adresse: ");
        string kundeAdresse = Console.ReadLine();
        Console.Write("Indtast kundens email: ");
        string kundeEmail = Console.ReadLine();
        Console.Write("Indtast status: 1 = Afventer, 2 = Afsluttet, 3 = Annulleret: ");

        var kunde = kundeManager.OpretKunde(kundeNavn, kundeEmail, kundeAdresse, kundeTelefon);

        if (!int.TryParse(Console.ReadLine(), out int statusNum) || !Enum.IsDefined(typeof(ForespoergselsStatus), statusNum))
        {
            Console.WriteLine("Ugyldig status. Prøv igen.");
            statusNum = (int)ForespoergselsStatus.Afventer;
        }

        ForespoergselsStatus status = (ForespoergselsStatus)statusNum;

        int forespørgselsNr = GenererForespoergselsnummer();
        Forespoergsel forespørgsel = new Forespoergsel(forespørgselsNr, dato, spilNavn, status, kunde);
        forespoergsler.Add(forespørgsel);

        Console.WriteLine("Forespørgslen er oprettet! \n");
    }

    private void VisForespoergsler()
    {
        if (forespoergsler.Count == 0)
        {
            Console.WriteLine("Der er ingen forespørgsler");
            return;
        }
        else
        {
            foreach (var forespørgsel in forespoergsler)
            {
                Console.WriteLine($"Forespørgsel: {forespørgsel.ForespoergselNr}");
                Console.WriteLine($"Dato: {forespørgsel.Dato:dd-MM-yyyy}");
                Console.WriteLine($"Spil: {forespørgsel.SpilNavn}");
                Console.WriteLine($"Kunde: {forespørgsel.Kunde.Navn}");
                Console.WriteLine($"Status: {forespørgsel.Status}");
                Console.WriteLine("\nTryk på enhver knap for at gå tilbage.");
            }
        }   
    }

    private void RedigerEllerSletForespoergsel()
    {
        Console.WriteLine("Indtast forespørgselsnummer:");
        if (int.TryParse(Console.ReadLine(), out int forespørgselsNr))
        {
            var forespørgsel = forespoergsler.Find(f => f.ForespoergselNr == forespørgselsNr);
            if (forespørgsel == null)
            {
                Console.WriteLine("Forespørgslen blev ikke fundet.");
                Thread.Sleep(2000);
                KørForespoergselMenu();
                return;
            }

            string prompt = "--- Rediger/Slet Forespørgsel ---\n \n" +
                $"Rediger forespørgselsnr: {forespørgsel.ForespoergselNr}\n" +
                $"Dato: {forespørgsel.Dato:dd-MM-yyyy}\n" + 
                $"Spil: {forespørgsel.SpilNavn}\n" +
                $"Kunde: {forespørgsel.Kunde.Navn}\n" +
                $"Telefon: {forespørgsel.Kunde.TlfNr}\n" +
                $"Adresse: {forespørgsel.Kunde.Adresse}\n" +
                $"Email: {forespørgsel.Kunde.Email}\n" +
                $"Nuværende status: {forespørgsel.Status}\n";
            string[] menuPunkter = { "Rediger forespørgsel", "Slet forespørgsel", "Tilbage" };
            Menu redSletForespoergselMenu = new Menu(prompt, menuPunkter);
            int indexValgt = redSletForespoergselMenu.Kør();

            switch (indexValgt)
            {
                case 0:
                    RedigerForespoergsel(forespørgsel);
                    Thread.Sleep(2000);
                    KørForespoergselMenu();
                    break;
                case 1:
                    SletForespoergsel(forespørgselsNr);
                    Thread.Sleep(2000);
                    KørForespoergselMenu();
                    break;
                case 2:
                    Console.WriteLine("Annulleret");
                    Thread.Sleep(2000);
                    KørForespoergselMenu();
                    return;
            }
        }

    }
    private void RedigerForespoergsel(Forespoergsel forespørgsel)
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
    private void SletForespoergsel(int forespørgselsNr)
    {
        var forespørgsel = forespoergsler.Find(f => f.ForespoergselNr == forespørgselsNr);
        if (forespørgsel == null)
        {
            Console.WriteLine("Forespørgslen blev ikke fundet.");
        }
        else
        {
            Console.WriteLine($"Rediger forespørgsel: {forespørgsel.ForespoergselNr}");
            Console.WriteLine($"Dato: {forespørgsel.Dato:dd-MM-yyyy}");
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
    public void KørReservationMenu()
    {
        string prompt = "--- Reservationer ---\n";
        string[] menuPunkter = { "Tilføj reservation", "Vis reservationer", "Rediger/Slet reservation", "Tilbage" };
        Menu reservationerMenu = new Menu(prompt, menuPunkter);
        int indexValgt = reservationerMenu.Kør();

        switch (indexValgt)
        {
            case 0:
                TilføjReservation();
                Thread.Sleep(2000);
                KørReservationMenu();
                break;
            case 1:
                VisReservationer();
                Console.ReadKey(true);
                KørReservationMenu();
                break;
            case 2:
                RedigerEllerSletReservation();
                KørReservationMenu();
                break;
            case 3:
                KørStartMenu();
                break;

        }
    }

    private int GenererAfhentningsNr()
    {
        if (reservationer.Count == 0) return 1;
        return reservationer.Max(f => f.AfhentningsNr) + 1;
    }

    private void TilføjReservation()
    {
        Console.WriteLine("Tilføj En Reservation \n\n");
        DateTime reservationsDato = DateTime.Now;
        Console.Write("Indtast dato for afhentning (dd-mm-åååå): ");
        string inputDato = Console.ReadLine();
        DateTime afhentningsDato;
        while (!DateTime.TryParse(inputDato, out afhentningsDato))
        {
            Console.WriteLine("Ugyldig dato. Prøv igen.");
            inputDato = Console.ReadLine();
        }
        int afhentningsNr = GenererAfhentningsNr();
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
        var kunde = kundeManager.OpretKunde(kundeNavn, kundeEmail, kundeAdresse, kundeTelefon);

        Reservation reservation = new Reservation(afhentningsNr, reservationsDato, afhentningsDato, kunde, braetspil, status);
        reservationer.Add(reservation);
        Console.WriteLine("Reservationen er oprettet!");
    }

    private void VisReservationer()
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
                Console.WriteLine($"Spil: {reservation.Braetspil.Version}");
                Console.WriteLine($"Kunde: {reservation.Kunde.Navn}");
                Console.WriteLine($"Status: {reservation.Status}");
                Console.WriteLine("\nTryk på enhver knap for at gå tilbage.");
            }
        }   
    }

    private void RedigerEllerSletReservation()
    {
        Console.WriteLine("Indtast afhentningsnr.:");
        if (int.TryParse(Console.ReadLine(), out int afhentningsNr))
        {
            var reservation = reservationer.Find(f => f.AfhentningsNr == afhentningsNr);
            if (reservation == null)
            {
                Console.WriteLine("Reservationen blev ikke fundet.");
                Thread.Sleep(2000);
                KørReservationMenu();
                return;
            }

            string prompt = "--- Rediger/Slet Reservation ---\n \n" +
                $"Rediger afhentningsnr.: {reservation.AfhentningsNr}\n" +
                $"Afhentningsdato: {reservation.AfhentningsDato.ToShortDateString()}\n" + 
                $"Spil: {reservation.Braetspil.Navn}\n" +
                $"Spil: {reservation.Braetspil.Version}\n" +
                $"Kunde: {reservation.Kunde.Navn}\n" +
                $"Telefon: {reservation.Kunde.TlfNr}\n" +
                $"Adresse: {reservation.Kunde.Adresse}\n" +
                $"Email: {reservation.Kunde.Email}\n" +
                $"Nuværende status: {reservation.Status}\n";
            string[] menuPunkter = { "Rediger reservation", "Slet reservation", "Tilbage" };
            Menu redSletReservationMenu = new Menu(prompt, menuPunkter);
            int indexValgt = redSletReservationMenu.Kør();

            switch (indexValgt)
            {
                case 0:
                    RedigerReservation(reservation);
                    Thread.Sleep(2000);
                    KørReservationMenu();
                    break;
                case 1:
                    SletReservation(afhentningsNr);
                    Thread.Sleep(2000);
                    KørReservationMenu();
                    break;
                case 2:
                    Console.WriteLine("Annulleret");
                    Thread.Sleep(2000);
                    KørReservationMenu();
                    return;
            }
        }

    }
    private void RedigerReservation(Reservation reservation)
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
    private void SletReservation(int afhentningsNr)
    {
        var reservation = reservationer.Find(f => f.AfhentningsNr == afhentningsNr);
        if (reservation == null)
        {
            Console.WriteLine("Reservationen blev ikke fundet.");
        }
        else
        {
            Console.WriteLine($"Rediger afhentningsnr.: {reservation.AfhentningsNr}");
            Console.WriteLine($"Afhentningsdato: {reservation.AfhentningsDato.ToShortDateString()}\n");
            Console.WriteLine($"Spil: {reservation.Braetspil.Navn}\n");
            Console.WriteLine($"Kunde: {reservation.Kunde.Navn}\n");
            Console.WriteLine($"Telefon: {reservation.Kunde.TlfNr}");
            Console.WriteLine($"Adresse: {reservation.Kunde.Adresse}\n");
            Console.WriteLine($"Email: {reservation.Kunde.Email}\n");
            Console.WriteLine($"Nuværende status: {reservation.Status}\n");

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

    private void KørKundeMenu()
    {
        string prompt = "--- Kundedata ---\n\n";
        string[] menuPunkter = { "Opret kunde", "Find kunde", "Opdater kunde", "Slet kunde", "Vis alle kunder", "Tilbage"};
        Menu kundeMenu = new Menu(prompt, menuPunkter);
        int indexValgt = kundeMenu.Kør();

        switch (indexValgt)
        {
            case 0:
                Console.Write("Indtast kundens navn: ");
                string kundeNavn = Console.ReadLine();
                Console.Write("Indtast kundens telefonnummer: ");
                string kundeTelefon = Console.ReadLine();
                Console.Write("Indtast kundens adresse: ");
                string kundeAdresse = Console.ReadLine();
                Console.Write("Indtast kundens email: ");
                string kundeEmail = Console.ReadLine();
                kundeManager.OpretKunde(kundeNavn, kundeEmail, kundeAdresse, kundeTelefon);
                Console.ReadKey();
                KørKundeMenu();
                break;

            case 1:
                Console.Write("Indtast kundeNr.: ");
                if (int.TryParse(Console.ReadLine(), out int kundeId))
                {
                    var kunde = kundeManager.FindKunde(kundeId);
                    if (kunde != null)
                        Console.WriteLine($"Navn: {kunde.Navn}, Email: {kunde.Email}, Adresse: {kunde.Adresse}");
                    else
                        Console.WriteLine("Kunde ikke fundet.");
                }
                Console.ReadKey();
                KørKundeMenu();
                break;

            case 2:
                Console.Write("Indtast kundeNr.: ");
                if (int.TryParse(Console.ReadLine(), out int opdaterId))
                {
                    Console.Write("Nyt navn: ");
                    string nytNavn = Console.ReadLine();
                    Console.Write("Ny email: ");
                    string nyEmail = Console.ReadLine();
                    Console.Write("Ny adresse: ");
                    string nyAdresse = Console.ReadLine();
                    Console.Write("Nyt telefonnummer: ");
                    string nyTlfNr = Console.ReadLine();
                    kundeManager.OpdaterKunde(opdaterId, nytNavn, nyEmail, nyAdresse, nyTlfNr);
                }
                Console.ReadKey();
                KørKundeMenu();
                break;

            case 3:
                Console.Write("Indtast kundeNr.: ");
                if (int.TryParse(Console.ReadLine(), out int sletId))
                {
                    kundeManager.SletKunde(sletId);
                }
                Console.ReadKey();
                KørKundeMenu();
                break;

            case 4:
                kundeManager.VisAlleKunder();
                Console.ReadKey();
                KørKundeMenu();
                break;

            case 5:
                KørStartMenu();
                break;

        }
    }
}
