namespace SintLeoPannenkoeken.Models
{
    public class Scoutsjaar
    {
        public Scoutsjaar(DateTime begin, DateTime einde)
        {
            Begin = begin;
            Einde = einde;
            Bestellingen = new List<Bestelling>();
        }

        public int Id {  get; set; }
        public DateTime Begin { get; set; }
        public DateTime Einde { get; set; }
        public List<Bestelling> Bestellingen { get; set; }
    }
}
