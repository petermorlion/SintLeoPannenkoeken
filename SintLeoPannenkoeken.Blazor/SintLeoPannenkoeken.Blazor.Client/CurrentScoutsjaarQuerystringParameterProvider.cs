using Microsoft.AspNetCore.Components;
using SintLeoPannenkoeken.Blazor.Client.Server;
using SintLeoPannenkoeken.Blazor.Client.Server.Contracts;

namespace SintLeoPannenkoeken.Blazor.Client
{
    public class CurrentScoutsjaarQuerystringParameterProvider
    {
        private readonly NavigationManager _navigationManager;
        private readonly IServerData _serverData;

        public CurrentScoutsjaarQuerystringParameterProvider(NavigationManager navigationManager, IServerData serverData)
        {
            _navigationManager = navigationManager;
            _serverData = serverData;
        }

        /// <summary>
        /// Gets the current scoutsjaar. Either from the query string or the most recent if no value is found.
        /// </summary>
        /// <returns></returns>
        public async Task<ScoutsjaarDto?> GetCurrentScoutsjaar()
        {
            var uriBuilder = new UriBuilder(_navigationManager.Uri);
            var query = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
            var scoutsjaarParameter = query["scoutsjaar"];

            var _scoutsjaren = await _serverData.GetScoutsjaren();
            ScoutsjaarDto? scoutsjaar = null;
            if (int.TryParse(scoutsjaarParameter, out int scoutsjaarBegin))
            {
                scoutsjaar = _scoutsjaren.FirstOrDefault(sj => sj.Begin == scoutsjaarBegin);
            }

            if (scoutsjaar == null)
            {
                scoutsjaar = _scoutsjaren.OrderByDescending(sj => sj.Begin).FirstOrDefault();
            }

            return scoutsjaar;
        }
    }
}
