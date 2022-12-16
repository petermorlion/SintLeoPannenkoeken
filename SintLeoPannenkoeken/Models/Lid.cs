namespace SintLeoPannenkoeken.Models
{
    public class Lid
    {
        public Lid(string voornaam, string achternaam)
        {
            Voornaam = voornaam;
            Achternaam = achternaam;
            Functie = "";
        }
        public int Id { get; set; }
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
        public string Functie { get; set; }
        public List<Bestelling> Bestellingen { get; set; }
        public int TakId { get; set; }
        public Tak Tak { get; set; }
    }
}
