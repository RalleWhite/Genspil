using System;
using System.Reflection.Emit;
using System.Reflection;
using System.Runtime.CompilerServices;

class GenspilManagementSystem
{
    private static bool loggedIn = false;

    public void Kør()
    {
        JSONHelper.LoadAll();
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
        string[] menuPunkter = { "Lager", "Reservationer", "Forespørgsler", "Kundedata", "Butikslogin", "Afslut" };
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
                KørMedarbejderMenu();
                break;
            case 5:
                Console.WriteLine("\nProgrammet afsluttes...");
                JSONHelper.SaveAll();
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
        Console.WriteLine("Dette C# projekt er lavet af UCL-studerende: Anders Vincent Danielsen, Cecilia Mølgaard Hafezan, Ilham Abbas Hashi, Rasmus Malmberg Christensen og Yousof Mohamed Fathi Ibrahim.");
        Console.WriteLine("Dato: 09-04-2025");
        Console.WriteLine("\n \nTryk på enhver tast for at vende tilbage til hovedmenuen.");
        Console.ReadKey(true);
        KørHovedMenu();
    }
    private void KørLagerMenu()
    {
        JSONHelper.SaveAll();
        string prompt = "--- Lager ---\n";
        string[] menuPunkter = { "Tilføj spil", "Opdater spil", "Fjern spil", "Vis lager", "Søg i lager", "Tilbage" };
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
                Braetspil spil = BraetspilManager.Instance.OpretSpilOpdatering();
                BraetspilManager.Instance.OpdaterSpil(spil);
                Thread.Sleep(2000);
                KørLagerMenu();
                break;
            case 2:
                BraetspilManager.Instance.SpilUdgået();
                Thread.Sleep(2000);
                KørLagerMenu();
                break;
            case 3:
                BraetspilManager.Instance.VisLager();
                Console.ReadKey(true);
                KørLagerMenu();
                break;

            case 4:
                BraetspilManager.Instance.SoegSpil();
                Console.ReadKey(true);
                KørLagerMenu();
                break;

            case 5:
                KørStartMenu();
                break;
        }

    }

    private void KørMedarbejderMenu()
    {
        JSONHelper.SaveAll();
        string prompt = "--- MedarbejderLogin ---\n";
        string[] menuPunkter = { "Login/Logud", "Tilføj medarbejder", "Tilbage" };
        Menu medarbejderMenu = new Menu(prompt, menuPunkter);
        int indexValgt = medarbejderMenu.Kør();

        switch (indexValgt)
        {
            case 0:
                if (loggedIn)
                {
                    MedarbejderLogud();
                    break;
                }
                else
                {
                    LoginMenu();
                }
                break;
            case 1:
                if (loggedIn)
                {
                    MedarbejderManager.Instance.VisMedarbejdere();
                    MedarbejderManager.Instance.TilføjNyMedarbejder();
                    Thread.Sleep(2000);
                    KørMedarbejderMenu();
                }
                else
                {
                    Console.WriteLine("Du er ikke logged ind!");
                    Thread.Sleep(2000);
                    KørMedarbejderMenu();
                }
                break;
            case 2:
                KørStartMenu();
                break;
        }
    }

    private void LoginMenu()
    {
        Console.Write("Indtast adgangskode: ");
        string kode = Console.ReadLine();
        if (kode == "1")
        {
            loggedIn = true;
            Console.WriteLine("Login succesfuldt!");
            Thread.Sleep(2000);
            KørStartMenu();
        }
        else
        {
            Console.WriteLine("Forkert adgangskode!");
            Thread.Sleep(1000);
            KørMedarbejderMenu();
        }
    }
    private void MedarbejderLogud()
    {
        loggedIn = false;
        Console.WriteLine("Logget ud!");
        Thread.Sleep(2000);
        KørStartMenu();
    }

    private void KørForespoergselMenu()
    {
        JSONHelper.SaveAll();
        string prompt = "--- Forespørgsler ---\n";
        string[] menuPunkter = { "Tilføj forespørgsel", "Vis forespørgsler", "Opdater/Slet forespørgsel", "Tilbage" };
        Menu forespørgslerMenu = new Menu(prompt, menuPunkter);
        int indexValgt = forespørgslerMenu.Kør();

        switch (indexValgt)
        {
            case 0:
                ForespoergselManager.Instance.TilfoejForespoergsel(ForespoergselManager.Instance.OpretNyForespoergsel());
                Thread.Sleep(2000);
                KørForespoergselMenu();
                break;
            case 1:
                ForespoergselManager.Instance.VisForespoergsler();
                Console.ReadKey(true);
                KørForespoergselMenu();
                break;
            case 2:
                RedSletForespoergselMenu();
                KørForespoergselMenu();
                break;
            case 3:
                KørStartMenu();
                break;

        }
    }

    private void RedSletForespoergselMenu()
    {
        string prompt = "--- Opdater/Slet Forespørgsel ---\n";
        string[] menuPunkter = { "Opdater status", "Slet forespørgsel", "Tilbage" };
        Menu redSletForespoergselMenu = new Menu(prompt, menuPunkter);
        int indexValgt = redSletForespoergselMenu.Kør();

        switch (indexValgt)
        {
            case 0:

                Console.Write("Indtast forespørgselsnummer: ");
                if (int.TryParse(Console.ReadLine(), out int forespørgselsNr))
                {
                    var forespørgsel = ForespoergselManager.forespoergsler.Find(f => f.ForespoergselNr == forespørgselsNr);
                    if (forespørgsel == null)
                    {
                        Console.WriteLine("Forespørgslen blev ikke fundet.");
                        Thread.Sleep(2000);
                        KørForespoergselMenu();
                        return;
                    }
                    else
                    {
                        ForespoergselManager.Instance.VisForespoergsel(forespørgsel);
                        ForespoergselManager.Instance.OpdaterForespoergselStatus(forespørgsel);
                        Thread.Sleep(2000);
                        RedSletForespoergselMenu();
                    }
                }
                break;
            case 1:

                Console.Write("Indtast forespørgselsnummer: ");
                if (int.TryParse(Console.ReadLine(), out int forespørgselsNrR))
                {
                    var forespørgsel = ForespoergselManager.forespoergsler.Find(f => f.ForespoergselNr == forespørgselsNrR);
                    if (forespørgsel == null)
                    {
                        Console.WriteLine("Forespørgslen blev ikke fundet.");
                        Thread.Sleep(2000);
                        KørForespoergselMenu();
                        return;
                    }
                    else
                    {
                        ForespoergselManager.Instance.VisForespoergsel(forespørgsel);
                        ForespoergselManager.Instance.SletForespoergsel(forespørgselsNrR);
                        Thread.Sleep(2000);
                        RedSletForespoergselMenu();
                    }
                }
                break;
            case 2:
                KørForespoergselMenu();
                return;
        }
    }
    private void KørReservationMenu()
    {
        JSONHelper.SaveAll();
        string prompt = "--- Reservationer ---\n";
        string[] menuPunkter = { "Tilføj reservation", "Vis reservationer", "Opdater/Slet reservation", "Tilbage" };
        Menu reservationerMenu = new Menu(prompt, menuPunkter);
        int indexValgt = reservationerMenu.Kør();

        switch (indexValgt)
        {
            case 0:
                ReservationManager.Instance.TilfoejReservation(ReservationManager.Instance.OpretNyReservation());
                Thread.Sleep(2000);
                KørReservationMenu();
                break;
            case 1:
                ReservationManager.Instance.VisReservationer();
                Console.ReadKey(true);
                KørReservationMenu();
                break;
            case 2:
                RedSletReservationMenu();
                KørReservationMenu();
                break;
            case 3:
                KørStartMenu();
                break;

        }
    }

    private void RedSletReservationMenu()
    {
        string prompt = "--- Opdater/Slet Reservation ---\n";
        string[] menuPunkter = {"Opdater status", "Slet reservation", "Tilbage"};
        Menu redSletReservationMenu = new Menu(prompt, menuPunkter);
        int indexValgt = redSletReservationMenu.Kør();

        switch (indexValgt)
        {
            case 0:
                Console.Write("Indtast afhentningsnr.: ");
                if (int.TryParse(Console.ReadLine(), out int afhentningsNr))
                {
                    var reservation = ReservationManager.reservationer.Find(f => f.AfhentningsNr == afhentningsNr);
                    if (reservation == null)
                    {
                        Console.WriteLine("Reservationen blev ikke fundet.");
                        Thread.Sleep(2000);
                        RedSletReservationMenu();
                        return;
                    }
                    else
                    {
                        ReservationManager.Instance.VisReservation(reservation);
                        ReservationManager.Instance.OpdaterReservationStatus(reservation);
                        Thread.Sleep(2000);
                        RedSletReservationMenu();
                    }
                }
                break;

            case 1:
                Console.Write("Indtast afhentningsnr.: ");
                if (int.TryParse(Console.ReadLine(), out int afhentningsNrR))
                {
                    var reservation = ReservationManager.reservationer.Find(f => f.AfhentningsNr == afhentningsNrR);
                    if (reservation == null)
                    {
                        Console.WriteLine("Reservationen blev ikke fundet.");
                        Thread.Sleep(2000);
                        RedSletReservationMenu();
                        return;
                    }
                    else
                    {
                        ReservationManager.Instance.VisReservation(reservation);
                        ReservationManager.Instance.SletReservation(afhentningsNrR);
                        Thread.Sleep(2000);
                        RedSletReservationMenu();
                    }
                }
                break;

            case 2:
                KørReservationMenu();
                break;
        }
    }

    private void KørKundeMenu()
    {
        JSONHelper.SaveAll();
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
                KundeManager.Instance.OpretKunde(kundeNavn, kundeEmail, kundeAdresse, kundeTelefon);
                Thread.Sleep(2000);
                KørKundeMenu();
                break;

            case 1:
                Console.Write("Indtast kundeNr.: ");
                if (int.TryParse(Console.ReadLine(), out int kundeId))
                {
                    var kunde = KundeManager.Instance.FindKunde(kundeId);
                    if (kunde != null)
                        Console.WriteLine($"Navn: {kunde.Navn}, Email: {kunde.Email}, Adresse: {kunde.Adresse}");
                    else
                        Console.WriteLine("Kunde ikke fundet.");
                }
                Console.ReadKey();
                KørKundeMenu();
                break;

            case 2: // Evt. lav opdateringsprocess det samme som braetspils update
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
                    KundeManager.Instance.OpdaterKunde(opdaterId, nytNavn, nyEmail, nyAdresse, nyTlfNr);
                }
                Thread.Sleep(2000);
                KørKundeMenu();
                break;

            case 3:
                Console.Write("Indtast kundeNr.: ");
                if (int.TryParse(Console.ReadLine(), out int sletId))
                {
                    KundeManager.Instance.SletKunde(sletId);
                }
                Thread.Sleep(2000);
                KørKundeMenu();
                break;

            case 4:
                KundeManager.Instance.VisAlleKunder();
                Console.ReadKey();
                KørKundeMenu();
                break;

            case 5:
                KørStartMenu();
                break;

        }
    }
}
