namespace SintLeoPannenkoeken.Models
{
    public class Tak
    {
        public Tak(string naam)
        {
            Naam = naam;
            Leden = new List<Lid>();
        }
        public int Id { get; set; }
        public string Naam { get; set; }
        public string Afkorting { get; set; }
        public List<Lid> Leden { get; set; }
    }
}
