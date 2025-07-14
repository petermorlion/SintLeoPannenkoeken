using System.ComponentModel;

namespace SintLeoPannenkoeken.Blazor.Client.Server.Contracts
{
    public enum ScoutsjaarStatusDto
    {
        [Description("Actief")]
        Actief = 0,

        [Description("Gearchiveerd")]
        Gearchiveerd = 1
    }
}
