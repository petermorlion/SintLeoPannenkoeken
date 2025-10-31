namespace SintLeoPannenkoeken.Blazor.External.TourPlanning
{
    public class Stop
    {
        public Location Location { get; set; } = new Location();
        public IList<Activity> Activities { get; set; } = new List<Activity>();
    }
}
