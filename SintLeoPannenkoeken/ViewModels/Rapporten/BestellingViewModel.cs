namespace SintLeoPannenkoeken.ViewModels.Rapporten
{
    public class BestellingViewModel
    {
        public int BestellingId { get; set; }
        public string Naam { get; set; }
        public string Straat { get; set; }
        public string Nummer { get; set; }
        public string Bus { get; set; }
        public int Aantal { get; set; }
        public bool Geleverd { get; set; }
    }
}
