namespace SintLeoPannenkoeken.Blazor.Client.Server.Contracts
{
    public class ScoutsjaarDto
    {
        public int? Id { get; set; }
        public int Begin { get; set; }
        public int PannenkoekenPerPak { get; set; }
        public ScoutsjaarStatusDto Status { get; set; }
        public ScoutsjaarDto(int begin, int pannenkoekenPerPak, ScoutsjaarStatusDto status, int? id = null)
        {
            Id = id;
            Begin = begin;
            PannenkoekenPerPak = pannenkoekenPerPak;
            Status = status; 
        }
    }
}
