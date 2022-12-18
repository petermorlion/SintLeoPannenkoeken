namespace SintLeoPannenkoeken.Models
{
    public class Straat
    {
        public Straat(string naam, string gemeente) {
            Naam = naam;
            Gemeente = gemeente;
        }

        public int Id { get; set; }
        public string Naam { get; set; }
        public int PostNummer { get; set; }
        public string Gemeente { get; set; }
        public string? Omschrijving { get; set; }
        public int ZoneId { get; set; }
        public Zone? Zone { get; set; }
    }
}
