namespace SintLeoPannenkoeken.Blazor.Client.Server.Contracts.Rapporten
{
    public class VerkoopPerLidDto
    {
        public int ScoutsjaarBegin { get; set; }
        public List<LidVerkoopDto> LidVerkopen { get; set; } = new List<LidVerkoopDto>();
    }
}
