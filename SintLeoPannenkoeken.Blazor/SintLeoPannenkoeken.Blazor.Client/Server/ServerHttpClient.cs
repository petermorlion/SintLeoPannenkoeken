using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using SintLeoPannenkoeken.Blazor.Client.Auth;
using SintLeoPannenkoeken.Blazor.Client.Server.Contracts;
using SintLeoPannenkoeken.Blazor.Client.Server.Contracts.Rapporten;
using System.IO;
using System.Net;
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
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadAsAsync<TResult>();
            }
            else 
            {
                var error = await result.Content.ReadAsStringAsync();
                throw new ApplicationException(error);
            }
        }

        private async Task Update<TInput>(TInput input, string endpoint, string roles)
        {
            if (!await IsUserAuthorized(roles))
            {
                throw new UnauthorizedException();
            }

            var result = await _httpClient.PutAsJsonAsync(endpoint, input);
            if (result.IsSuccessStatusCode)
            {
                return;
            }
            else
            {
                var error = await result.Content.ReadAsStringAsync();
                throw new ApplicationException(error);
            }
        }

        private async Task Delete(string endpoint, string roles)
        {
            if (!await IsUserAuthorized(roles))
            {
                throw new UnauthorizedException();
            }

            var result = await _httpClient.DeleteAsync(endpoint);
            if (result.IsSuccessStatusCode)
            {
                return;
            }
            else
            {
                var error = await result.Content.ReadAsStringAsync();
                throw new ApplicationException(error);
            }
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

        private async Task<TResult> Get<TResult>(string endpoint, string roles)
        {
            if (!await IsUserAuthorized(roles))
            {
                throw new UnauthorizedException();
            }

            var result = await _httpClient.GetFromJsonAsync<TResult>(endpoint);
            return result;
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

        public async Task<StraatDto> CreateStraat(StraatDto straatDto)
        {
            return await Create<StraatDto, StraatDto>(straatDto, "/api/straten", Roles.RolesForStraten);
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

        public async Task<string> GetPasswordResetCode(string email)
        {
            return await Get<string>($"/api/gebruikers/{email}/passwordResetCode", Roles.RolesForGebruikers);
        }

        public async Task<IList<ZoneDto>> GetZones()
        {
            return await GetList<ZoneDto>("/api/zones", Roles.RolesForBeheer);
        }

        public async Task<RondeDto> CreateRonde(int chauffeurId, int scoutsjaarBegin, CreateRondeDto rondeDto)
        {
            return await Create<CreateRondeDto, RondeDto>(rondeDto, $"/api/chauffeurs/{chauffeurId}/rondes/{scoutsjaarBegin}", Roles.RolesForChauffeurs);
        }

        public async Task<IList<RondeDto>> GetRondesForChauffeur(int chauffeurId, int scoutsjaarBegin)
        {
            return await GetList<RondeDto>($"/api/chauffeurs/{chauffeurId}/rondes/{scoutsjaarBegin}", Roles.RolesForChauffeurs);
        }

        public async Task<ChauffeurDto> GetChauffeur(int scoutsjaar, int chauffeurId)
        {
            return await Get<ChauffeurDto>($"/api/chauffeurs/{scoutsjaar}/{chauffeurId}", Roles.RolesForChauffeurs);
        }

        public async Task DeleteRonde(int rondeId)
        {
            await Delete($"/api/rondes/{rondeId}", Roles.RolesForChauffeurs);
        }

        public async Task<IList<RondeDto>> GetRondes(int scoutsjaarBegin)
        {
            return await GetList<RondeDto>($"/api/rondes/{scoutsjaarBegin}", Roles.RolesForRondes);
        }

        public async Task<RondeDetailsDto> GetRonde(int scoutsjaarBegin, int rondeId)
        {
            return await Get<RondeDetailsDto>($"/api/rondes/{scoutsjaarBegin}/ronde/{rondeId}", Roles.RolesForRondes);
        }

        public async Task UpdateGeleverd(int bestellingId, bool gelevered)
        {
            await Update(new BestellingGeleverdDto { Geleverd = gelevered }, $"/api/bestellingen/{bestellingId}/delivery", Roles.RolesForBestellingen);
        }

        public async Task<ChauffeurRondeDetailsDto> GetChauffeurRondeDetails(int scoutsjaarBegin, int chauffeurId)
        {
            return await Get<ChauffeurRondeDetailsDto>($"/api/rapporten/{scoutsjaarBegin}/chaffeurrondedetails/{chauffeurId}", Roles.RolesForRapporten);
        }

        public async Task<ChauffeurRondeDetailsDto> GetChauffeurRondeDetailsRoute(int scoutsjaarBegin, int chauffeurId)
        {
            return await Get<ChauffeurRondeDetailsDto>($"/api/rapporten/{scoutsjaarBegin}/chaffeurrondedetails/{chauffeurId}/route", Roles.RolesForRapporten);
        }

        public async Task<VerkoopPerTakDto> GetVerkoopPerTakRapport(int scoutsjaarBegin)
        {
            return await Get<VerkoopPerTakDto>($"/api/rapporten/{scoutsjaarBegin}/verkooppertak", Roles.RolesForRapporten);
        }

        public async Task<VerkoopPerLidDto> GetVerkoopPerLidRapport(int scoutsjaarBegin)
        {
            return await Get<VerkoopPerLidDto>($"/api/rapporten/{scoutsjaarBegin}/verkoopperlid", Roles.RolesForRapporten);
        }

        public async Task<IngaveTotalenDto> GetIngaveTotalen(int scoutsjaarBegin)
        {
            return await Get<IngaveTotalenDto>($"/api/rapporten/{scoutsjaarBegin}/ingavetotalen", Roles.RolesForRapporten);
        }

        public async Task<BestellingenImportResultDto> ImportBestellingen(int scoutsjaarBegin, IBrowserFile file)
        {
            using var requestContent = new MultipartFormDataContent();


            var fileContent = new StreamContent(file.OpenReadStream());

            requestContent.Add(
                        content: fileContent,
                        name: "\"file\"",
                        fileName: file.Name);

            if (!await IsUserAuthorized(Roles.RolesForBestellingen))
            {
                throw new UnauthorizedException();
            }

            var result = await _httpClient.PostAsync($"/api/bestellingen/{scoutsjaarBegin}/import", requestContent);
            
            if (result.IsSuccessStatusCode)
            {
                return await result.Content.ReadAsAsync<BestellingenImportResultDto>();
            }
            else
            {
                var error = await result.Content.ReadAsStringAsync();
                throw new ApplicationException(error);
            }
        }
    }
}
