using Microsoft.EntityFrameworkCore;
using SintLeoPannenkoeken.Blazor.Client.Server.Contracts;
using SintLeoPannenkoeken.Blazor.Data;
using SintLeoPannenkoeken.Blazor.Models;

namespace SintLeoPannenkoeken.Blazor.External.SintLeoWebsite
{
    public class OnlineOrderImporter
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
        private readonly SintLeoWebsiteService _sintLeoWebsiteService;
        private readonly ILogger<OnlineOrderImporter> _logger;

        public OnlineOrderImporter(
            IDbContextFactory<ApplicationDbContext> dbContextFactory,
            SintLeoWebsiteService sintLeoWebsiteService,
            ILogger<OnlineOrderImporter> logger)
        {
            _dbContextFactory = dbContextFactory;
            _sintLeoWebsiteService = sintLeoWebsiteService;
            _logger = logger;
        }

        public async Task<IList<PendingOnlineBestellingDto>> GetPendingOnlineBestellingen(int scoutsjaarBegin)
        {
            var onlineBestellingen = await _sintLeoWebsiteService.GetOnlineBestelingen(scoutsjaarBegin);
            
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var scoutsjaar = await dbContext.Scoutsjaren.SingleOrDefaultAsync(sj => sj.Begin == scoutsjaarBegin);
                if (scoutsjaar == null)
                {
                    _logger.LogWarning("Scoutsjaar {ScoutsjaarBegin} not found", scoutsjaarBegin);
                    return new List<PendingOnlineBestellingDto>();
                }

                // Load reference data once
                var straten = await dbContext.Straten.ToListAsync();
                var leden = await dbContext.Leden.Include(l => l.Tak).ToListAsync();
                var takken = await dbContext.Takken.ToListAsync();

                var pendingOnlineBestellingen = new List<PendingOnlineBestellingDto>();
                var hangendeBestellingen = new List<HangendeBestelling>();

                foreach (var onlineBestelling in onlineBestellingen)
                {
                    if (await IsAlreadyImported(dbContext, onlineBestelling.Id, scoutsjaar.Id))
                    {
                        continue; // Already imported
                    }

                    var (straatMatch, lidMatch) = MatchOrder(onlineBestelling, straten, leden);
                    var takMatch = MatchTak(onlineBestelling.Tak, takken);

                    if (straatMatch != null && lidMatch != null)
                    {
                        // Full match - add to pending with proposed IDs
                        onlineBestelling.ProposedLidId = lidMatch.Id;
                        onlineBestelling.ProposedStraatId = straatMatch.Id;
                        onlineBestelling.ProposedTakId = takMatch?.Id;
                        pendingOnlineBestellingen.Add(onlineBestelling);
                    }
                    else
                    {
                        // Partial or no match - store in HangendeBestellingen
                        var hangendeBestelling = CreateHangendeBestelling(
                            onlineBestelling, scoutsjaar.Id, straatMatch?.Id, lidMatch?.Id, takMatch?.Id);
                        hangendeBestellingen.Add(hangendeBestelling);
                    }
                }

                await SaveHangendeBestellingen(dbContext, hangendeBestellingen, scoutsjaar.Id);

                return pendingOnlineBestellingen;
            }
        }

        private async Task<bool> IsAlreadyImported(ApplicationDbContext dbContext, int onlineBestellingId, int scoutsjaarId)
        {
            return await dbContext.Bestellingen
                .AnyAsync(b => b.OnlineBestellingId == onlineBestellingId && b.ScoutsjaarId == scoutsjaarId);
        }

        private (Straat?, Lid?) MatchOrder(PendingOnlineBestellingDto onlineBestelling, List<Straat> straten, List<Lid> leden)
        {
            // Try to match street (lowercase + trimmed + postcode)
            var straatNaam = onlineBestelling.Straat?.ToLower().Trim() ?? "";
            var postcode = onlineBestelling.Postcode?.Trim() ?? "";
            var straatMatch = straten.FirstOrDefault(s => 
                s.Naam.ToLower().Trim() == straatNaam && s.PostNummer.ToString() == postcode);

            // Try to match member (firstname + lastname lowercase + trimmed + Tak)
            var lidNaamParts = onlineBestelling.Naam?.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
            var lidVoornaam = lidNaamParts.Length > 0 ? lidNaamParts[0].ToLower().Trim() : "";
            var lidAchternaam = lidNaamParts.Length > 1 ? string.Join(" ", lidNaamParts.Skip(1)).ToLower().Trim() : "";
            var takNaam = onlineBestelling.Tak?.Trim() ?? "";

            Lid lidMatch = null;
            if (!string.IsNullOrEmpty(lidVoornaam) && !string.IsNullOrEmpty(lidAchternaam))
            {
                lidMatch = leden.FirstOrDefault(l => 
                    l.Voornaam.ToLower().Trim() == lidVoornaam && 
                    l.Achternaam.ToLower().Trim() == lidAchternaam &&
                    l.Tak.Naam.Trim() == takNaam);
            }

            return (straatMatch, lidMatch);
        }

        private Tak? MatchTak(string takName, List<Tak> takken)
        {
            if (string.IsNullOrEmpty(takName))
            {
                return null;
            }
            return takken.FirstOrDefault(t => t.Naam.Trim() == takName.Trim());
        }

        private HangendeBestelling CreateHangendeBestelling(
            PendingOnlineBestellingDto onlineBestelling,
            int scoutsjaarId,
            int? proposedStraatId,
            int? proposedLidId,
            int? proposedTakId)
        {
            return new HangendeBestelling
            {
                OnlineBestellingId = onlineBestelling.Id,
                Naam = onlineBestelling.Naam,
                Telefoon = onlineBestelling.Telefoon,
                Email = onlineBestelling.Email,
                AantalPakken = onlineBestelling.AantalPakken,
                Opmerkingen = onlineBestelling.Opmerkingen,
                Betaald = onlineBestelling.Betaald,
                Geleverd = onlineBestelling.Geleverd,
                Straat = onlineBestelling.Straat,
                Nummer = onlineBestelling.Nummer,
                Bus = onlineBestelling.Bus,
                Postcode = onlineBestelling.Postcode,
                Gemeente = onlineBestelling.Gemeente,
                LidNaam = onlineBestelling.Lid,
                Tak = onlineBestelling.Tak,
                Levering = onlineBestelling.Levering,
                DatumAangemaakt = onlineBestelling.DatumAangemaakt,
                ScoutsjaarId = scoutsjaarId,
                ProposedStraatId = proposedStraatId,
                ProposedLidId = proposedLidId,
                ProposedTakId = proposedTakId,
                Verwerkt = false,
                RawData = onlineBestelling.RawData
            };
        }

        private async Task SaveHangendeBestellingen(ApplicationDbContext dbContext, List<HangendeBestelling> hangendeBestellingen, int scoutsjaarId)
        {
            if (hangendeBestellingen.Count == 0)
            {
                return;
            }

            // Check for existing entries to avoid duplicates
            var existingHangendeIds = await dbContext.HangendeBestellingen
                .Where(h => h.ScoutsjaarId == scoutsjaarId)
                .Select(h => h.OnlineBestellingId)
                .ToListAsync();

            var newHangende = hangendeBestellingen
                .Where(h => !existingHangendeIds.Contains(h.OnlineBestellingId))
                .ToList();

            if (newHangende.Count > 0)
            {
                _logger.LogInformation("Saving {Count} new hangende bestellingen", newHangende.Count);
                await dbContext.HangendeBestellingen.AddRangeAsync(newHangende);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<IList<PendingOnlineBestellingDto>> GetHangendeBestellingen(int scoutsjaarBegin)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var scoutsjaar = await dbContext.Scoutsjaren.SingleOrDefaultAsync(sj => sj.Begin == scoutsjaarBegin);
                if (scoutsjaar == null)
                {
                    _logger.LogWarning("Scoutsjaar {ScoutsjaarBegin} not found for hangende bestellingen", scoutsjaarBegin);
                    return new List<PendingOnlineBestellingDto>();
                }

                var hangendeBestellingen = await dbContext.HangendeBestellingen
                    .Where(h => h.ScoutsjaarId == scoutsjaar.Id && !h.Verwerkt)
                    .OrderBy(h => h.DatumAangemaakt)
                    .ToListAsync();

                return hangendeBestellingen.Select(ConvertToPendingOnlineBestellingDto).ToList();
            }
        }

        private PendingOnlineBestellingDto ConvertToPendingOnlineBestellingDto(HangendeBestelling h)
        {
            return new PendingOnlineBestellingDto
            {
                Id = h.OnlineBestellingId,
                Naam = h.Naam,
                Telefoon = h.Telefoon,
                Email = h.Email,
                AantalPakken = h.AantalPakken,
                Opmerkingen = h.Opmerkingen,
                Betaald = h.Betaald,
                Geleverd = h.Geleverd,
                Straat = h.Straat,
                Nummer = h.Nummer,
                Bus = h.Bus,
                Postcode = h.Postcode,
                Gemeente = h.Gemeente,
                Lid = h.LidNaam,
                Tak = h.Tak,
                Levering = h.Levering,
                DatumAangemaakt = h.DatumAangemaakt,
                ProposedLidId = h.ProposedLidId,
                ProposedStraatId = h.ProposedStraatId,
                ProposedTakId = h.ProposedTakId,
                Verwerkt = h.Verwerkt,
                RawData = h.RawData
            };
        }
    }
}
