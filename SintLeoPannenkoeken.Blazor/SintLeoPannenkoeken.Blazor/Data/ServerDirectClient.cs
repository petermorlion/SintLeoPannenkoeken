using Microsoft.EntityFrameworkCore;
using SintLeoPannenkoeken.Blazor.Client.Pages.Beheer;
using SintLeoPannenkoeken.Blazor.Client.Server;
using SintLeoPannenkoeken.Blazor.Client.Server.Contracts;
using SintLeoPannenkoeken.Blazor.External.Geocoding;
using SintLeoPannenkoeken.Blazor.External.TourPlanning;
using SintLeoPannenkoeken.Blazor.Models;

namespace SintLeoPannenkoeken.Blazor.Data
{
    /// <summary>
    /// Direct access to server data. To be used in the server-side Blazor application.
    /// </summary>
    public class ServerDirectClient : IServerData
    {
        private readonly ILogger<ServerDirectClient> _logger;
        private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
        private readonly UsersService _usersService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HereGeocodingService _hereGeoCodingService;
        private readonly HereTourPlanningService _hereTourPlanningService;

        public ServerDirectClient(ILogger<ServerDirectClient> logger,
            IDbContextFactory<ApplicationDbContext> dbContextFactory,
            UsersService usersService, IHttpContextAccessor httpContextAccessor,
            HereGeocodingService hereGeoCodingService,
            HereTourPlanningService hereTourPlanningService)
        {
            _logger = logger;
            _dbContextFactory = dbContextFactory;
            _usersService = usersService;
            _httpContextAccessor = httpContextAccessor;
            _hereGeoCodingService = hereGeoCodingService;
            _hereTourPlanningService = hereTourPlanningService;
        }

        public async Task<IList<GebruikerDto>> GetGebruikers()
        {
            return await _usersService.GetGebruikers();
        }

        public async Task<IList<LidDto>> GetLeden()
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var leden = await dbContext
                .Leden
                .Include(lid => lid.Tak)
                .OrderBy(lid => lid.Achternaam)
                .ToListAsync();

                var lidDtos = leden == null
                    ? new List<LidDto>()
                    : leden.Select(lid => new LidDto(
                        lid.Achternaam,
                        lid.Voornaam,
                        lid.Functie,
                        lid.Tak?.Naam ?? "Onbekend",
                        lid.Id
                    )).ToList();

                return lidDtos;
            }
        }

        public async Task<IList<ScoutsjaarDto>> GetScoutsjaren()
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var scoutsjaren = await dbContext
                .Scoutsjaren
                .OrderByDescending(scoutsjaar => scoutsjaar.Begin)
                .ToListAsync();

                var scoutsjaarDtos = scoutsjaren == null
                    ? new List<ScoutsjaarDto>()
                    : scoutsjaren.Select(scoutsjaar => new ScoutsjaarDto(
                        scoutsjaar.Begin,
                        scoutsjaar.PannenkoekenPerPak,
                        (ScoutsjaarStatusDto)scoutsjaar.Status,
                        scoutsjaar.Id)).ToList();

                return scoutsjaarDtos;
            }
        }

        public async Task<StraatDto> CreateStraat(StraatDto straatDto)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var straat = new Straat(straatDto.Naam, straatDto.Gemeente)
                {
                    PostNummer = straatDto.PostNummer,
                    Omschrijving = straatDto.Omschrijving,
                    Nummer = straatDto.Nummer,
                    ZoneId = straatDto.ZoneId
                };

                dbContext.Straten.Add(straat);

                await dbContext.SaveChangesAsync();

                var zoneNaam = (await dbContext.Zones.SingleAsync(z => z.Id == straat.ZoneId)).Naam;

                return new StraatDto
                {
                    Id = straat.Id,
                    Naam = straat.Naam,
                    PostNummer = straat.PostNummer,
                    Gemeente = straat.Gemeente,
                    Omschrijving = straat.Omschrijving,
                    ZoneId = straat.ZoneId,
                    ZoneNaam = zoneNaam,
                    Nummer = straat.Nummer
                };
            }
        }

        public async Task<IList<StraatDto>> GetStraten()
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var straten = await dbContext
                .Straten
                .Include(straat => straat.Zone)
                .OrderBy(straat => straat.Naam)
                .ToListAsync();

                var straatDtos = straten == null
                    ? new List<StraatDto>()
                    : straten.Select(straat => new StraatDto
                    {
                        Id = straat.Id,
                        Naam = straat.Naam,
                        PostNummer = straat.PostNummer,
                        Gemeente = straat.Gemeente,
                        Omschrijving = straat.Omschrijving,
                        ZoneId = straat.ZoneId,
                        ZoneNaam = straat.Zone?.Naam ?? "",
                        Nummer = straat.Nummer
                    }).ToList();

                return straatDtos;
            }
        }

        public async Task<IList<ChauffeurDto>> GetChauffeurs(int scoutsjaar)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var chauffeurs = await dbContext
                .Bestuurders
                .OrderBy(straat => straat.Achternaam)
                .ToListAsync();

                var data = await dbContext
                    .Scoutsjaren
                    .Include(sj => sj.Bestellingen)
                    .ThenInclude(b => b.Straat)
                    .Include(sj => sj.Rondes)
                    .SingleOrDefaultAsync(sj => sj.Begin == scoutsjaar);

                var bestellingenPerZone = data
                    .Bestellingen
                    .GroupBy(b => b.Straat.ZoneId)
                    .ToDictionary(group => group.Key, group => group.ToList());

                var zonesPerChauffeur = data
                    .Rondes
                    .GroupBy(r => r.BestuurderId, r => r.ZoneId)
                    .ToDictionary(group => group.Key, group => group.ToList());

                var chauffeurDtos = chauffeurs == null
                    ? new List<ChauffeurDto>()
                    : chauffeurs.Select(chauffeur =>
                    {
                        var aantalPakken = 0;
                        var aantalBestellingen = 0;
                        var zonesForChauffeur = zonesPerChauffeur.ContainsKey(chauffeur.Id) ? zonesPerChauffeur[chauffeur.Id] : new List<int>();
                        foreach (var zoneId in zonesForChauffeur)
                        {
                            var bestellingenForZone = bestellingenPerZone.ContainsKey(zoneId) ? bestellingenPerZone[zoneId] : new List<Bestelling>();
                            aantalPakken += bestellingenForZone.Sum(x => x.AantalPakken);
                            aantalBestellingen += bestellingenForZone.Count;
                        }

                        return new ChauffeurDto
                        {
                            Id = chauffeur.Id,
                            Voornaam = chauffeur.Voornaam,
                            Achternaam = chauffeur.Achternaam,
                            AantalBestellingen = aantalBestellingen,
                            AantalPakken = aantalPakken
                        };
                    }).ToList();

                return chauffeurDtos;
            }
        }

        public async Task<ChauffeurDto> GetChauffeur(int scoutsjaar, int chauffeurId)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var chauffeur = await dbContext
                    .Bestuurders
                    .SingleOrDefaultAsync(x => x.Id == chauffeurId);

                if (chauffeur == null)
                {
                    return null;
                }

                var data = await dbContext
                    .Scoutsjaren
                    .Include(sj => sj.Bestellingen)
                    .ThenInclude(b => b.Straat)
                    .Include(sj => sj.Rondes)
                    .SingleOrDefaultAsync(sj => sj.Begin == scoutsjaar);

                var bestellingenPerZone = data
                    .Bestellingen
                    .GroupBy(b => b.Straat.ZoneId)
                    .ToDictionary(group => group.Key, group => group.ToList());

                var zonesPerChauffeur = data
                    .Rondes
                    .GroupBy(r => r.BestuurderId, r => r.ZoneId)
                    .ToDictionary(group => group.Key, group => group.ToList());

                var aantalPakken = 0;
                var aantalBestellingen = 0;
                var zonesForChauffeur = zonesPerChauffeur.ContainsKey(chauffeur.Id) ? zonesPerChauffeur[chauffeur.Id] : new List<int>();
                foreach (var zoneId in zonesForChauffeur)
                {
                    var bestellingenForZone = bestellingenPerZone.ContainsKey(zoneId) ? bestellingenPerZone[zoneId] : new List<Bestelling>();
                    aantalPakken += bestellingenForZone.Sum(x => x.AantalPakken);
                    aantalBestellingen += bestellingenForZone.Count;
                }

                return new ChauffeurDto
                {
                    Id = chauffeur.Id,
                    Voornaam = chauffeur.Voornaam,
                    Achternaam = chauffeur.Achternaam,
                    AantalBestellingen = aantalBestellingen,
                    AantalPakken = aantalPakken
                };
            }
        }

        public async Task DeleteRonde(int rondeId)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                await dbContext.Rondes.Where(s => s.Id == rondeId).ExecuteDeleteAsync();
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateScoutsjaar(ScoutsjaarDto scoutsjaarDto)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var scoutsjaar = await dbContext
                .Scoutsjaren
                .FirstOrDefaultAsync(s => s.Begin == scoutsjaarDto.Begin);

                if (scoutsjaar == null)
                {
                    scoutsjaar = new Scoutsjaar(scoutsjaarDto.Begin, scoutsjaarDto.PannenkoekenPerPak);
                    dbContext.Scoutsjaren.Add(scoutsjaar);
                }
                else
                {
                    scoutsjaar.PannenkoekenPerPak = scoutsjaarDto.PannenkoekenPerPak;
                    scoutsjaar.Status = (ScoutsjaarStatus)scoutsjaarDto.Status;
                }

                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<IList<BestellingDto>> GetBestellingen(int begin)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var bestellingen = (await dbContext.Scoutsjaren
                 .Include(scoutsjaar => scoutsjaar.Bestellingen)
                 .ThenInclude(bestelling => bestelling.Straat)
                 .ThenInclude(straat => straat.Zone)
                 .Include(scoutsjaar => scoutsjaar.Bestellingen)
                 .ThenInclude(bestelling => bestelling.Lid)
                 .ThenInclude(lid => lid.Tak)
                 .SingleOrDefaultAsync(scoutsjaar => scoutsjaar.Begin == begin))
                 ?.Bestellingen
                 ?.Where(bestelling => bestelling.DeletedOn == null)
                 ?.OrderByDescending(bestelling => bestelling.BestellingNummer)
                 ?.ToList();

                var bestellingenDtos = bestellingen == null
                    ? new List<BestellingDto>()
                    : bestellingen.Select(bestelling => new BestellingDto
                    {
                        Id = bestelling.Id,
                        BestellingNummer = bestelling.BestellingNummer,
                        Naam = bestelling.Naam,
                        AantalPakken = bestelling.AantalPakken,
                        Telefoon = bestelling.Telefoon,
                        Opmerkingen = bestelling.Opmerkingen,
                        Betaald = bestelling.Betaald,
                        Geleverd = bestelling.Geleverd,
                        Lid = new LidDto
                        (
                            bestelling.Lid.Achternaam,
                            bestelling.Lid.Voornaam,
                            bestelling.Lid.Functie,
                            bestelling.Lid.Tak.Naam
                        ),
                        StraatId = bestelling.StraatId,
                        Nummer = bestelling.Nummer,
                        Bus = bestelling.Bus
                    }).ToList();

                return bestellingenDtos;
            }
        }

        public async Task UpdateBestelling(UpdateBestellingDto bestellingDto)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var bestelling = await dbContext.Bestellingen.SingleOrDefaultAsync(bestelling => bestelling.Id == bestellingDto.Id);
                if (bestelling == null)
                {
                    return;
                }

                var takId = (await dbContext.Leden.SingleAsync(lid => lid.Id == bestellingDto.LidId)).TakId;

                bestelling.Naam = bestellingDto.Naam;
                bestelling.AantalPakken = bestellingDto.AantalPakken;
                bestelling.Telefoon = bestellingDto.Telefoon != null ? bestellingDto.Telefoon : "";
                bestelling.Opmerkingen = bestellingDto.Opmerkingen != null ? bestellingDto.Opmerkingen : "";
                bestelling.Betaald = bestellingDto.Betaald;
                bestelling.Geleverd = bestellingDto.Geleverd;
                bestelling.LidId = bestellingDto.LidId;
                bestelling.TakId = takId;
                bestelling.StraatId = bestellingDto.StraatId;
                bestelling.Nummer = bestellingDto.Nummer;
                bestelling.Bus = bestellingDto.Bus;
            }
        }

        public async Task UpdateLid(LidDto lidDto)
        {
            if (!lidDto.Id.HasValue)
            {
                throw new ArgumentException("Lid ID must be provided for update.");
            }

            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var lid = await dbContext
                        .Leden
                        .Include(l => l.Tak)
                        .FirstOrDefaultAsync(s => s.Id == lidDto.Id.Value);

                if (lid == null)
                {
                    throw new ArgumentException($"Lid with ID {lidDto.Id.Value} not found.");
                }

                lid.Voornaam = lidDto.Voornaam;
                lid.Achternaam = lidDto.Achternaam;
                lid.Functie = lidDto.Functie;

                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<IList<TakDto>> GetTakken()
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var takken = await dbContext
                .Takken
                .OrderBy(tak => tak.Id)
                .ToListAsync();

                var takDtos = takken == null
                    ? new List<TakDto>()
                    : takken.Select(tak => new TakDto(
                        tak.Id,
                        tak.Naam)).ToList();

                return takDtos;
            }
        }

        public async Task<LidDto> CreateLid(NewLidDto lidDto)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var lid = new Lid(lidDto.Voornaam, lidDto.Achternaam);
                lid.Functie = lidDto.Functie;
                lid.TakId = lidDto.TakId;
                dbContext.Leden.Add(lid);

                await dbContext.SaveChangesAsync();

                lid = await dbContext
                    .Leden
                    .Include(l => l.Tak)
                    .FirstAsync(l => l.Id == lid.Id);

                return new LidDto(lid.Achternaam, lid.Voornaam, lid.Functie, lid.Tak.Naam, lid.Id);
            }
        }

        public async Task<BestellingDto> CreateBestelling(NewBestellingDto bestellingDto)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var takId = (await dbContext.Leden.SingleAsync(lid => lid.Id == bestellingDto.LidId)).TakId;

                var bestelling = new Bestelling(bestellingDto.Naam, bestellingDto.AantalPakken);
                bestelling.Telefoon = bestellingDto.Telefoon ?? "";
                bestelling.Opmerkingen = bestellingDto.Opmerkingen ?? "";
                bestelling.Betaald = bestellingDto.Betaald;
                bestelling.Geleverd = bestellingDto.Geleverd;
                bestelling.LidId = bestellingDto.LidId;
                bestelling.TakId = takId;
                bestelling.StraatId = bestellingDto.StraatId;
                bestelling.Nummer = bestellingDto.Nummer ?? "";
                bestelling.Bus = bestellingDto.Bus ?? "";
                bestelling.IngaveDatum = DateTime.Now;
                bestelling.ScoutsjaarId = bestellingDto.ScoutsjaarId;

                var lastBestelling = await dbContext.Bestellingen
                    .Where(b => b.ScoutsjaarId == bestellingDto.ScoutsjaarId)
                    .OrderByDescending(b => b.Id)
                    .FirstOrDefaultAsync();

                // I totally realize this could lead to duplicate bestellingNummers, but the chances are slim
                // because only about 2 users (maximum) will be active at the same time.
                var bestellingNummer = lastBestelling != null ? lastBestelling.BestellingNummer + 1 : 1;
                bestelling.BestellingNummer = bestellingNummer;

                dbContext.Bestellingen.Add(bestelling);

                await dbContext.SaveChangesAsync();

                bestelling = await dbContext.Bestellingen
                    .Include(b => b.Tak)
                    .Include(b => b.Lid)
                    .Include(b => b.Straat)
                    .ThenInclude(s => s.Zone)
                    .SingleAsync(b => b.Id == bestelling.Id);

                var geocodes = await _hereGeoCodingService.GetGeocode(bestelling.Straat.Naam, bestelling.Nummer, bestelling.Straat.Zone?.PostNummer.ToString() ?? "", bestelling.Straat.Zone.Gemeente);
                var geocode = geocodes.Items.FirstOrDefault();
                if (geocode != null)
                {
                    bestelling.Latitude = geocode.Position.Lat;
                    bestelling.Longitude = geocode.Position.Lng;
                    await dbContext.SaveChangesAsync();
                }

                return new BestellingDto
                {
                    Id = bestelling.Id,
                    BestellingNummer = bestelling.BestellingNummer,
                    Naam = bestelling.Naam,
                    AantalPakken = bestelling.AantalPakken,
                    Telefoon = bestelling.Telefoon,
                    Opmerkingen = bestelling.Opmerkingen,
                    Betaald = bestelling.Betaald,
                    Geleverd = bestelling.Geleverd,
                    Lid = new LidDto(bestelling.Lid.Achternaam, bestelling.Lid.Voornaam, bestelling.Lid.Functie, bestelling.Lid.Tak?.Naam ?? "Onbekend", bestelling.Lid.Id),
                    StraatId = bestelling.StraatId,
                    Nummer = bestelling.Nummer,
                    Bus = bestelling.Bus
                };
            }
        }

        public async Task<IList<StreefcijferDto>> GetStreefcijfers(int jaar)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var scoutsjaar = await dbContext.Scoutsjaren
                                        .Include(scoutsjaar => scoutsjaar.StreefCijfers)
                                        .ThenInclude(streefCijfer => streefCijfer.Tak)
                                        .SingleOrDefaultAsync(scoutsjaar => scoutsjaar.Begin == jaar);

                var streefCijfers = scoutsjaar
                    ?.StreefCijfers
                    ?.ToList();

                if (streefCijfers == null)
                {
                    return new List<StreefcijferDto>();
                }

                return streefCijfers.Select(x => new StreefcijferDto
                {
                    Id = x.Id,
                    TakId = x.TakId,
                    TakNaam = x.Tak?.Naam ?? "Onbekend",
                    Aantal = x.Aantal,
                    ScoutsjaarId = scoutsjaar.Id
                }).ToList();
            }
        }

        public async Task<StreefcijferDto> CreateStreefcijfer(StreefcijferDto streefcijferDto)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var scoutsjaar = await dbContext
                    .Scoutsjaren
                    .Include(s => s.StreefCijfers)
                    .FirstOrDefaultAsync(s => s.Id == streefcijferDto.ScoutsjaarId);

                var existingStreefcijfer = scoutsjaar?.StreefCijfers.FirstOrDefault(sc => sc.TakId == streefcijferDto.TakId);

                if (existingStreefcijfer != null)
                {
                    throw new ArgumentException($"Er bestaat al een streefcijfer voor deze tak en scoutsjaar.");
                }

                var streefcijfer = new StreefCijfer
                {
                    TakId = streefcijferDto.TakId,
                    Aantal = streefcijferDto.Aantal
                };

                scoutsjaar.StreefCijfers.Add(streefcijfer);

                await dbContext.SaveChangesAsync();

                streefcijfer = await dbContext
                    .StreefCijfers
                    .Include(sc => sc.Tak)
                    .FirstAsync(sc => sc.Id == streefcijfer.Id);

                return new StreefcijferDto
                {
                    Id = streefcijfer.Id,
                    TakId = streefcijfer.TakId,
                    TakNaam = streefcijfer.Tak?.Naam ?? "Onbekend",
                    Aantal = streefcijfer.Aantal,
                    ScoutsjaarId = scoutsjaar.Id
                };
            }
        }

        public async Task UpdateStreefcijfer(StreefcijferDto streefcijferDto)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var streefcijfer = await dbContext
                        .StreefCijfers
                        .Include(l => l.Tak)
                        .FirstOrDefaultAsync(s => s.Id == streefcijferDto.Id);

                if (streefcijfer == null)
                {
                    throw new ArgumentException($"Lid with ID {streefcijferDto.Id} not found.");
                }

                streefcijfer.TakId = streefcijferDto.TakId;
                streefcijfer.Aantal = streefcijferDto.Aantal;

                await dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteStreefcijfer(int streefcijferId)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                await dbContext.StreefCijfers.Where(s => s.Id == streefcijferId).ExecuteDeleteAsync();
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<ChauffeurDto> CreateChauffeur(ChauffeurDto chauffeurDto)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var chauffeur = new Bestuurder
                {
                    Achternaam = chauffeurDto.Achternaam,
                    Voornaam = chauffeurDto.Voornaam
                };

                dbContext.Bestuurders.Add(chauffeur);

                await dbContext.SaveChangesAsync();

                return new ChauffeurDto
                {
                    Id = chauffeur.Id,
                    Achternaam = chauffeur.Achternaam,
                    Voornaam = chauffeur.Voornaam
                };
            }
        }

        public async Task UpdateChauffeur(ChauffeurDto chauffeurDto)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var chauffeur = await dbContext
                        .Bestuurders
                        .SingleOrDefaultAsync(s => s.Id == chauffeurDto.Id);

                if (chauffeur == null)
                {
                    throw new ArgumentException($"Chauffeur with ID {chauffeurDto.Id} not found.");
                }

                chauffeur.Achternaam = chauffeurDto.Achternaam;
                chauffeur.Voornaam = chauffeurDto.Voornaam;

                await dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteChauffeur(int chauffeurId)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                await dbContext.Bestuurders.Where(s => s.Id == chauffeurId).ExecuteDeleteAsync();
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<GebruikerCreatedDto> CreateGebruiker(NewGebruikerDto gebruiker)
        {
            return await _usersService.CreateUser(gebruiker);
        }

        public async Task UpdateGebruiker(GebruikerDto gebruiker)
        {
            await _usersService.UpdateGebruiker(gebruiker);
        }

        public async Task DeleteGebruiker(string email)
        {
            await _usersService.DeleteGebruiker(email);
        }

        public async Task DeleteBestelling(int bestellingId)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var bestelling = await dbContext.Bestellingen.SingleOrDefaultAsync(b => b.Id == bestellingId);
                if (bestelling == null)
                {
                    return;
                }

                var user = _httpContextAccessor?.HttpContext?.User;
                if (user == null || user.Identity == null)
                {
                    _logger.LogError("No authentication state or user found when deleting bestelling.");
                    return;
                }

                bestelling.DeletedOn = DateTime.UtcNow;
                bestelling.DeletedBy = user.Identity.Name ?? "Unknown";
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<IList<ZoneDto>> GetZones()
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var zones = await dbContext
                .Zones
                .ToListAsync();

                var zoneDtos = zones == null
                    ? new List<ZoneDto>()
                    : zones.Select(zone => new ZoneDto
                    {
                        Id = zone.Id,
                        Naam = zone.Naam
                    }).ToList();

                return zoneDtos;
            }
        }

        public async Task<IList<RondeDto>> GetRondesForChauffeur(int chauffeurId, int scoutsjaarBegin)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var rondes = await dbContext.Scoutsjaren
                    .Where(sj => sj.Begin == scoutsjaarBegin)
                    .SelectMany(scoutsjaar => scoutsjaar.Rondes)
                    .Include(ronde => ronde.Zone)
                    .ThenInclude(zone => zone.Straten)
                    .Include(ronde => ronde.Bestuurder)
                    .Where(ronde => ronde.BestuurderId == chauffeurId)
                    .ToListAsync();

                var zoneIds = rondes.Select(ronde => ronde.ZoneId).ToList();

                var bestellingen = await dbContext.Scoutsjaren
                    .Where(sj => sj.Begin == scoutsjaarBegin)
                    .SelectMany(scoutsjaar => scoutsjaar.Bestellingen)
                    .Include(bestelling => bestelling.Straat)
                    .Where(bestelling => zoneIds.Contains(bestelling.Straat.ZoneId))
                    .ToListAsync();

                var rondeDtos = rondes == null
                    ? new List<RondeDto>()
                    : rondes.Select(ronde =>
                    {
                        var aantalPakken = bestellingen
                        .Where(bestelling => bestelling.Straat.ZoneId == ronde.ZoneId)
                        .Sum(bestelling => bestelling.AantalPakken);

                        return new RondeDto
                        {
                            Id = ronde.Id,
                            ChauffeurId = ronde.BestuurderId,
                            ChauffeurNaam = ronde.Bestuurder.Achternaam + " " + ronde.Bestuurder.Voornaam,
                            ZoneId = ronde.ZoneId,
                            ZoneNaam = ronde.Zone.Naam,
                            Gemeente = ronde.Zone.Gemeente,
                            Straten = ronde.Zone.Straten.Count,
                            Kaartnummer = ronde.Zone.KaartNummer ?? "",
                            ScoutsjaarId = ronde.ScoutsjaarId,
                            Pakken = aantalPakken
                        };
                    }).ToList();

                return rondeDtos;
            }
        }

        public async Task<RondeDto> CreateRonde(int chauffeurId, int scoutsjaarBegin, CreateRondeDto createRondeDto)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var scoutsjaarModel = await dbContext.Scoutsjaren.SingleAsync(sj => sj.Begin == scoutsjaarBegin);
                var existing = await dbContext.Rondes
                    .Include(ronde => ronde.Bestuurder)
                    .SingleOrDefaultAsync(ronde => ronde.ScoutsjaarId == scoutsjaarModel.Id
                        && ronde.ZoneId == createRondeDto.ZoneId);

                if (existing != null)
                {
                    throw new ArgumentException($"Deze zonde is al toegekend aan {existing.Bestuurder.Achternaam} {existing.Bestuurder.Voornaam}");
                }

                var ronde = new Ronde
                {
                    BestuurderId = chauffeurId,
                    ZoneId = createRondeDto.ZoneId,
                    ScoutsjaarId = scoutsjaarModel.Id
                };

                dbContext.Rondes.Add(ronde);
                await dbContext.SaveChangesAsync();

                ronde = await dbContext.Rondes
                    .Include(ronde => ronde.Zone)
                    .ThenInclude(zone => zone.Straten)
                    .Include(ronde => ronde.Bestuurder)
                    .SingleAsync(r => r.Id == ronde.Id);

                var aantalPakken = await dbContext.Scoutsjaren
                    .Where(sj => sj.Id == scoutsjaarModel.Id)
                    .SelectMany(scoutsjaar => scoutsjaar.Bestellingen)
                    .Include(bestelling => bestelling.Straat)
                    .Where(bestelling => bestelling.Straat.ZoneId == createRondeDto.ZoneId)
                    .SumAsync(x => x.AantalPakken);

                return new RondeDto
                {
                    Id = ronde.Id,
                    ChauffeurId = ronde.BestuurderId,
                    ChauffeurNaam = ronde.Bestuurder.Achternaam + " " + ronde.Bestuurder.Voornaam,
                    ZoneId = ronde.ZoneId,
                    ZoneNaam = ronde.Zone.Naam,
                    Gemeente = ronde.Zone.Gemeente,
                    Straten = ronde.Zone.Straten.Count,
                    ScoutsjaarId = ronde.ScoutsjaarId,
                    Pakken = aantalPakken
                };
            }
        }

        public async Task<IList<RondeDto>> GetRondes(int scoutsjaarBegin)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var rondes = await dbContext.Scoutsjaren
                    .Where(sj => sj.Begin == scoutsjaarBegin)
                    .SelectMany(scoutsjaar => scoutsjaar.Rondes)
                    .Include(ronde => ronde.Zone)
                    .ThenInclude(zone => zone.Straten)
                    .Include(ronde => ronde.Bestuurder)
                    .ToListAsync();

                var bestellingen = await dbContext.Scoutsjaren
                    .Where(sj => sj.Begin == scoutsjaarBegin)
                    .SelectMany(scoutsjaar => scoutsjaar.Bestellingen)
                    .Include(bestelling => bestelling.Straat)
                    .ToListAsync();

                var bestellingenPerZone = bestellingen
                    .GroupBy(b => b.Straat.ZoneId)
                    .ToDictionary(g => g.Key, g => g.ToList());

                var rondeDtos = rondes == null
                    ? new List<RondeDto>()
                    : rondes.Select(ronde =>
                    {
                        var bestellingenForZone = bestellingenPerZone.ContainsKey(ronde.ZoneId) ? bestellingenPerZone[ronde.ZoneId] : new List<Bestelling>();
                        var adressenCount = bestellingenForZone.GroupBy(b => $"{b.StraatId}-{b.Nummer}").Count();
                        var bestellingenCount = bestellingenForZone.Count;
                        var pakkenCount = bestellingenForZone.Sum(b => b.AantalPakken);
                        var geleverdCount = bestellingenForZone.Where(b => b.Geleverd).Count();
                        var nietGeleverdCount = bestellingenForZone.Where(b => !b.Geleverd).Count();

                        return new RondeDto
                        {
                            Id = ronde.Id,
                            ChauffeurId = ronde.BestuurderId,
                            ChauffeurNaam = ronde.Bestuurder.Achternaam + " " + ronde.Bestuurder.Voornaam,
                            ZoneId = ronde.ZoneId,
                            ZoneNaam = ronde.Zone.Naam,
                            Gemeente = ronde.Zone.Gemeente,
                            Straten = ronde.Zone.Straten.Count,
                            Kaartnummer = ronde.Zone.KaartNummer ?? "",
                            ScoutsjaarId = ronde.ScoutsjaarId,
                            Adressen = adressenCount,
                            Bestellingen = bestellingenCount,
                            Geleverd = geleverdCount,
                            NietGeleverd = nietGeleverdCount,
                            Pakken = pakkenCount
                        };
                    })
                    .OrderBy(x => x.ZoneNaam)
                    .ToList();

                return rondeDtos;
            }
        }

        public async Task<RondeDetailsDto> GetRonde(int scoutsjaarBegin, int rondeId)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                Scoutsjaar? sj = dbContext.Scoutsjaren.SingleOrDefault(s => s.Begin == scoutsjaarBegin);
                var ronde = await dbContext.Rondes
                    .Include(x => x.Bestuurder)
                    .Include(x => x.Zone)
                    .SingleOrDefaultAsync(x => x.Id == rondeId && x.ScoutsjaarId == sj.Id);

                if (ronde == null)
                {
                    return new RondeDetailsDto();
                }

                var zone = ronde.Zone;

                var bestellingen = await dbContext.Bestellingen
                    .Include(b => b.Straat)
                    .Where(b => b.Straat.ZoneId == ronde.ZoneId && b.ScoutsjaarId == sj.Id)
                    .ToListAsync();

                var rondeDetailsDto = new RondeDetailsDto
                {
                    Id = ronde.Id,
                    ChauffeurId = ronde.BestuurderId,
                    ChauffeurNaam = ronde.Bestuurder != null ? $"{ronde.Bestuurder.Voornaam} {ronde.Bestuurder.Achternaam}" : "",
                    ZoneId = zone != null ? zone.Id : 0,
                    ZoneNaam = zone != null ? zone.Naam : "",
                    PostNummer = zone?.PostNummer ?? 0,
                    Gemeente = zone != null ? zone.Gemeente : "",
                    ScoutsjaarId = sj.Id,
                    Bestellingen = bestellingen.Select(b => new RondeDetailsBestellingDto
                    {
                        BestellingId = b.Id,
                        Naam = b.Naam,
                        Straat = b.Straat.Naam,
                        Nummer = b.Nummer,
                        Bus = b.Bus,
                        AantalPakken = b.AantalPakken,
                        Geleverd = b.Geleverd
                    }).ToList()
                };

                return rondeDetailsDto;
            }
        }

        public async Task UpdateGeleverd(int bestellingId, bool gelevered)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var bestelling = await dbContext.Bestellingen.SingleOrDefaultAsync(b => b.Id == bestellingId);
                if (bestelling == null)
                {
                    return;
                }

                bestelling.Geleverd = gelevered;
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<ChauffeurRondeDetailsDto> GetChauffeurRondeDetails(int scoutsjaarBegin, int chauffeurId)
        {
            using (var dbContext = _dbContextFactory.CreateDbContext())
            {
                var zoneIds = await dbContext.Scoutsjaren
                    .Where(sj => sj.Begin == scoutsjaarBegin)
                    .SelectMany(scoutsjaar => scoutsjaar.Rondes)
                    .Where(ronde => ronde.BestuurderId == chauffeurId)
                    .Select(x => x.ZoneId)
                    .ToListAsync();

                var bestellingen = await dbContext.Scoutsjaren
                    .Where(sj => sj.Begin == scoutsjaarBegin)
                    .SelectMany(scoutsjaar => scoutsjaar.Bestellingen)
                    .Include(bestelling => bestelling.Straat)
                    .ThenInclude(straat => straat.Zone)
                    .Where(b => zoneIds.Contains(b.Straat.ZoneId))
                    .ToListAsync();

                var bestuurder = dbContext.Bestuurders.Single(b => b.Id == chauffeurId);

                return new ChauffeurRondeDetailsDto
                {
                    ChauffeurNaam = $"{bestuurder.Voornaam} {bestuurder.Achternaam}",
                    Details = bestellingen.Select(b => new ChauffeurRondeDetailDto
                    {
                        BestellingId = b.Id,
                        ZoneNaam = b.Straat?.Zone?.Naam ?? "",
                        Naam = b.Naam,
                        Straat = b.Straat?.Naam ?? "",
                        Nummer = b.Nummer,
                        Bus = b.Bus,
                        PostNummer = b.Straat?.Zone?.PostNummer.ToString() ?? "",
                        Gemeente = b.Straat?.Zone?.Gemeente ?? "",
                        AantalPakken = b.AantalPakken,
                        Position = b.Latitude.HasValue && b.Longitude.HasValue
                            ? new PositionDto
                            {
                                Latitude = b.Latitude.Value,
                                Longitude = b.Longitude.Value
                            }
                            : null
                    }).ToList()
                };
            }
        }

        public async Task<ChauffeurRondeDetailsDto> GetChauffeurRondeDetailsRoute(int scoutsjaarBegin, int chauffeurId)
        {
            var result = await GetChauffeurRondeDetails(scoutsjaarBegin, chauffeurId);

            foreach (var detail in result.Details)
            {
                if (detail.Position != null)
                {
                    continue;
                }

                var response = await _hereGeoCodingService.GetGeocode(detail.Straat, detail.Nummer, detail.PostNummer, detail.Gemeente);

                var firstItem = response.Items.FirstOrDefault();
                if (firstItem != null)
                {
                    detail.Position = new PositionDto
                    {
                        Latitude = firstItem.Position.Lat,
                        Longitude = firstItem.Position.Lng
                    };

                    using (var dbContext = _dbContextFactory.CreateDbContext())
                    {
                        var bestelling = await dbContext.Bestellingen.SingleOrDefaultAsync(b => b.Id == detail.BestellingId);
                        if (bestelling != null)
                        {
                            bestelling.Latitude = firstItem.Position.Lat;
                            bestelling.Longitude = firstItem.Position.Lng;
                            await dbContext.SaveChangesAsync();
                        }
                    }
                }
            }

            var tour = await _hereTourPlanningService.GetRoute(
                result.Details
                .Where(d => d.Position != null)
                .Select(d => d.Position!)
                .ToList());

            return result;
        }
    }
}
