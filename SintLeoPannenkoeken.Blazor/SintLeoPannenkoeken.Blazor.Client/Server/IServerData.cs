using SintLeoPannenkoeken.Blazor.Client.Server.Contracts;

namespace SintLeoPannenkoeken.Blazor.Client.Server
{
    /// <summary>
    /// Interface to access server-side data.
    /// </summary>
    public interface IServerData
    {
        Task<IList<ScoutsjaarDto>> GetScoutsjaren();
        Task<IList<LidDto>> GetLeden();
        Task<IList<GebruikerDto>> GetGebruikers();
        Task<IList<StraatDto>> GetStraten();
        Task<IList<ChauffeurDto>> GetChauffeurs();

        Task UpdateScoutsjaar(ScoutsjaarDto scoutsjaar);
    }
}