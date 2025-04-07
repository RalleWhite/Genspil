internal class Medarbejder
{
    public string Navn { get; set; }
    public string Rolle { get; set; }
    public string TlfNr { get; set; }
    public string Email { get; set; }


    public Medarbejder(string navn, string rolle, string tlfNr, string email)
    {
        Navn = navn;
        Rolle = rolle;
        TlfNr = tlfNr;
        Email = email;
    }
}

