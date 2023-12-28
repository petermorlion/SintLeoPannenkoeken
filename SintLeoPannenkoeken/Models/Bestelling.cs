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

        /// <summary>
        /// Uniek nummer per verkoopsjaar. Elk jaar begint terug vanaf 1.
        /// </summary>
        public int BestellingNummer { get; set; }
        public string Naam { get; set; }
        public string Telefoon { get; set; }
        public int AantalPakken { get; set; }
        public string Opmerkingen { get; set; }
        public bool Betaald { get; set; }
        public int LidId { get; set; }
        public Lid Lid { get; set; }
        public int TakId { get; set; }
        public Tak Tak { get; set; }
        public int StraatId { get; set; }
        public Straat Straat { get; set; }
        public string Nummer { get; set; }
        public string Bus { get; set; }
        public DateTime IngaveDatum { get; set; }
        public int ScoutsjaarId { get; set; }
    }
}
