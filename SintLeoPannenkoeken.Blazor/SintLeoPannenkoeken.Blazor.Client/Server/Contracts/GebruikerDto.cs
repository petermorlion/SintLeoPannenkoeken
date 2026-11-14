namespace SintLeoPannenkoeken.Blazor.Client.Server.Contracts
{
    public class GebruikerDto
    {
        public GebruikerDto(string id, string email, IEnumerable<string> rollen)
        {
            Id = id;
            Email = email;
            Rollen = new HashSet<string>(rollen ?? new List<string>());
        }

        public string Id { get; set; }
        public string Email { get; set; }
        public HashSet<string> Rollen { get; set; } = new();
    }
}
