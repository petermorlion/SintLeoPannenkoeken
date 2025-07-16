using SintLeoPannenkoeken.Blazor.Client.Server.Contracts;
using SintLeoPannenkoeken.Blazor.Models;

namespace SintLeoPannenkoeken.Blazor.Tests.Models
{
    public class ScoutsjaarStatusMappingTests
    {
        [Fact]
        public void Should_Have_Identical_Values_In_Dto()
        {
            var statusNames = Enum.GetNames(typeof(ScoutsjaarStatus));
            var dtoNames = Enum.GetNames(typeof(ScoutsjaarStatusDto));
            Assert.Equal(statusNames, dtoNames);

            var statusValues = Enum.GetValues(typeof(ScoutsjaarStatus)).Cast<ScoutsjaarStatus>().Select(x => (int)x);
            var dtoValues = Enum.GetValues(typeof(ScoutsjaarStatusDto)).Cast<ScoutsjaarStatusDto>().Select(x => (int)x);
            Assert.Equal(statusValues, dtoValues);
        }
    }
}
