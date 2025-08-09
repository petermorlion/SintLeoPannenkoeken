using Microsoft.AspNetCore.Components;
using SintLeoPannenkoeken.Blazor.Client.Server;
using SintLeoPannenkoeken.Blazor.Client.Server.Contracts;

namespace SintLeoPannenkoeken.Blazor.Client
{
    public class CurrentScoutsjaarQuerystringParameterProvider
    {
        private readonly NavigationManager _navigationManager;
        private readonly IServerData _serverData;
        private readonly int? _currentScoutsjaarBegin = null;

        public CurrentScoutsjaarQuerystringParameterProvider(NavigationManager navigationManager, IServerData serverData)
        {
            _navigationManager = navigationManager;
            _serverData = serverData;
        }

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

            return scoutsjaar;
        }

        public async Task<int?> GetCurrentScoutsjaarBegin()
        {
            if (_currentScoutsjaarBegin == null)
            {
                var scoutsjaar = await GetCurrentScoutsjaar();
                if (scoutsjaar != null)
                {
                    return scoutsjaar.Begin;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return _currentScoutsjaarBegin;
            }
        }
    }
}
