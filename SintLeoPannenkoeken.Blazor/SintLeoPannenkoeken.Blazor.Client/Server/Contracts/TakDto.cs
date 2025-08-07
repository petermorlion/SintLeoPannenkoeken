namespace SintLeoPannenkoeken.Blazor.Client.Server.Contracts
{
    public class TakDto
    {
        public TakDto(int id, string naam)
        {
            Id = id;
            Naam = naam;
        }

        public int Id { get; init; }
        public string Naam { get; init; }
    }
}
