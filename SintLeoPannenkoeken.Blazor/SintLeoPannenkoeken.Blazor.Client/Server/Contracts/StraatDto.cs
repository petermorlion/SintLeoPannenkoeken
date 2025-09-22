namespace SintLeoPannenkoeken.Blazor.Client.Server.Contracts
{
    public class StraatDto
    {
        public int Id { get; init; }
        public string Naam { get; init; } = "";
        public int PostNummer { get; init; }
        public string Gemeente { get; init; } = "";
        public string? Omschrijving { get; init; }
        public int ZoneId { get; init; }
        public string ZoneNaam { get; init; } = "";
        public int Nummer { get; init; }
    }
}
