using System.Text.Json.Serialization;

namespace SintLeoPannenkoeken.Blazor.External.RouteXL
{
    public class RouteXLLocation
    {
        [JsonPropertyName("address")]
        public string Address { get; init; } = "";

        [JsonPropertyName("lat")]
        public double Lat { get; init; }

        [JsonPropertyName("lng")]
        public double Lng { get; init; }

        [JsonPropertyName("servicetime")]
        public int ServiceTime { get; init; } = 5;
    }
}
