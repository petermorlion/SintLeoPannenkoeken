namespace SintLeoPannenkoeken.Blazor.Client.Server.Contracts
{
    public class RondeDetailsDto
    {
        public int Id { get; init; }
        public int ChauffeurId { get; init; }
        public string ChauffeurNaam { get; init; } = "";
        public int ZoneId { get; init; }
        public string ZoneNaam { get; init; } = "";
        public int PostNummer { get; init; } = 0;
        public string Gemeente { get; init; } = "";
        public int ScoutsjaarId { get; init; }
        public IList<RondeDetailsBestellingDto> Bestellingen { get; init; } = [];

        public int AantalGeleverd => Bestellingen.Where(x => x.Geleverd).Sum(x => x.AantalPakken);
        public int AantalNietGeleverd => Bestellingen.Where(x => !x.Geleverd).Sum(x => x.AantalPakken);
        public int TotaalAantal => Bestellingen.Sum(x => x.AantalPakken);
    }
}
