using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Bestellingen
{
    public class IndexViewModel
    {
        public IndexViewModel(Scoutsjaar scoutsjaar)
        {
            Scoutsjaar = new ScoutsjaarViewModel(scoutsjaar);
        }

        public ScoutsjaarViewModel Scoutsjaar { get; set; }
    }
}
