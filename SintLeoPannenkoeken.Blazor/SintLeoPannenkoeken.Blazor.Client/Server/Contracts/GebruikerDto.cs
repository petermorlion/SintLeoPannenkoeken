namespace SintLeoPannenkoeken.Blazor.Client.Server.Contracts
{
    public class GebruikerDto
    {
        public GebruikerDto(string id, string email, IEnumerable<string> rollen)
        {
            Id = id;
            Email = email;
            Rollen = rollen ?? new List<string>();
        }

        public string Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Rollen { get; set; }
    }
}
