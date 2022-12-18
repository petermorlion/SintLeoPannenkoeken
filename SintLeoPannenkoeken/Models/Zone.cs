namespace SintLeoPannenkoeken.Models
{
    public class Zone
    {
        public Zone()
        {
            Zones = new List<Zone>();
        }

        public int Id { get; set; }
        public string Naam { get; set; }
        public int? PostNummer { get; set; }
        public string Gemeente { get; set; }
        public int? KaartNummer { get; set; }
        public string? Omschrijving { get; set; }
        public string? Bestuurder { get; set; }
        public List<Zone> Zones { get; set; }
    }
}
