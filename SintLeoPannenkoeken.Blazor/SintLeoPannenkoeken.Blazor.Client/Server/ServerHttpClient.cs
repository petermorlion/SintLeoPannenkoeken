using SintLeoPannenkoeken.Blazor.Client.Server.Contracts;
using System.Net.Http.Json;

namespace SintLeoPannenkoeken.Blazor.Client.Server
{
    public class ServerHttpClient : IServerHttpClient
    {
        private readonly HttpClient _httpClient;

        public ServerHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<IList<ScoutsjaarDto>> GetScoutsjaren()
        {
            var result = await _httpClient.GetFromJsonAsync<IList<ScoutsjaarDto>>("/api/scoutsjaren");
            return result ?? new List<ScoutsjaarDto>();
        }

        public async Task<IList<LidDto>> GetLeden()
        {
            var result = await _httpClient.GetFromJsonAsync<IList<LidDto>>("/api/leden");
            return result ?? new List<LidDto>();
        }
    }
}
