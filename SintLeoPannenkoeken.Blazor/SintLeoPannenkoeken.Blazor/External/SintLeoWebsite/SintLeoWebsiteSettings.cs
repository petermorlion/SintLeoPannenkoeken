namespace SintLeoPannenkoeken.Blazor.External.SintLeoWebsite
{
    public class SintLeoWebsiteSettings
    {
        public string BaseUrl { get; set; } = "https://www.events-sintleo.be/wp-json/gf/v2";
        public string FormId { get; set; } = "67";
        public string OAuthConsumerKey { get; set; } = "";
        public string OAuthConsumerSecret { get; set; } = "";
        public string OAuthToken { get; set; } = "";
        public string OAuthTokenSecret { get; set; } = "";
        public int BatchSize { get; set; } = 40;
        public int StartDateMonthsBeforeJanuary { get; set; } = 3;
    }
}
