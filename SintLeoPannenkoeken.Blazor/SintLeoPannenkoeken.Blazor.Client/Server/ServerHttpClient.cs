using Microsoft.AspNetCore.Components.Authorization;
using SintLeoPannenkoeken.Blazor.Client.Auth;
using SintLeoPannenkoeken.Blazor.Client.Server.Contracts;
using System.Net.Http.Json;

namespace SintLeoPannenkoeken.Blazor.Client.Server
{
    /// <summary>
    /// HTTP client implementation of the server interface. To be used in the 
    /// Blazor client application to communicate with the server API.
    /// </summary>
    public class ServerHttpClient : IServerData
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public ServerHttpClient(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<IList<ScoutsjaarDto>> GetScoutsjaren()
        {
            if (!await IsUserAuthorized(Roles.RolesForScoutsjaren))
            {
                return new List<ScoutsjaarDto>();
            }

            var result = await _httpClient.GetFromJsonAsync<IList<ScoutsjaarDto>>("/api/scoutsjaren");
            return result ?? new List<ScoutsjaarDto>();
        }

        public async Task<IList<LidDto>> GetLeden()
        {
            if (!await IsUserAuthorized(Roles.RolesForLeden))
            {
                return new List<LidDto>();
            }

            var result = await _httpClient.GetFromJsonAsync<IList<LidDto>>("/api/leden");
            return result ?? new List<LidDto>();
        }

        public async Task<IList<GebruikerDto>> GetGebruikers()
        {
            if (!await IsUserAuthorized(Roles.RolesForGebruikers))
            {
                return new List<GebruikerDto>();
            }

            var result = await _httpClient.GetFromJsonAsync<IList<GebruikerDto>>("/api/gebruikers");
            return result ?? new List<GebruikerDto>();
        }

        public async Task<IList<StraatDto>> GetStraten()
        {
            if (!await IsUserAuthorized(Roles.RolesForStraten))
            {
                return new List<StraatDto>();
            }

            var result = await _httpClient.GetFromJsonAsync<IList<StraatDto>>("/api/straten");
            return result ?? new List<StraatDto>();
        }

        public async Task<IList<ChauffeurDto>> GetChauffeurs()
        {
            if (!await IsUserAuthorized(Roles.RolesForChauffeurs))
            {
                return new List<ChauffeurDto>();
            }

            var result = await _httpClient.GetFromJsonAsync<IList<ChauffeurDto>>("/api/chauffeurs");
            return result ?? new List<ChauffeurDto>();
        }

        public async Task UpdateScoutsjaar(ScoutsjaarDto scoutsjaar)
        {
            await _httpClient.PutAsJsonAsync($"/api/scoutsjaren/{scoutsjaar.Begin}", scoutsjaar);
        }

        private async Task<bool> IsUserAuthorized(string roleString)
        {
            var roles = roleString.Split(',');

            var authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            if (authenticationState == null
                || authenticationState.User == null
                || authenticationState.User.Identity == null)
            {
                return false;
            }

            if (!authenticationState.User.Identity.IsAuthenticated)
            {
                return false;
            }

            foreach (var role in roles)
            {
                if (authenticationState.User.IsInRole(role))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
