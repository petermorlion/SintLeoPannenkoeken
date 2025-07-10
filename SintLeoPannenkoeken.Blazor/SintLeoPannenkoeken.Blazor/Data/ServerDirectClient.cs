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

        public ServerDirectClient(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
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
    }
}
