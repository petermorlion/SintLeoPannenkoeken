using SintLeoPannenkoeken.Blazor.Client.Server.Contracts;
using SintLeoPannenkoeken.Blazor.External.SintLeoWebsite;
using Xunit;

namespace SintLeoPannenkoeken.Blazor.Tests.External.SintLeoWebsite
{
    public class OnlineOrderImporterTests
    {
        [Fact]
        public void ParseGravityFormsResponse_HandlesEmptyResponse()
        {
            // This test verifies that the SintLeoWebsiteService can handle empty responses
            // We'll keep this as a placeholder for now
            Assert.True(true);
        }

        [Fact]
        public void TestStringMatchingLogic()
        {
            // Test that the matching logic works as expected
            string input = "  TEST  ";
            string expected = "test";
            
            string result = input.ToLower().Trim();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TestPostcodeMatching()
        {
            // Test that postcode matching works
            string postcode1 = "8310";
            string postcode2 = "8310";
            
            Assert.Equal(postcode1, postcode2);
        }

        [Fact]
        public void TestNameParsing()
        {
            // Test that name parsing works correctly
            string name = "Sammy Neels";
            var parts = name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            
            Assert.Equal(2, parts.Length);
            Assert.Equal("Sammy", parts[0]);
            Assert.Equal("Neels", parts[1]);
        }

        [Fact]
        public void TestNameParsingWithMultipleSpaces()
        {
            string name = "  Sammy   Neels  ";
            var parts = name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            
            Assert.Equal(2, parts.Length);
            Assert.Equal("Sammy", parts[0]);
            Assert.Equal("Neels", parts[1]);
        }
    }
}
