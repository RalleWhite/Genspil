using System;
using System.Collections.Generic;
using System.Linq;

public enum ReservationStatus
{
    Oprettet = 1,
    Klar = 2,
    Afhentet = 3,
    Annulleret = 4
}


internal class Reservation
{
    public int AfhentningsNr { get; set; }
    public DateTime ReservationsDato { get; set; }
    public DateTime AfhentningsDato { get; set; }
    public Kunde Kunde { get; set; }
    public Braetspil Braetspil { get; set; }
    public Medarbejder Medarbejder { get; set; }
    public ReservationStatus Status;

    public Reservation(int afhentningsNr, DateTime reservationsDato, DateTime afhentningsDato, Kunde kunde, Braetspil braetspil, ReservationStatus status, Medarbejder medarbejder)
    {
        AfhentningsNr = afhentningsNr;
        ReservationsDato = reservationsDato;
        AfhentningsDato = afhentningsDato;
        Kunde = kunde;
        Braetspil = braetspil;
        Status = status;
        Medarbejder = medarbejder;
    }
}
