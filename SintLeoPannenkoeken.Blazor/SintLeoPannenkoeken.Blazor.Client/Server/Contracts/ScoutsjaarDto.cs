namespace SintLeoPannenkoeken.Blazor.Client.Server.Contracts
{
    public class ScoutsjaarDto
    {
        public int Begin { get; set; }
        public int PannenkoekenPerPak { get; set; }
        public ScoutsjaarStatusDto Status { get; set; }
        public ScoutsjaarDto(int begin, int pannenkoekenPerPak, ScoutsjaarStatusDto status)
        {
            Begin = begin;
            PannenkoekenPerPak = pannenkoekenPerPak;
            Status = status; 
        }
    }
}
