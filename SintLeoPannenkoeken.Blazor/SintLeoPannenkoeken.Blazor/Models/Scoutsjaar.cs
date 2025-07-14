using Microsoft.EntityFrameworkCore;

namespace SintLeoPannenkoeken.Blazor.Models
{
    [Index(nameof(Begin), IsUnique = true)]
    public class Scoutsjaar
    {
        public Scoutsjaar(int begin, int pannenkoekenPerPak)
        {
            Begin = begin;
            PannenkoekenPerPak = pannenkoekenPerPak;
            Bestellingen = new List<Bestelling>();
        }

        public int Id { get; set; }
        public int Begin { get; set; }
        public int PannenkoekenPerPak { get; set; }
        public ScoutsjaarStatus Status { get;set; } = ScoutsjaarStatus.Actief;
        public List<Bestelling> Bestellingen { get; set; }
        public List<StreefCijfer> StreefCijfers { get; set; }
        public List<Ronde> Rondes { get; set; }
    }
}
