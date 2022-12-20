namespace SintLeoPannenkoeken.Models
{
    public class Zone
    {
        public Zone()
        {
            Straten = new List<Straat>();
        }

        public int Id { get; set; }
        public string Naam { get; set; }
        public int? PostNummer { get; set; }
        public string Gemeente { get; set; }
        public string? KaartNummer { get; set; }
        public string? Omschrijving { get; set; }
        public string? Bestuurder { get; set; }
        public List<Straat> Straten { get; set; }
    }
}
