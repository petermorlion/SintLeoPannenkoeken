namespace SintLeoPannenkoeken.Blazor.Options
{
    public class RouteXLOptions
    {
        public const string SectionName = "RouteXL";

        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public string ApiKey { get; set; } = "";
        public int FreeTierStopLimit { get; set; } = 10;

        public bool HasApiKey => !string.IsNullOrWhiteSpace(ApiKey);
        public bool HasCredentials =>
            !string.IsNullOrWhiteSpace(UserName) &&
            !string.IsNullOrWhiteSpace(Password);
    }
}
