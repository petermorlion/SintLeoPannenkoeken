namespace SintLeoPannenkoeken.Models
{
    public class Bestelling
    {
        public Bestelling(string naam, int aantalPakken)
        {
            Naam = naam;
            AantalPakken = aantalPakken;
        }

        public int Id { get; set; }
        public string Naam { get; set; }
        public string Telefoon { get; set; }
        public int AantalPakken { get; set; }
        public string Opmerkingen { get; set; }
        public bool Betaald { get; set; }
    }
}
