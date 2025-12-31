namespace SintLeoPannenkoeken.Blazor.Client.Server.Contracts.Rapporten
{
    public class IngaveTotalenDto
    {
        public IList<IngaveTotalenRowDto> IngaveTotalen { get; set; } = new List<IngaveTotalenRowDto>();
        public IDictionary<string, string> Takken { get; set; } = new Dictionary<string, string>();
    }
}
