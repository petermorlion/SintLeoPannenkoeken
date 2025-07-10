using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SintLeoPannenkoeken.Blazor.Client.Server;
using SintLeoPannenkoeken.Blazor.Client.Server.Contracts;

namespace SintLeoPannenkoeken.Blazor.Data
{
    /// <summary>
    /// Direct access to server data. To be used in the server-side Blazor application.
    /// </summary>
    public class ServerDirectClient : IServerData
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public ServerDirectClient(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IList<GebruikerDto>> GetGebruikers()
        {
            var users = await _dbContext.Users.ToListAsync();

            var userDtos = new List<GebruikerDto>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDtos.Add(new GebruikerDto(user.Id, user.Email, roles.ToList()));
            }

            return userDtos;
        }

        public async Task<IList<LidDto>> GetLeden()
        {
            var leden = await _dbContext
                .Leden
                .Include(lid => lid.Tak)
                .OrderBy(lid => lid.Achternaam)
                .ToListAsync();

            var lidDtos = leden == null
                ? new List<LidDto>()
                : leden.Select(lid => new LidDto(
                    lid.Id,
                    lid.Voornaam,
                    lid.Achternaam,
                    lid.Functie,
                    lid.Tak?.Naam ?? "Onbekend"
                )).ToList();

            return lidDtos;
        }

        public async Task<IList<ScoutsjaarDto>> GetScoutsjaren()
        {
            var scoutsjaren = await _dbContext
                .Scoutsjaren
                .OrderBy(scoutsjaar => scoutsjaar.Begin)
                .ToListAsync();

            var scoutsjaarDtos = scoutsjaren == null
                ? new List<ScoutsjaarDto>()
                : scoutsjaren.Select(scoutsjaar => new ScoutsjaarDto(scoutsjaar.Begin, scoutsjaar.PannenkoekenPerPak)).ToList();

            return scoutsjaarDtos;
        }

        public async Task<IList<StraatDto>> GetStraten()
        {
            var straten = await _dbContext
                .Straten
                .Include(straat => straat.Zone)
                .OrderBy(straat => straat.Naam)
                .ToListAsync();

            var straatDtos = straten == null
                ? new List<StraatDto>()
                : straten.Select(straat => new StraatDto(
                    straat.Id,
                    straat.Naam,
                    straat.PostNummer,
                    straat.Gemeente,
                    straat.Omschrijving,
                    straat.ZoneId,
                    straat.Zone?.Naam ?? "",
                    straat.Nummer)).ToList();

            return straatDtos;
        }
    }
}
