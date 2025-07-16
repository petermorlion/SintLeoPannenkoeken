namespace SintLeoPannenkoeken.Blazor.Client.Server.Contracts
{
    public class BestellingenOverviewDto
    {

        public BestellingenOverviewDto(IList<BestellingDto> bestellingen, int pannenkoekenPerPak)
        {
            Bestellingen = bestellingen;
            PannenkoekenPerPak = pannenkoekenPerPak;
        }

        public IList<BestellingDto> Bestellingen { get; init; }
        public int PannenkoekenPerPak { get; init; }
        public int AantalPakjes => Bestellingen.Sum(bestelling => bestelling.AantalPakken);
        public int AantalPannenkoeken => Bestellingen.Sum(bestelling => bestelling.AantalPakken) * PannenkoekenPerPak;
    }
}
