namespace SintLeoPannenkoeken.Blazor.Models
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

        public static string GetAfkorting(string takNaam)
        {
            takNaam = takNaam.ToUpper();
            if (takNaam.Contains("ZEEWOLF"))
            {
                return "ZW";
            }
            else if (takNaam.Contains("ZEEPAARD"))
            {
                return "ZP";
            }
            else if (takNaam.Contains("DOLFIJN"))
            {
                return "DO";
            }
            else if (takNaam.Contains("ZEEROB"))
            {
                return "ZR";
            }
            else if (takNaam.Contains("SCHEEPSMAKKER"))
            {
                return "SM";
            }
            else if (takNaam.Contains("ZEEVERKENNER"))
            {
                return "ZV";
            }
            else if (takNaam.Contains("LOODS"))
            {
                return "LO";
            }
            else if (takNaam.Contains("BOOTSLUI"))
            {
                return "BL";
            }
            else if (takNaam.Contains("FINPLG"))
            {
                return "FP";
            }
            else if (takNaam.Contains("AFDELINGSLEIDING"))
            {
                return "AL";
            }
            else
            {
                return "";
            }
        }
    }
}
