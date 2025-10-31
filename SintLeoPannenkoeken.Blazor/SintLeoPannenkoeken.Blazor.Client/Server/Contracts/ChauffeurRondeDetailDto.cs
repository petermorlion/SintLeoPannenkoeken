namespace SintLeoPannenkoeken.Blazor.Client.Server.Contracts
{

    public class ChauffeurRondeDetailDto
    {
        public int BestellingId { get; init; } = 0;
        public string ZoneNaam { get; init; } = "";
        public string Straat { get; init; } = "";
        public string Nummer { get; init; } = "";
        public string Bus { get; init; } = "";
        public string Naam { get; init; } = "";
        public int AantalPakken { get; init; }
        public string PostNummer { get; init; } = "";
        public string Gemeente { get; init; } = "";
        public PositionDto? Position { get; set; }
    }
}
