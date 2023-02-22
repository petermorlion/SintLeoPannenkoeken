namespace SintLeoPannenkoeken.Models
{
    public class Ronde
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public Zone Zone { get; set; }
        public int BestuurderId { get; set; }
        public Bestuurder Bestuurder { get; set; }
        public int ScoutsjaarId { get; set; }
    }
}
