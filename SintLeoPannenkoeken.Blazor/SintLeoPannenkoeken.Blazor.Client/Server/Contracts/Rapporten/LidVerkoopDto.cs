namespace SintLeoPannenkoeken.Blazor.Client.Server.Contracts.Rapporten
{
    public class LidVerkoopDto
    {
        public int LidId { get; set; }
        public string Voornaam { get; set; } = string.Empty;
        public string Achternaam { get; set; } = string.Empty;
        public string TakNaam { get; set; } = string.Empty;
        public int AantalPakkenVerkocht { get; set; }
    }
}
