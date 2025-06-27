namespace SintLeoPannenkoeken.Blazor.Models
{
    public class StreefCijfer
    {
        public int Id { get; set; }
        public int TakId { get; set; }
        public Tak Tak { get; set; }
        public int Aantal { get; set; }
    }
}
