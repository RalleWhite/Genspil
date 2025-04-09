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
}
