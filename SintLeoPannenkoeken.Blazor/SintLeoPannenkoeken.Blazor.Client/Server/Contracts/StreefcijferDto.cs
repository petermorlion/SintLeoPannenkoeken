namespace SintLeoPannenkoeken.Blazor.Client.Server.Contracts
{
    public class StreefcijferDto
    {
        public int? Id { get; set; }
        public int TakId { get; set; }
        public string TakNaam { get; set; } = "";
        public int Aantal { get; set; }
        public int ScoutsjaarId { get; set; }
    }
}
