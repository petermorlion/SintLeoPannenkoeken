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

        /// <summary>
        /// The API Key of the client, hashed and salted.
        /// </summary>
        public string ApiKey { get; set; }
    }
}
