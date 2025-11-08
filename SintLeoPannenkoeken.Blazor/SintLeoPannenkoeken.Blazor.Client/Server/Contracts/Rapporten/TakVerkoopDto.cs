namespace SintLeoPannenkoeken.Blazor.Client.Server.Contracts.Rapporten
{
    public class TakVerkoopDto
    {
        public string Naam { get; set; } = "";
        public int Streefcijfer { get; set; }
        public int AantalPakkenVerkocht { get; set; }
        public int Leden { get; set; }
    }
}