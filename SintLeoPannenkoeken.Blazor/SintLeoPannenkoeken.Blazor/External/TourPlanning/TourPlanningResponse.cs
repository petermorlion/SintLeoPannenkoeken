namespace SintLeoPannenkoeken.Blazor.External.TourPlanning
{
    public class TourPlanningResponse
    {
        public Statistics Statistics { get; set; } = new Statistics();
        public IList<Tour> Tours { get; set; } = new List<Tour>();
    }
}
