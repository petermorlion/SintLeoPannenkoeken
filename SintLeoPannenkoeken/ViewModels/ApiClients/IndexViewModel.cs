using SintLeoPannenkoeken.Models;

namespace SintLeoPannenkoeken.ViewModels.ApiClients
{
    public class IndexViewModel
    {
        public IndexViewModel(ICollection<ApiClient> apiClients)
        {
            ApiClients = apiClients.Select(apiClient => new ApiClientViewModel(apiClient)).OrderBy(apiClient => apiClient.Naam).ToList();
        }

        public IList<ApiClientViewModel> ApiClients { get; set; }
    }
}
