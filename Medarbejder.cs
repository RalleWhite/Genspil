using System.Reflection.Emit;
using System.Security.Cryptography;

internal class Medarbejder
{
    public string Navn {  get; set; }
    public string Brugernavn {  get; set; }

    public Medarbejder(string navn, string brugernavn)
    {
        Navn = navn;
        Brugernavn = brugernavn;
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
        string jamal = "jamu01";
        string jonas = "jona02";
        string emilie = "empe03";

        var medarbejdereData = new List<(string Navn, string Brugernavn)>
            {
                ("Jonas Nielsen", jonas),
                ("Jamal Mohammed", jamal),
                ("Emilie Petersen",emilie)
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
        for (int i = 0; i < medarbejdere.Count; i++)
        {
            var m = medarbejdere[i];
            Console.WriteLine($"{i + 1}. Navn: {m.Navn}, Brugernavn: {m.Brugernavn}");
            Console.ResetColor();
        }
    }


    public Medarbejder TilføjNyMedarbejder()
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

        return medarbejder;
    }

    public int VaelgMedarbejder()
    {
        if (medarbejdere.Count == 0)
        {
            Console.WriteLine("Ingen medarbejdere tilgængelige. Tilføj en medarbejder først.");
            return -1;
        }

        int index = -1;
        bool gyldigtValg = false;

        while (!gyldigtValg)
        {
            VisMedarbejdere();
            Console.Write("Indtast nummer på medarbejder: ");
            if (int.TryParse(Console.ReadLine(), out int valg) &&
                valg > 0 && valg <= medarbejdere.Count)
            {
                index = valg - 1;
                gyldigtValg = true;
            }
            else
            {
                Console.WriteLine("Ugyldigt valg. Prøv igen.");
            }
        }

        return index;
    }


    public Medarbejder HentMedarbejder(int index)
    {
        if (index >= 0 && index < medarbejdere.Count)
        {
            return medarbejdere[index];
        }
        return null;
    }
    public int GetMedarbejderCount()
    {
        return medarbejdere.Count;
    }
}


