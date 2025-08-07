namespace SintLeoPannenkoeken.Blazor.Client.Server.Contracts
{
    public class LidDto
    {
        public LidDto(string achternaam, string voornaam, string functie, string takNaam, int? id = null)
        {
            Id = id; 
            Achternaam = achternaam;
            Voornaam = voornaam;
            Functie = functie;
            TakNaam = takNaam;
        }

        public int? Id { get; init; }
        public string Achternaam { get; init; }
        public string Voornaam { get; init; }
        public string Functie { get; init; }
        public string TakNaam { get; init; }
    }

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
