namespace SintLeoPannenkoeken.Blazor.External.Geocoding
{
    public class HereGeocodingService
    {
        private readonly HttpClient _httpClient;
        private readonly string _hereApiKey;

        public HereGeocodingService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _hereApiKey = Environment.GetEnvironmentVariable("HereApiKey") ?? "";
        }

        public async Task<HereGeocodingResponse> GetGeocode(string straat, string nummer, string postNummer, string gemeente)
        {
            var qualifiedQuery = $"street={straat};houseNumber={nummer};postalCode={postNummer};city={gemeente}";
            var requestUri = $"v1/geocode?apiKey={_hereApiKey}&qq={qualifiedQuery}";
            var response = await _httpClient.GetFromJsonAsync<HereGeocodingResponse>(requestUri);

            return response ?? new HereGeocodingResponse();
        }
    }
}
