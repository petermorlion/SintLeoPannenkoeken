using Newtonsoft.Json;
using SintLeoPannenkoeken.Blazor.Client.Server.Contracts;

namespace SintLeoPannenkoeken.Blazor.External.TourPlanning
{
    public class HereTourPlanningService
    {
        private readonly HttpClient _httpClient;
        private readonly string _hereApiKey;

        public HereTourPlanningService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _hereApiKey = Environment.GetEnvironmentVariable("HereApiKey") ?? "";
        }

        public async Task<TourPlanningResponse> GetRoute(IList<PositionDto> positions)
        {
            var requestUri = $"v3/problems?apiKey={_hereApiKey}";

            var body = new
            {
                plan = new
                {
                    jobs = positions.Select(position => new
                    {
                        id = Guid.NewGuid().ToString(),
                        tasks = new
                        {
                            deliveries = new[]
                            {
                                new
                                {
                                    places = new[]
                                    {
                                        new
                                        {
                                            location = new
                                            {
                                                lat = position.Latitude,
                                                lng = position.Longitude
                                            },
                                            duration = 300 // seconds (5 minutes)
                                        }
                                    },
                                    demand = new [] { 1 } // TODO: use aantalpakken?
                                }
                            }
                        }
                    }),
                },
                fleet = new
                {
                    types = new[]
                    {
                        new
                        {
                            id = "VehicleType1",
                            profile = "car",
                            costs = new {},
                            amount = 1,
                            capacity = new[] { 100 },
                            shifts = new[]
                            {
                                new
                                {
                                    start = new
                                    {
                                        time = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                                        location = new
                                        {
                                            // Zeescouts Sint-Leo (Lodewijk Coiseaukaai 9, 8000 Brugge)
                                            lat = 51.23229,
                                            lng = 3.22338
                                        }
                                    }
                                }
                            },
                        },
                    },
                    profiles = new[]
                                        {
                        new
                        {
                            name = "car",
                            type = "car"
                        }
                    }
                },
                configuration = new
                {
                    termination = new
                    {
                        maxTime = 5,
                        stagnationTime = 2
                    }
                }
            };

            var bodyAsString = JsonConvert.SerializeObject(body, Formatting.Indented);

            var response = await _httpClient.PostAsJsonAsync(requestUri, body);

            var result = await response.Content.ReadFromJsonAsync<TourPlanningResponse>();
            return result;
        }
    }
}
