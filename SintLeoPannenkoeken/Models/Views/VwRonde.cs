using Microsoft.EntityFrameworkCore;

namespace SintLeoPannenkoeken.Models.Views
{
    [Keyless]
    public class VwRonde
    {
        public int huisnummer { get; set; }
        public string Bus { get; set; }
        public string straatnaam { get; set; }
        public string Naam { get; set; }
        public int AantalPakken { get; set; }
        public string zone_naam { get; set; }
        public string zone_postnummer { get; set; }
        public string zone_gemeente{ get; set; }
        public string bestuurder { get; set; }
    }
}
