namespace SintLeoPannenkoeken.Blazor.Client.Server.Contracts
{
    public class StraatDto
    {
        public int Id { get; init; }
        public string Naam { get; init; }
        public int PostNummer { get; init; }
        public string Gemeente { get; init; }
        public string? Omschrijving { get; init; }
        public int ZoneId { get; init; }
        public string ZoneNaam { get; init; }
        public int Nummer { get; init; }
        public StraatDto(int id, string naam, int postNummer, string gemeente, string? omschrijving, int zoneId, string zoneNaam, int nummer)
        {
            Id = id;
            Naam = naam;
            PostNummer = postNummer;
            Gemeente = gemeente;
            Omschrijving = omschrijving;
            ZoneId = zoneId;
            ZoneNaam = zoneNaam;
            Nummer = nummer;
        }
    }
}
