namespace SintLeoPannenkoeken.Blazor.Client.Auth
{
    public static class Roles
    {
        private const string Admin = "Admin";
        private const string FinanciePloeg = "FinanciePloeg";

        public const string RolesForBeheer = $"{Admin},{FinanciePloeg}";
        public const string RolesForBestellingen = $"{Admin},{FinanciePloeg}";
        public const string RolesForChauffeurs = $"{Admin},{FinanciePloeg}";
        public const string RolesForGebruikers = $"{Admin},{FinanciePloeg}";
        public const string RolesForLeden = $"{Admin}";
        public const string RolesForScoutsjaren = $"{Admin},{FinanciePloeg}";
        public const string RolesForStraten= $"{Admin},{FinanciePloeg}";
        public const string RolesForStreefcijfers = $"{Admin},{FinanciePloeg}";
        
    }
}
