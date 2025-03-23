using System;
using System.Collections.Generic;

internal class Braetspil
{
    public string Navn { get; set; }
    public string Version { get; set; }
    public string Stand { get; set; }
    public double Pris { get; set; }
    public string Genre { get; set; }
    public string AntalSpillere { get; set; }

    public Braetspil(string navn, string version, string stand, double pris, string genre, string antalSpillere)
    {
        Navn = navn;
        Version = version;
        Stand = stand;
        Pris = pris;
        Genre = genre;
        AntalSpillere = antalSpillere;
    }
}
