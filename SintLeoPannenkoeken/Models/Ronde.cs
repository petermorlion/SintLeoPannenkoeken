namespace SintLeoPannenkoeken.Models
{
    /// <summary>
    /// De link tussen een bestuurder en een zone. Dit duidt aan dat deze bestuurder deze zone moet doen, in een bepaald scoutsjaar.
    /// </summary>
    public class Ronde
    {
        public int Id { get; set; }
        public int ZoneId { get; set; }
        public Zone Zone { get; set; }
        public int BestuurderId { get; set; }
        public Bestuurder Bestuurder { get; set; }
        public int ScoutsjaarId { get; set; }
    }
}
