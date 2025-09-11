namespace SintLeoPannenkoeken.Blazor.Client.Server.Contracts
{
    public class NewGebruikerDto
    {
        public NewGebruikerDto(string email, IList<string> rollen)
        {
            Email = email;
            Rollen = rollen ?? new List<string>();
        }

        public string Email { get; init; }
        public IList<string> Rollen { get; init; }
    }
}
