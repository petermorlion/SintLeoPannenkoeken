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

        private async Task<TResult> Create<TInput, TResult>(TInput input, string endpoint, string roles)
        {
            if (!await IsUserAuthorized(roles))
            {
                throw new UnauthorizedException();
            }

            var result = await _httpClient.PostAsJsonAsync(endpoint, input);
            return await result.Content.ReadAsAsync<TResult>();
        }

        private async Task Update<TInput>(TInput input, string endpoint, string roles)
        {
            if (!await IsUserAuthorized(roles))
            {
                return;
            }

            await _httpClient.PutAsJsonAsync(endpoint, input);
        }

        private async Task Delete(string endpoint, string roles)
        {
            if (!await IsUserAuthorized(roles))
            {
                throw new UnauthorizedException();
            }

            await _httpClient.DeleteAsync(endpoint);
        }

        private async Task<IList<TResult>> GetList<TResult>(string endpoint, string roles)
        {
            if (!await IsUserAuthorized(roles))
            {
                return new List<TResult>();
            }

            var result = await _httpClient.GetFromJsonAsync<IList<TResult>>(endpoint);
            return result ?? new List<TResult>();
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

        public async Task<IList<ScoutsjaarDto>> GetScoutsjaren()
        {
            return await GetList<ScoutsjaarDto>("/api/scoutsjaren", Roles.RolesForScoutsjaren);
        }

        public async Task<IList<LidDto>> GetLeden()
        {
            return await GetList<LidDto>("/api/leden", Roles.RolesForLeden);
        }

        public async Task<IList<GebruikerDto>> GetGebruikers()
        {
            return await GetList<GebruikerDto>("/api/gebruikers", Roles.RolesForGebruikers);
        }

        public async Task<GebruikerCreatedDto> CreateGebruiker(NewGebruikerDto gebruiker)
        {
            return await Create<NewGebruikerDto, GebruikerCreatedDto>(gebruiker, "/api/gebruikers", Roles.RolesForGebruikers);
        }

        public async Task UpdateGebruiker(GebruikerDto gebruiker)
        {
            await Update(gebruiker, "/api/gebruikers", Roles.RolesForGebruikers);
        }

        public async Task<IList<StraatDto>> GetStraten()
        {
            return await GetList<StraatDto>("/api/straten", Roles.RolesForStraten);
        }

        public async Task<IList<ChauffeurDto>> GetChauffeurs(int scoutsjaar)
        {
            return await GetList<ChauffeurDto>($"/api/chauffeurs/{scoutsjaar}", Roles.RolesForChauffeurs);
        }

        public async Task UpdateScoutsjaar(ScoutsjaarDto scoutsjaar)
        {
            await Update(scoutsjaar, "/api/scoutsjaren", Roles.RolesForScoutsjaren);
        }

        public async Task<IList<BestellingDto>> GetBestellingen(int scoutsjaar)
        {
            return await GetList<BestellingDto>($"/api/bestellingen/{scoutsjaar}", Roles.RolesForBestellingen);
        }

        public async Task UpdateBestelling(UpdateBestellingDto bestelling)
        {
            await Update(bestelling, "/api/bestellingen", Roles.RolesForBestellingen);
        }

        public async Task UpdateLid(LidDto lid)
        {
            await Update(lid, "/api/leden", Roles.RolesForLeden);
        }

        public async Task<IList<TakDto>> GetTakken()
        {
            return await GetList<TakDto>("/api/takken", Roles.RolesForTakken);
        }

        public async Task<LidDto> CreateLid(NewLidDto lid)
        {
            return await Create<NewLidDto, LidDto>(lid, "/api/leden", Roles.RolesForLeden);
        }

        public async Task<BestellingDto> CreateBestelling(NewBestellingDto bestelling)
        {
            return await Create<NewBestellingDto, BestellingDto>(bestelling, "/api/bestellingen", Roles.RolesForBestellingen);
        }

        public async Task<IList<StreefcijferDto>> GetStreefcijfers(int jaar)
        {
            return await GetList<StreefcijferDto>($"/api/streefcijfers/{jaar}", Roles.RolesForStreefcijfers);
        }

        public async Task<StreefcijferDto> CreateStreefcijfer(StreefcijferDto streefcijfer)
        {
            return await Create<StreefcijferDto, StreefcijferDto>(streefcijfer, "/api/streefcijfers", Roles.RolesForStreefcijfers);
        }

        public async Task UpdateStreefcijfer(StreefcijferDto streefcijfer)
        {
            await Update(streefcijfer, "/api/streefcijfers", Roles.RolesForStreefcijfers);
        }

        public async Task DeleteStreefcijfer(int streefcijferId)
        {
            await Delete($"/api/streefcijfers/{streefcijferId}", Roles.RolesForStreefcijfers);
        }

        public async Task DeleteBestelling(int bestellingId)
        {
            await Delete($"/api/bestellingen/{bestellingId}", Roles.RolesForBestellingen);
        }

        public async Task<ChauffeurDto> CreateChauffeur(ChauffeurDto chauffeur)
        {
            return await Create<ChauffeurDto, ChauffeurDto>(chauffeur, "/api/chauffeurs", Roles.RolesForChauffeurs);
        }

        public async Task UpdateChauffeur(ChauffeurDto chauffeur)
        {
            await Update(chauffeur, "/api/chauffeurs", Roles.RolesForChauffeurs);
        }

        public async Task DeleteChauffeur(int chauffeurId)
        {
            await Delete($"/api/chauffeurs/{chauffeurId}", Roles.RolesForChauffeurs);
        }

        public async Task DeleteGebruiker(string email)
        {
            await Delete($"/api/gebruikers/{email}", Roles.RolesForGebruikers);
        }
    }
}
