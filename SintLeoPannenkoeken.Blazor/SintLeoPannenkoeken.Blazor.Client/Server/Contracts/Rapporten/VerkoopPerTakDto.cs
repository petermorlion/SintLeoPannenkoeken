namespace SintLeoPannenkoeken.Blazor.Client.Server.Contracts.Rapporten
{
    public class VerkoopPerTakDto
    {
        public int ScoutsjaarBegin { get; set; }
        public List<TakVerkoopDto> TakVerkopen { get; set; } = new List<TakVerkoopDto>();
    }
}
