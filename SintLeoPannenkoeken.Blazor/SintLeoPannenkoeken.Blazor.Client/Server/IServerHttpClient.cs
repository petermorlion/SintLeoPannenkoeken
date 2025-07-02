using SintLeoPannenkoeken.Blazor.Client.Server.Contracts;

namespace SintLeoPannenkoeken.Blazor.Client.Server
{
    public interface IServerHttpClient
    {
        Task<IList<ScoutsjaarDto>> GetScoutsjaren();
    }
}