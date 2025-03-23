using System;
using System.Collections.Generic;

internal class Forespoergsel
{
    public int ForespoergselNr { get; set; }
    public DateTime Dato { get; set; }
    public int Antal { get; set; }

    public Forespoergsel(int forespoergselNr, DateTime dato, int antal)
    {
        ForespoergselNr = forespoergselNr;
        Dato = dato;
        Antal = antal;
    }
}
