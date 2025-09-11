using SintLeoPannenkoeken.Blazor.Client.Server.Contracts;

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


        Task<IList<StraatDto>> GetStraten();
        Task<IList<TakDto>> GetTakken();

        Task<IList<BestellingDto>> GetBestellingen(int scoutsjaar);
        Task<BestellingDto> CreateBestelling(NewBestellingDto bestelling);
        Task UpdateBestelling(UpdateBestellingDto bestelling);

        

        Task<IList<StreefcijferDto>> GetStreefcijfers(int jaar);
        Task<StreefcijferDto> CreateStreefcijfer(StreefcijferDto streefcijfer);
        Task UpdateStreefcijfer(StreefcijferDto streefcijfer);
        Task DeleteStreefcijfer(int streefcijferId);

        Task<IList<ChauffeurDto>> GetChauffeurs();
        Task<ChauffeurDto> CreateChauffeur(ChauffeurDto chauffeur);
        Task UpdateChauffeur(ChauffeurDto chauffeur);
        Task DeleteChauffeur(int chauffeurId);
    }
}