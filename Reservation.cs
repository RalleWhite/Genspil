using System;
using System.Collections.Generic;
using System.Linq;

internal class Reservation
{
    public DateTime Reservationsdato { get; set; }
    public int Afhentningsnr { get; set; }
    public DateTime Afhentningsdato { get; set; }
    public double SamletPris { get; set; }
    public string Status { get; set; }
    public Kunde Kunde { get; set; }

    public Reservation(DateTime reservationsdato, int afhentningsnr, DateTime afhentningsdato, double samletPris, string status, Kunde kunde)
    {
        Reservationsdato = reservationsdato;
        Afhentningsnr = afhentningsnr;
        Afhentningsdato = afhentningsdato;
        SamletPris = samletPris;
        Status = status;
        Kunde = kunde;
    }
}
