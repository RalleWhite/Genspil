using System;
using System.Collections.Generic;

internal class Kunde
{
    public string Navn { get; set; }
    public string Adresse { get; set; }
    public string TlfNr { get; set; }
    public string Email { get; set; }

    public Kunde(string navn, string adresse, string tlfNr, string email)
    {
        Navn = navn;
        Adresse = adresse;
        TlfNr = tlfNr;
        Email = email;
    }
}
