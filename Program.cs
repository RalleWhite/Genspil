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

        static void Main(string[] args)
        {
            StartMenu();
        }

        static void StartMenu()
        {
            Console.ResetColor();
            Console.Clear();
            Console.WriteLine("\nGenspil Management System\nDu har nu følgende muligheder");
            Console.WriteLine("1. Lagerstatus  |  2. Reservationer  |  3. Forespørgsel");
            Console.WriteLine("4. Medarbejder login |  5. Medarbejder logud  |  6. Afslut\n");
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
                            Console.WriteLine("Du er allerede logged ind!");
                            Thread.Sleep(3000);
                            StartMenu();
                            break;
                        }
                        else
                        {
                            MedarbejderLogin();
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
                            Console.WriteLine("Du er ikke logged ind!");
                            Thread.Sleep(3000);
                            StartMenu();
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
            Console.WriteLine("1. Tilføj spil | 2. Fjern spil | 3. Vis lager | 4. Tilbage");
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

                        BraetspilManager.Instance.TilfoejSpil(new Braetspil(navn, version, stand, pris, genre, antalSpillere));
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

            if(!int.TryParse(Console.ReadLine(), out int statusNum) || !Enum.IsDefined(typeof(ForespoergselsStatus), statusNum))
            {
                Console.WriteLine("Ugyldig status. Prøv igen.");
                statusNum = (int)ForespoergselsStatus.Afventer;
            }

            ForespoergselsStatus status = (ForespoergselsStatus)statusNum;

            Kunde kunde = new Kunde(kundeNavn, kundeAdresse, kundeTelefon, kundeEmail);

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