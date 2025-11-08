using SintLeoPannenkoeken.Blazor.Client.Server.Contracts;
using SintLeoPannenkoeken.Blazor.Client.Server.Contracts.Rapporten;

namespace SintLeoPannenkoeken.Blazor.Client.Server
{
    /// <summary>
    /// Interface to access server-side data.
    /// </summary>
    public interface IServerData
    {
        Task<IList<ScoutsjaarDto>> GetScoutsjaren();
        Task UpdateScoutsjaar(ScoutsjaarDto scoutsjaar);

        Task<IList<LidDto>> GetLeden();
        Task<LidDto> CreateLid(NewLidDto lid);
        Task UpdateLid(LidDto lid);

        Task<IList<GebruikerDto>> GetGebruikers();
        Task<GebruikerCreatedDto> CreateGebruiker(NewGebruikerDto gebruiker);
        Task UpdateGebruiker(GebruikerDto gebruiker);
        Task DeleteGebruiker(string email);


        Task<StraatDto> CreateStraat(StraatDto straatDto);
        Task<IList<StraatDto>> GetStraten();


        Task<IList<TakDto>> GetTakken();

        Task<IList<BestellingDto>> GetBestellingen(int scoutsjaar);
        Task<BestellingDto> CreateBestelling(NewBestellingDto bestelling);
        Task UpdateBestelling(UpdateBestellingDto bestelling);
        Task DeleteBestelling(int bestellingId);
        Task UpdateGeleverd(int bestellingId, bool gelevered);



        Task<IList<StreefcijferDto>> GetStreefcijfers(int jaar);
        Task<StreefcijferDto> CreateStreefcijfer(StreefcijferDto streefcijfer);
        Task UpdateStreefcijfer(StreefcijferDto streefcijfer);
        Task DeleteStreefcijfer(int streefcijferId);

        Task<IList<ChauffeurDto>> GetChauffeurs(int scoutsjaar);
        Task<ChauffeurDto> CreateChauffeur(ChauffeurDto chauffeur);
        Task UpdateChauffeur(ChauffeurDto chauffeur);
        Task DeleteChauffeur(int chauffeurId);
        Task<ChauffeurDto> GetChauffeur(int scoutsjaar, int chauffeurId);

        Task<IList<ZoneDto>> GetZones();


        Task<IList<RondeDto>> GetRondesForChauffeur(int chauffeurId, int scoutsjaarBegin);
        Task<RondeDto> CreateRonde(int chauffeurId, int scoutsjaarBegin, CreateRondeDto rondeDto);
        Task DeleteRonde(int rondeId);

        Task<IList<RondeDto>> GetRondes(int scoutsjaarBegin);
        Task<RondeDetailsDto> GetRonde(int scoutsjaarBegin, int rondeId);

       
        Task<ChauffeurRondeDetailsDto> GetChauffeurRondeDetails(int scoutsjaarBegin, int chauffeurId);
        Task<ChauffeurRondeDetailsDto> GetChauffeurRondeDetailsRoute(int scoutsjaarBegin, int chauffeurId);


        Task<VerkoopPerTakDto> GetVerkoopPerTakRapport(int scoutsjaarBegin);
    }
}