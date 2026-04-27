using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using SintLeoPannenkoeken.Blazor.Client.Server.Contracts;
using SintLeoPannenkoeken.Blazor.Options;

namespace SintLeoPannenkoeken.Blazor.External.RouteXL
{
    public class RouteXLService
    {
        private readonly HttpClient _httpClient;
        private readonly IOptions<RouteXLOptions> _options;

        public RouteXLService(HttpClient httpClient, IOptions<RouteXLOptions> options)
        {
            _httpClient = httpClient;
            _options = options;
        }

        public RouteXLOptions Options => _options.Value;

        public bool HasCredentials => Options.HasCredentials;

        public async Task<IList<int>> GetOptimizedStopIds(IList<ChauffeurRondeDetailDto> details)
        {
            if (details.Count < 2)
            {
                return new List<int>();
            }

            using var request = new HttpRequestMessage(HttpMethod.Post, "tour/");
            request.Headers.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Options.UserName}:{Options.Password}")));

            var locations = details
                .Where(detail => detail.Position != null)
                .Select(detail => new RouteXLLocation
                {
                    Address = detail.BestellingId.ToString(),
                    Lat = detail.Position!.Latitude,
                    Lng = detail.Position.Longitude
                })
                .ToList();

            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["locations"] = JsonSerializer.Serialize(locations)
            });

            using var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var route = await response.Content.ReadFromJsonAsync<RouteXLTourResponse>();
            return RouteXLRoutePlanner.ExtractStopIds(route);
        }
    }
}
