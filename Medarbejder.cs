using System.Reflection.Emit;
using System.Security.Cryptography;

internal class Medarbejder
{
    public string Navn {  get; set; }
    public string BrugerNavn {  get; set; }

    public Medarbejder(string navn, string brugernavn)
    {
        Navn = navn;
        BrugerNavn = brugernavn;
    }
}

internal class MedarbejderManager
{
    private static MedarbejderManager _instance;
    public List<Medarbejder> medarbejdere = new List<Medarbejder>();
    private bool _defaultemployeesAdded = false;

    public static MedarbejderManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new MedarbejderManager();
            return _instance;
        }
    }

    public void TilfoejDefaultMedarbejdere()
    {
        if (_defaultemployeesAdded) return;
        var medarbejdereData = new List<(string Navn, string Brugernavn)>
            {
                ("Jonas Nielsen", "joni01"),
                ("Jamal Muhammed", "jamu02"),
                ("Emilie Petersen","empe03")
            };
        foreach (var medarbejderData in medarbejdereData)
        {
            Medarbejder medarbejder = new Medarbejder(medarbejderData.Navn, medarbejderData.Brugernavn);
            medarbejdere.Add(medarbejder);
        }
    }
    public void VisMedarbejdere()
    {
        if (medarbejdere.Count == 0)
        {
            Console.WriteLine("Ingen registrerede medarbejdere!");
            return;
        }

        Console.WriteLine("\nMedarbejdere:\n");
        foreach (var medarbejder in medarbejdere)
        {
            Console.WriteLine($"Navn: {medarbejder.Navn}, Brugernavn: {medarbejder.BrugerNavn}");
        }
        Console.ResetColor();
    }

    public void TilføjNyMedarbejder()
    {
        Console.Write("Indtast medarbejder fornavn: ");
        string fornavn = Console.ReadLine();
        Console.Write("Indtast medarbejder efternavn: ");
        string efternavn = Console.ReadLine();
        string navn = fornavn + " " + efternavn;
        Random rd = new Random();
        string brugernavn = (fornavn[0].ToString() + fornavn[1].ToString() + efternavn[0].ToString() + efternavn[1].ToString()).ToLower() + rd.Next(10,99).ToString();

        Medarbejder medarbejder = new Medarbejder(navn, brugernavn);
        medarbejdere.Add(medarbejder);
        Console.WriteLine("Medarbejder Oprettet: " + brugernavn);
    }
}


