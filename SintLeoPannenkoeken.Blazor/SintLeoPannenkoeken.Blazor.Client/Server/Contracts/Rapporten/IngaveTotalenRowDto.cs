namespace SintLeoPannenkoeken.Blazor.Client.Server.Contracts.Rapporten
{
    public class IngaveTotalenRowDto
    {
        public DateTime IngaveDatum { get; set; }
        public IDictionary<string, int> AantalPerTak { get; set; } = new Dictionary<string, int>();
        public int Totaal => AantalPerTak.Sum(x => x.Value);
        public int OplopendTotaal { get; set; }
    }
}
