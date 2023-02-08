using Microsoft.EntityFrameworkCore;

namespace SintLeoPannenkoeken.Models
{
    [Index(nameof(Begin), IsUnique = true)]
    public class Scoutsjaar
    {
        public Scoutsjaar(int begin, int einde)
        {
            Begin = begin;
            Einde = einde;
            Bestellingen = new List<Bestelling>();
        }

        public int Id { get; set; }
        public int Begin { get; set; }
        public int Einde { get; set; }
        public List<Bestelling> Bestellingen { get; set; }
        public List<StreefCijfer> StreefCijfers { get; set; }
    }
}
