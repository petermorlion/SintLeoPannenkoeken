using System.Text.Json.Serialization;

namespace SintLeoPannenkoeken.Blazor.External.RouteXL
{
    public class RouteXLTourResponse
    {
        [JsonPropertyName("route")]
        public IDictionary<string, RouteXLRouteWaypoint> Route { get; init; } = new Dictionary<string, RouteXLRouteWaypoint>();
    }
}
