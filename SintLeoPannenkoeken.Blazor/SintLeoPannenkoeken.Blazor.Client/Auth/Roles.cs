namespace SintLeoPannenkoeken.Blazor.Client.Auth
{
    public static class Roles
    {
        private const string Admin = "admin";
        private const string FinanciePloeg = "financieploeg";

        public const string RolesForBeheer = $"{Admin},{FinanciePloeg}";
        public const string RolesForBestellingen = $"{Admin},{FinanciePloeg}";
        public const string RolesForChauffeurs = $"{Admin},{FinanciePloeg}";
        public const string RolesForGebruikers = $"{Admin},{FinanciePloeg}";
        public const string RolesForLeden = $"{Admin}";
        public const string RolesForScoutsjaren = $"{Admin},{FinanciePloeg}";
        public const string RolesForStraten= $"{Admin},{FinanciePloeg}";
        
    }
}
