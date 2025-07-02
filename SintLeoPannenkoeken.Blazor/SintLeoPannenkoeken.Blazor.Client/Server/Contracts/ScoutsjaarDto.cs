namespace SintLeoPannenkoeken.Blazor.Client.Server.Contracts
{
    public class ScoutsjaarDto
    {
        public int Begin { get; init; }
        public int PannenkoekenPerPak { get; init; }
        public ScoutsjaarDto(int begin, int pannenkoekenPerPak)
        {
            Begin = begin;
            PannenkoekenPerPak = pannenkoekenPerPak;
        }
    }
}
