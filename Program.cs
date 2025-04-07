using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Genspil
{
    internal class Program
    {
        private static List<Reservation> reservationer = new List<Reservation>();
        private static List<Forespoergsel> forespoergsler = new List<Forespoergsel>();
        private static bool loggedIn = false;
        static KundeManager kundeManager = new KundeManager();

        static void Main(string[] args)
        {
            BraetspilManager.Instance.TilfoejDefaultSpil();
            StartMenu();
        }

        static void StartMenu()
        {
            Console.ResetColor();
            Console.Clear();
            Console.WriteLine("\nGenspil Management System\nDu har nu følgende muligheder");
            Console.WriteLine("1. Lagerstatus  |  2. Reservationer         |  3. Forespørgsel");
            Console.WriteLine("4. Kundedata    |  5. Medarbejder login/ud  |  6. Afslut\n");
            Console.WriteLine("Indtast hvilken funktion du vil køre:");

            if (int.TryParse(Console.ReadLine(), out int taskNum))
            {
                switch (taskNum)
                {
                    case 1:
                        if (loggedIn) {
                            LagerMenu();
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Du er ikke logged ind!");
                            Thread.Sleep(3000);
                            StartMenu();
                            break;
                        }
                    case 2:
                        if (loggedIn)
                        {
                            ReservationMenu();
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Du er ikke logged ind!");
                            Thread.Sleep(3000);
                            StartMenu();
                            break;
                        }
                    case 3:
                        if (loggedIn)
                        {
                            ForespoergselMenu();
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Du er ikke logged ind!");
                            Thread.Sleep(3000);
                            StartMenu();
                            break;
                        }
                    case 4:
                        if (loggedIn)
                        {
                            KundeMenu();
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Du er ikke logged ind!");
                            Thread.Sleep(3000);
                            StartMenu();
                            break;
                        }
                    case 5:
                        if (loggedIn)
                        {
                            MedarbejderLogud();
                            break;
                        }
                        else
                        {
                            MedarbejderLogin();
                            break;
                        }
                    case 6:
                        Console.WriteLine("\nProgrammet afsluttes...");
                        Thread.Sleep(2000);
                        Environment.Exit(0);
                        break;
                    default:
                        Fejlmeddelelse();
                        break;
                }
            }
            else
            {
                Fejlmeddelelse();
            }
        }

        static void LagerMenu()
        {
            Console.WriteLine("1. Tilføj spil | 2. Fjern spil | 3. Vis lager | 4. Søg i Lager | 6. Tilbage");
            if (int.TryParse(Console.ReadLine(), out int valg))
            {
                switch (valg)
                {
                    case 1:
                        Console.WriteLine("Indtast navn:");
                        string navn = Console.ReadLine();
                        Console.WriteLine("Indtast version:");
                        string version = Console.ReadLine();
                        Console.WriteLine("Indtast stand:");
                        string stand = Console.ReadLine();
                        Console.WriteLine("Indtast pris:");
                        double pris = Convert.ToDouble(Console.ReadLine());
                        Console.WriteLine("Indtast genre:");
                        string genre = Console.ReadLine();
                        Console.WriteLine("Indtast antal spillere:");
                        string antalSpillere = Console.ReadLine();
                        Console.WriteLine("Indtast lagerstatus: 1 = På lager, 2 = Bestilt hjem");
                        int statusInt = int.Parse(Console.ReadLine());
                        Lagerstatus lagerstatus = (Lagerstatus)(statusInt - 1);

                        BraetspilManager.Instance.TilfoejSpil(new Braetspil(navn, version, stand, pris, genre, antalSpillere, lagerstatus));
                        Thread.Sleep(2000);
                        StartMenu();
                        break;

                    case 2:
                        foreach (var spil in BraetspilManager.Instance.HentLager())
                        {
                            Console.WriteLine(spil.Navn);
                        }
                        Console.WriteLine("Indtast navn på spil der skal fjernes:");
                        string spilNavn = Console.ReadLine();
                        BraetspilManager.Instance.FjernSpil(spilNavn);
                        Thread.Sleep(2000);
                        StartMenu();
                        break;

                    case 3:
                        BraetspilManager.Instance.VisLager();
                        Console.ReadLine();
                        StartMenu();
                        break;

                    case 4:
                        Console.WriteLine("Søg efter et brætspil:");

                        Console.WriteLine("Indtast navnet på spillet (eller tryk Enter for at springe over):");
                        string searchTerm = Console.ReadLine();

                        Console.WriteLine("Indtast genre (eller tryk Enter for at springe over):");
                        string searchGenre = Console.ReadLine();

                        Console.WriteLine("Indtast version (eller tryk Enter for at springe over):");
                        string searchVersion = Console.ReadLine();

                        Console.WriteLine("Indtast stand (eller tryk Enter for at springe over):");
                        string searchStand = Console.ReadLine();

                        Console.WriteLine("Indtast antal spillere (eller tryk Enter for at springe over):");
                        string searchAntalSpillere = Console.ReadLine();

                        Console.WriteLine("Indtast lagerstatus (1 = PåLager, 2 = BestiltHjem, 3 = Udgået, eller tryk Enter for at springe over):");
                        string statusInput = Console.ReadLine();
                        Lagerstatus? status = null;

                        if (int.TryParse(statusInput, out int statusChoice))
                        {
                            status = (Lagerstatus)(statusChoice - 1);
                        }

                        List<Braetspil> searchResults = BraetspilManager.Instance.SoegSpil(searchTerm, searchGenre, status, searchVersion, searchStand, searchAntalSpillere);

                        if (searchResults.Count > 0)
                        {
                            Console.WriteLine("Søgeresultater:");
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
                        StartMenu();
                        break;

                    case 5:
                        StartMenu();
                        break;

                    default:
                        Fejlmeddelelse();
                        break;
                }
            }
            else
            {
                Fejlmeddelelse();
            }
        }

        static void KundeMenu()
        {
            Console.WriteLine("1. Opret kunde  |  2. Find Kunde         |  3. Opdater Kunde");
            Console.WriteLine("4. Slet Kunde    |  5. Vis Alle Kunder  |  6. Afslut\n");
            Console.WriteLine("Vælg en handling:");

            if (int.TryParse(Console.ReadLine(), out int kvalg))
            {
                switch (kvalg)
                {
                    case 1:
                        Console.Write("Navn: ");
                        string navn = Console.ReadLine();
                        Console.Write("Email: ");
                        string email = Console.ReadLine();
                        Console.Write("Adresse: ");
                        string adresse = Console.ReadLine();
                        Console.Write("Telefonnummer: ");
                        string tlfNr = Console.ReadLine();
                        kundeManager.OpretKunde(navn, email, adresse, tlfNr);
                        break;

                    case 2:
                        Console.Write("Indtast kunde ID: ");
                        if (int.TryParse(Console.ReadLine(), out int kundeId))
                        {
                            var kunde = kundeManager.FindKunde(kundeId);
                            if (kunde != null)
                                Console.WriteLine($"Navn: {kunde.Navn}, Email: {kunde.Email}, Adresse: {kunde.Adresse}");
                            else
                                Console.WriteLine("Kunde ikke fundet.");
                        }
                        break;

                    case 3:
                        Console.Write("Indtast kunde ID: ");
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
                        break;

                    case 4:
                        Console.Write("Indtast kunde ID: ");
                        if (int.TryParse(Console.ReadLine(), out int sletId))
                        {
                            kundeManager.SletKunde(sletId);
                        }
                        break;

                    case 5:
                        kundeManager.VisAlleKunder();
                        Console.ReadLine();
                        break;

                    case 6:
                        Console.WriteLine("\nProgrammet afsluttes...");
                        Thread.Sleep(2000);
                        Environment.Exit(0);
                        break;

                    default:
                        Fejlmeddelelse();
                        break;
                }
            }
            else
            {
                Fejlmeddelelse();

            }
            StartMenu();
        }

        static void ReservationMenu()
        {
            Console.WriteLine("Reservationer vises her");
            Thread.Sleep(3000);
            StartMenu();
        }

        static void ForespoergselMenu()
        {
            Console.WriteLine("1. Tilføj forespørgsel | 2. Vis forespørgsler | 3. Rediger/ slet forespørgsel | 4. Afslut");
            if (int.TryParse(Console.ReadLine(), out int valg))
            {
                switch (valg)
                {
                    case 1:
                        TilføjForespørgsel();
                        break;
                    case 2:
                        VisForespørgsler();
                        break;
                    case 3:
                        RedigerEllerSletForespoergsel();
                        break;
                    case 4:
                        StartMenu();
                        break;
                    default:
                        Fejlmeddelelse();
                        break;

                }
            }
            else
            {
                Fejlmeddelelse();

            }
            StartMenu();
        }

        static int GenererForespoergselsnummer()
        {
            if (forespoergsler.Count == 0) return 1;

            return forespoergsler.Max(f => f.ForespoergselNr) + 1;
            
        }

        static void TilføjForespørgsel()
        {
            Console.WriteLine("Tilføj en forespørgsel");
            Console.WriteLine();
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

            //Kunde kunde = new Kunde(kundeNavn, kundeAdresse, kundeTelefon, kundeEmail);

            int forespørgselsNr = GenererForespoergselsnummer();
            Forespoergsel forespørgsel = new Forespoergsel(forespørgselsNr, dato, spilNavn, status, kunde);

            forespoergsler.Add(forespørgsel);

            

            Console.WriteLine("Forespørgslen blev tilføjet!");

            Console.WriteLine();

            Console.WriteLine($"Antal forespørgsler i listen: {forespoergsler.Count}");

            Thread.Sleep(2000);
            StartMenu();
        }

        static void VisForespørgsler()
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
                    Console.WriteLine();
                }
            }
            

            Console.Write("1. Tilbage til hovedmenuen | 2. Tilbage til forespørgselsmenuen: ");
            if (int.TryParse(Console.ReadLine(), out int valg))
            {
                switch (valg)
                {
                    case 1:
                        StartMenu();
                        break;
                    case 2:
                        Console.Clear();    
                        ForespoergselMenu();
                        break;
                    default:
                        Fejlmeddelelse();
                        break;

                }
            }
            else
            {
                Fejlmeddelelse();

            }
            StartMenu();
        }


        static void RedigerEllerSletForespoergsel()
        {
            Console.WriteLine("Indtast forespørgselsnummer:");

            if (int.TryParse(Console.ReadLine(), out int forespørgselsNr))
            {
                var forespørgsel = forespoergsler.Find(f => f.ForespoergselNr == forespørgselsNr);
                if (forespørgsel == null)
                {
                    Console.WriteLine("Forespørgslen blev ikke fundet.");
                    Thread.Sleep(2000);
                    StartMenu();
                    return;
                }

                
                Console.WriteLine($"Rediger forespørgsel: {forespørgsel.ForespoergselNr}");
                Console.WriteLine($"Dato: {forespørgsel.Dato:dd-MM-yyyy}");
                Console.WriteLine($"Spil: {forespørgsel.SpilNavn}");
                Console.WriteLine($"Kunde: {forespørgsel.Kunde.Navn}");
                Console.WriteLine($"Telefon: {forespørgsel.Kunde.TlfNr}");
                Console.WriteLine($"Adresse: {forespørgsel.Kunde.Adresse}");
                Console.WriteLine($"Email: {forespørgsel.Kunde.Email}");
                Console.WriteLine($"Nuværende status: {forespørgsel.Status}");
                
                Console.Write("1. Rediger forespørgsel | 2. Slet forespørgsel | 3. Tilbage: ");
                string valg = Console.ReadLine();

                switch (valg)
                {
                    case "1":
                        RedigerForespoergsel(forespørgsel); 
                        break;
                    case "2":
                        SletForespoergsel(forespørgselsNr);
                        break;
                    case "3":
                        Console.WriteLine("Annulleret");
                        Thread.Sleep(2000);
                        return; 
                    default:
                        Fejlmeddelelse();
                        break;
                }

            }


        }

        static void RedigerForespoergsel(Forespoergsel forespørgsel)

        {
                        
            
            Console.Write("Indtast ny status: 1 = Afventer, 2 = Afsluttet, 3 = Annulleret: ");

            if (int.TryParse(Console.ReadLine(), out int statusNum) && Enum.IsDefined(typeof(ForespoergselsStatus), statusNum))
            {
                forespørgsel.Status = (ForespoergselsStatus)statusNum;
                Console.WriteLine("Status for forespørgslen er opdateret!");
            }
            else
            {
                Console.WriteLine("Status for forespørgslen er ikke opdateret!");
            }
            Thread.Sleep(2000);
            StartMenu();
        }

        static void SletForespoergsel(int forespørgselsNr)
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
            Thread.Sleep(2000);
            StartMenu();
        }



        static void MedarbejderLogin()
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
            Thread.Sleep(2000);
            StartMenu();
        }

        static void MedarbejderLogud()
        {
            loggedIn = false;
            Console.WriteLine("Logget ud!");
            Thread.Sleep(2000);
            StartMenu();
        }

        static void Fejlmeddelelse()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nUgyldigt input. Prøv igen.");
            Thread.Sleep(2000);
            StartMenu();
        }

        
    }





    
}