namespace SintLeoPannenkoeken.Models
{
    public class ApiClient
    {
        public ApiClient(string naam, string apiKey)
        {
            Naam = naam;
            ApiKey = apiKey;
        }
        public int Id { get; set; }
        public string Naam { get; set; }
        public string ApiKey { get; set; }
    }
}
