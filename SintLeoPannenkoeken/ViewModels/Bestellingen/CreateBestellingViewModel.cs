namespace SintLeoPannenkoeken.ViewModels.Bestellingen
{
    public class CreateBestellingViewModel
    {
        public string Naam { get; set; }
        public string? Telefoon { get; set; }
        public int AantalPakken { get; set; }
        public string? Opmerkingen { get; set; }
        public bool Betaald { get; set; }
        public int LidId { get; set; }
        public int StraatId { get; set; }
        public string Nummer { get; set; }
        public string Bus { get; set; }
    }
}
