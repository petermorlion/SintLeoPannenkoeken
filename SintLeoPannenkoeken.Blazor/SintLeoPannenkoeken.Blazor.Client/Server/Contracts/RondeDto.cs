namespace SintLeoPannenkoeken.Blazor.Client.Server.Contracts
{
    public class RondeDto
    {
        public int Id { get; init; }
        public int ChauffeurId { get; init; }
        public string ChauffeurNaam { get; init; } = "";
        public int ZoneId { get; init; }
        public string ZoneNaam { get; init; } = "";
        public string Gemeente { get; init; } = "";
        public string Kaartnummer { get; init; } = "";
        public int Straten { get; init; }
        public int Pakken { get; init; }
        public int Adressen { get; init; }
        public int Bestellingen { get; init; }
        public int ScoutsjaarId { get; init; }
    }
}
