namespace SintLeoPannenkoeken.Blazor.Client.Server.Contracts
{
    public class ChauffeurDto
    {
        public int Id { get; init; }
        public string Achternaam { get; init; } = "";
        public string Voornaam { get; init; } = "";
        public int AantalBestellingen { get; set; } = 0;
        public int AantalPakken { get; set; } = 0;
    }
}
