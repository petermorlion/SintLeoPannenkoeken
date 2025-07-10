namespace SintLeoPannenkoeken.Blazor.Client.Server.Contracts
{
    public class GebruikerDto
    {
        public GebruikerDto(string id, string email, IList<string> rollen)
        {
            Id = id;
            Email = email;
            Rollen = rollen ?? new List<string>();
        }

        public string Id { get; init; }
        public string Email { get; init; }
        public IList<string> Rollen { get; init; }
    }
}
