using System;
using System.Collections.Generic;

internal class Kunde
{
    public int KundeId { get; set; }
    public string Navn { get; set; }
    public string Email { get; set; }
    public string Adresse { get; set; }
    public string TlfNr { get; set; }

    public Kunde(int kundeId, string navn, string email, string adresse, string tlfNr)
    {
        KundeId = kundeId;
        Navn = navn;
        Email = email;
        Adresse = adresse;
        TlfNr = tlfNr;
    }
}

internal class KundeManager
{
    private static KundeManager _instance;
    private Dictionary<int, Kunde> kunder = new Dictionary<int, Kunde>();
    private int næsteKundeId = 1;

    public static KundeManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new KundeManager();
            return _instance;
        }
    }
    public (string navn, string email, string adresse, string tlfNr) OpretNyKunde()
    {
        Console.Write("Indtast navn: ");
        string navn = Console.ReadLine();
        Console.Write("Indtast email: ");
        string email = Console.ReadLine();
        Console.Write("Indtast adresse: ");
        string adresse = Console.ReadLine();
        Console.Write("Indtast telefonnummer: ");
        string tlfNr = Console.ReadLine();

        return (navn, email, adresse, tlfNr);
    }


    public Kunde OpretKunde(string navn, string email, string adresse, string tlfNr)
    {
        var kunde = new Kunde(næsteKundeId++, navn, email, adresse, tlfNr);
        kunder[kunde.KundeId] = kunde;
        Console.WriteLine("Kunde oprettet: " + kunde.Navn);
        return kunde;
    }

    public Kunde FindKunde(int kundeId)
    {
        return kunder.ContainsKey(kundeId) ? kunder[kundeId] : null;
    }

    public void OpdaterKunde(int kundeId, string navn, string email, string adresse, string tlfNr)
    {
        if (kunder.ContainsKey(kundeId))
        {
            var kunde = kunder[kundeId];
            kunde.Navn = navn;
            kunde.Email = email;
            kunde.Adresse = adresse;
            kunde.TlfNr = tlfNr;
            Console.WriteLine("Kundeoplysninger opdateret!");
        }
        else
        {
            Console.WriteLine("Kunde ikke fundet.");
        }
    }

    public void SletKunde(int kundeId)
    {
        if (kunder.Remove(kundeId))
            Console.WriteLine("Kunde slettet.");
        else
            Console.WriteLine("Kunde ikke fundet.");
    }

    public void VisAlleKunder()
    {
        if (kunder.Count == 0)
        {
            Console.WriteLine("Ingen kunder registreret.");
            return;
        }

        Console.WriteLine("Kundeliste:");
        foreach (var kunde in kunder.Values)
        {
            Console.WriteLine($"KundeNr.: {kunde.KundeId}, Navn: {kunde.Navn}, Adresse: {kunde.Adresse}, Email: {kunde.Email}, TlfNr: {kunde.TlfNr}");
        }
    }

    public int VaelgKunde()
    {
        var kunder = KundeManager.Instance.kunder.Values.ToList();
        if (kunder.Count == 0)
        {
            Console.WriteLine("Ingen kunder tilgængelige. Tilføj en kunde først.");
            return -1;
        }

        int index = -1;
        bool gyldigtValg = false;

        while (!gyldigtValg)
        {
            VisAlleKunder();
            Console.Write("Indtast nummer på kunde: ");
            if (int.TryParse(Console.ReadLine(), out int valg) &&
                valg > 0 && valg <= kunder.Count)
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
    public Kunde HentKunde(int index)
    {
        var kunder = KundeManager.Instance.kunder.Values.ToList();
        if (index >= 0 && index < kunder.Count)
        {
            return kunder[index];
        }
        return null;
    }
    public int GetCustomerCount()
    {
        return kunder.Count;
    }


}
