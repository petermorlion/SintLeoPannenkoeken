namespace SintLeoPannenkoeken.Models
{
    public class Bestelling
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public int AantalPakken { get; set; }
        public string Opmerkingen { get; set; }
        public bool Betaald { get; set; }
    }
}
