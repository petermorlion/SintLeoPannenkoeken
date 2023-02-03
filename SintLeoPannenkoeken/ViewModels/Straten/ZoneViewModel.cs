using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Straten
{
    public class ZoneViewModel
    {
        public readonly Zone _zone;
        public ZoneViewModel(Zone zone)
        {
            _zone = zone;
        }

        public int Id => _zone.Id;
        public string Naam => _zone.Naam;
    }
}
