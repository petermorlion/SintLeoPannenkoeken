using System.Text.Json.Serialization;

namespace SintLeoPannenkoeken.Blazor.External.RouteXL
{
    public class RouteXLRouteWaypoint
    {
        [JsonPropertyName("name")]
        public string Name { get; init; } = "";
    }
}
