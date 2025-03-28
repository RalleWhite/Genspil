using System;
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
                        if (loggedIn){
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
            Console.WriteLine("Forespørgsler vises her");
            Thread.Sleep(3000);
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