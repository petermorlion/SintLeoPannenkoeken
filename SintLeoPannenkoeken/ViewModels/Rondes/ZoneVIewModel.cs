using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.Rondes
{
    public class ZoneViewModel
    {
        private readonly Zone _zone;

        public ZoneViewModel(Zone zone)
        {
            _zone = zone;
        }

        public int Id => _zone.Id;
        public string Naam => _zone.Naam;
    }
}
