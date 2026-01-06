namespace SintLeoPannenkoeken.Blazor.Client.Server.Contracts
{
    public class BestellingDto
    {
        public int Id { get; set; }
        public int BestellingNummer { get; set; }
        public string Naam { get; set; }
        public string? Telefoon { get; set; }
        public int AantalPakken { get; set; }
        public string? Opmerkingen { get; set; }
        public bool Betaald { get; set; }
        public bool Geleverd { get; set; }
        public string Nummer { get; set; }
        public string Bus { get; set; }
        public StraatDto? Straat { get; set; } = null;
        public LidDto? Lid { get; set; } = null;


        public string TakNaam => Lid?.TakNaam ?? string.Empty;
        public string LidNaam => $"{Lid?.Achternaam} {Lid?.Voornaam}";
        public string StraatNaam => Straat?.Naam ?? string.Empty;
    }
}
