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

        public async Task<int> GetPendingOnlineBestellingen(int scoutsjaarBegin)
        {
            var onlineBestellingen = await _sintLeoWebsiteService.GetOnlineBestelingen(scoutsjaarBegin);
            
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var scoutsjaar = await dbContext.Scoutsjaren.SingleOrDefaultAsync(sj => sj.Begin == scoutsjaarBegin);
                if (scoutsjaar == null)
                {
                    _logger.LogWarning("Scoutsjaar {ScoutsjaarBegin} not found", scoutsjaarBegin);
                    return 0;
                }

                // Load reference data once
                var straten = await dbContext.Straten.ToListAsync();
                var leden = await dbContext.Leden.Include(l => l.Tak).ToListAsync();

                var hangendeBestellingen = new List<HangendeBestelling>();
                int successfullyImportedCount = 0;

                foreach (var onlineBestelling in onlineBestellingen)
                {
                    if (await IsAlreadyImported(dbContext, onlineBestelling.Id, scoutsjaar.Id))
                    {
                        continue; // Already imported
                    }

                    var (straatMatch, lidMatch) = MatchOrder(onlineBestelling, straten, leden);

                    if (straatMatch != null && lidMatch != null)
                    {
                        // Full match - save to Bestellingen table
                        await SaveMatchedBestelling(dbContext, onlineBestelling, scoutsjaar.Id, straatMatch.Id, lidMatch.Id);
                        successfullyImportedCount++;
                    }
                    else
                    {
                        // Partial or no match - store in HangendeBestellingen
                        var hangendeBestelling = CreateHangendeBestelling(
                            onlineBestelling, scoutsjaar.Id, straatMatch?.Id, lidMatch?.Id);
                        hangendeBestellingen.Add(hangendeBestelling);
                    }
                }

                await SaveHangendeBestellingen(dbContext, hangendeBestellingen, scoutsjaar.Id);

                return successfullyImportedCount;
            }
        }

        private async Task SaveMatchedBestelling(ApplicationDbContext dbContext, PendingOnlineBestellingDto onlineBestelling, int scoutsjaarId, int straatId, int lidId)
        {
            var takId = await GetTakIdForLid(dbContext, lidId);
            
            var bestelling = new Bestelling(onlineBestelling.Naam, onlineBestelling.AantalPakken)
            {
                OnlineBestellingId = onlineBestelling.Id,
                Telefoon = onlineBestelling.Telefoon ?? "",
                Opmerkingen = onlineBestelling.Opmerkingen ?? "",
                Betaald = onlineBestelling.Betaald,
                Geleverd = onlineBestelling.Geleverd,
                LidId = lidId,
                StraatId = straatId,
                TakId = takId,
                Nummer = onlineBestelling.Nummer ?? "",
                Bus = onlineBestelling.Bus ?? "",
                IngaveDatum = onlineBestelling.DatumAangemaakt ?? DateTime.Now,
                ScoutsjaarId = scoutsjaarId
            };

            dbContext.Bestellingen.Add(bestelling);
            await dbContext.SaveChangesAsync();
            
            _logger.LogInformation("Saved matched bestelling: OnlineBestellingId={OnlineBestellingId}, Naam={Naam}, Straat={Straat}, AantalPakken={AantalPakken}",
                onlineBestelling.Id, onlineBestelling.Naam, onlineBestelling.Straat, onlineBestelling.AantalPakken);
        }
        
        private async Task<int> GetTakIdForLid(ApplicationDbContext dbContext, int lidId)
        {
            var lid = await dbContext.Leden.Include(l => l.Tak).FirstOrDefaultAsync(l => l.Id == lidId);
            return lid?.TakId ?? 0;
        }

        private async Task<bool> IsAlreadyImported(ApplicationDbContext dbContext, int onlineBestellingId, int scoutsjaarId)
        {
            return await dbContext.Bestellingen
                .AnyAsync(b => b.OnlineBestellingId == onlineBestellingId && b.ScoutsjaarId == scoutsjaarId);
        }

        internal (Straat?, Lid?) MatchOrder(PendingOnlineBestellingDto onlineBestelling, List<Straat> straten, List<Lid> leden)
        {
            // Try to match street (lowercase + trimmed + postcode)
            var straatNaam = onlineBestelling.Straat?.ToLower().Trim() ?? "";
            var postcode = onlineBestelling.Postcode?.Trim() ?? "";
            var straatMatch = straten.FirstOrDefault(s => 
                (s.Naam?.ToLower().Trim() ?? "") == straatNaam && s.PostNummer.ToString() == postcode);

            // Try to match member (firstname + lastname or lastname + firstname, lowercase + trimmed + Tak)
            var lidNaamParts = onlineBestelling.Lid?.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
            var takNaam = onlineBestelling.Tak?.ToLower().Trim() ?? "";

            Lid lidMatch = null;
            if (lidNaamParts.Length >= 2)
            {
                // Try all possible splits for firstname/lastname in both orders
                // For "First Middle Last", try:
                // - First + Middle Last (firstname, lastname)
                // - Middle Last + First (lastname, firstname)
                // - First Middle + Last (firstname, lastname)
                // - Last + First Middle (lastname, firstname)
                
                // Try splitting at position 1: first part as firstname, rest as lastname
                for (int splitIndex = 1; splitIndex < lidNaamParts.Length; splitIndex++)
                {
                    var voornaam = string.Join(" ", lidNaamParts.Take(splitIndex)).ToLower().Trim();
                    var achternaam = string.Join(" ", lidNaamParts.Skip(splitIndex)).ToLower().Trim();
                    
                    lidMatch = leden.FirstOrDefault(l => 
                        (l.Voornaam?.ToLower().Trim() ?? "") == voornaam && 
                        (l.Achternaam?.ToLower().Trim() ?? "") == achternaam &&
                        (l.Tak?.Naam?.ToLower().Trim() ?? "") == takNaam);
                    
                    if (lidMatch != null)
                        break;
                    
                    // Try reversed: first part as lastname, rest as firstname
                    lidMatch = leden.FirstOrDefault(l => 
                        (l.Voornaam?.ToLower().Trim() ?? "") == achternaam && 
                        (l.Achternaam?.ToLower().Trim() ?? "") == voornaam &&
                        (l.Tak?.Naam?.ToLower().Trim() ?? "") == takNaam);
                    
                    if (lidMatch != null)
                        break;
                }
            }

            return (straatMatch, lidMatch);
        }

        private HangendeBestelling CreateHangendeBestelling(
            PendingOnlineBestellingDto onlineBestelling,
            int scoutsjaarId,
            int? proposedStraatId,
            int? proposedLidId)
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
                Verwerkt = h.Verwerkt,
                RawData = h.RawData
            };
        }
    }
}
