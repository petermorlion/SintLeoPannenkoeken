namespace SintLeoPannenkoeken.Blazor.Client.Server.Contracts
{
    public class LidDto
    {
        public LidDto(int id, string achternaam, string voornaam, string functie, string takNaam)
        {
            Id = id;
            Achternaam = achternaam;
            Voornaam = voornaam;
            Functie = functie;
            TakNaam = takNaam;
        }

        public int Id { get; init; }
        public string Achternaam { get; init; }
        public string Voornaam { get; init; }
        public string Functie { get; init; }
        public string TakNaam { get; init; }
    }
}
