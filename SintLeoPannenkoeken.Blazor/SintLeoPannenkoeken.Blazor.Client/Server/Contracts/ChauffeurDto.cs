namespace SintLeoPannenkoeken.Blazor.Client.Server.Contracts
{
    public class ChauffeurDto
    {
        public ChauffeurDto(int id, string achternaam, string voornaam)
        {
            Id = id;
            Achternaam = achternaam;
            Voornaam = voornaam;
        }

        public int Id { get; init; }
        public string Achternaam { get; init; }
        public string Voornaam { get; init; }
    }
}
