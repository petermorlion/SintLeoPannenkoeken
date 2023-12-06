namespace SintLeoPannenkoeken.ViewModels.Rapporten
{
    public class IngaveTotalenViewModel
    {
        public IList<string> Takken { get; set; }
        public IList<IngaveTotalenRowViewModel> IngaveTotalen { get; set; }
        public int ScoutsjaarBegin { get; set; }
    }
}
