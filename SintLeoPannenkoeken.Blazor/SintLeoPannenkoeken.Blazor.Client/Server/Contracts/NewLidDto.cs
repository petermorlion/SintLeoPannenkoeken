namespace SintLeoPannenkoeken.Blazor.Client.Server.Contracts
{
    public class NewLidDto
    {
        public NewLidDto(string achternaam, string voornaam, string functie, int takId)
        {
            Achternaam = achternaam;
            Voornaam = voornaam;
            Functie = functie;
            TakId = takId;
        }
        public string Achternaam { get; init; }
        public string Voornaam { get; init; }
        public string Functie { get; init; }
        public int TakId { get; init; }
    }
}
