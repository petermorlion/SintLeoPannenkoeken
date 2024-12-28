using Microsoft.EntityFrameworkCore;

namespace SintLeoPannenkoeken.Models
{
    [Index(nameof(Begin), IsUnique = true)]
    public class Scoutsjaar
    {
        public Scoutsjaar(int begin)
        {
            Begin = begin;
            Bestellingen = new List<Bestelling>();
        }

        public int Id { get; set; }
        public int Begin { get; set; }
        public int PannenkoekenPerPak { get; set; }
        public List<Bestelling> Bestellingen { get; set; }
        public List<StreefCijfer> StreefCijfers { get; set; }
        public List<Ronde> Rondes { get; set; }
    }
}
