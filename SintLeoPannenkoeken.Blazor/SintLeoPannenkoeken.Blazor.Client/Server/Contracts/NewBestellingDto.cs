namespace SintLeoPannenkoeken.Blazor.Client.Server.Contracts
{
    public class NewBestellingDto
    {
        public int Id { get; set; }
        public int BestellingNummer { get; set; }
        public string Naam { get; set; }
        public string? Telefoon { get; set; }
        public int AantalPakken { get; set; }
        public string? Opmerkingen { get; set; }
        public bool Betaald { get; set; }
        public bool Geleverd { get; set; }
        public int StraatId { get; set; }
        public string Nummer { get; set; }
        public string Bus { get; set; }
        public int LidId { get; set; }
        public int ScoutsjaarId { get; set; }
    }
}
