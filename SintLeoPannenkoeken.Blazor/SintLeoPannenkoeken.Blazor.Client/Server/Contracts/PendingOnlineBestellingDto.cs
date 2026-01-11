namespace SintLeoPannenkoeken.Blazor.Client.Server.Contracts
{
    public class PendingOnlineBestellingDto
    {
        public int Id { get; set; }
        public string Naam { get; set; } = "";
        public string Telefoon { get; set; } = "";
        public int AantalPakken { get; set; }
        public string Opmerkingen { get; set; } = "";
        public bool Betaald { get; set; }
        public bool Geleverd { get; set; }
        public string Straat { get; set; } = "";
        public int? ProposedStraatId { get; set; }
        public string Nummer { get; set; } = "";
        public string Bus { get; set; } = "";
        public string Lid { get; set; } = "";
        public int? ProposedLidId { get; set; }
    }
}
