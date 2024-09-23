using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.ApiClients
{
    public class ApiClientViewModel
    {
        private readonly ApiClient _apiClient;

        public ApiClientViewModel(ApiClient apiClient)
        {
            _apiClient = apiClient;
            if (_apiClient == null)
            {
                _apiClient = new ApiClient("", "");
            }
        }

        public int Id => _apiClient.Id;
        public string Naam => _apiClient.Naam;
        public string ApiKey => _apiClient.ApiKey;
    }
}
