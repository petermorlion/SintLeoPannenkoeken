namespace SintLeoPannenkoeken.Blazor.Client.Server.Contracts
{
    public class RondeDetailsBestellingDto
    {
        public int BestellingId { get; init; }
        public string Naam { get; init; } = "";
        public string Straat { get; init; } = "";
        public string Nummer { get; init; } = "";
        public string Bus { get; init; } = "";
        public int AantalPakken { get; init; }
        public bool Geleverd { get; set; }
    }
}
