namespace SintLeoPannenkoeken.Blazor.Client.Server.Contracts
{
    public class ChauffeurRondeDetailsDto
    {
        public string ChauffeurNaam { get; init; } = "";
        public IList<ChauffeurRondeDetailDto> Details { get; set; } = new List<ChauffeurRondeDetailDto>();
    }
}
