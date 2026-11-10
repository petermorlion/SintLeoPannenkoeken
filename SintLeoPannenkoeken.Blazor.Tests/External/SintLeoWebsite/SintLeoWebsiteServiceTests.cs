using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using SintLeoPannenkoeken.Blazor.Client.Server.Contracts;
using SintLeoPannenkoeken.Blazor.External.SintLeoWebsite;
using Xunit;

namespace SintLeoPannenkoeken.Blazor.Tests.External.SintLeoWebsite
{
    public class SintLeoWebsiteServiceTests
    {
        [Fact]
        public void ParseGravityFormsResponse_HandlesEmptyResponse()
        {
            // Use reflection to call private method
            var service = CreateService();
            var method = typeof(SintLeoWebsiteService).GetMethod("ParseGravityFormsResponse", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            var json = "{}";
            var result = (SintLeoWebsiteService.GravityFormsResponse)method.Invoke(service, new object[] { json })!;
            
            Assert.Equal(0, result.TotalCount);
            Assert.Empty(result.Entries);
        }

        [Fact]
        public void ParseGravityFormsResponse_HandlesResponseWithEntries()
        {
            var service = CreateService();
            var method = typeof(SintLeoWebsiteService).GetMethod("ParseGravityFormsResponse", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            var json = "{\"total_count\":1,\"entries\":[{\"id\":\"1\",\"date_created\":\"2026-01-01 10:00:00\",\"status\":\"active\",\"7.3\":\"Test\",\"7.6\":\"User\"}]}";
            var result = (SintLeoWebsiteService.GravityFormsResponse)method.Invoke(service, new object[] { json })!;
            
            Assert.Equal(1, result.TotalCount);
            Assert.Single(result.Entries);
            Assert.Equal("1", result.Entries[0].Id);
            Assert.Equal("2026-01-01 10:00:00", result.Entries[0].DateCreated);
            Assert.Equal("active", result.Entries[0].Status);
            Assert.Equal("Test", result.Entries[0].Fields["7.3"]);
            Assert.Equal("User", result.Entries[0].Fields["7.6"]);
        }

        [Fact]
        public void GetFieldValue_ReturnsFirstAvailableField()
        {
            var service = CreateService();
            var method = typeof(SintLeoWebsiteService).GetMethod("GetFieldValue", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            var entry = new SintLeoWebsiteService.GravityFormsEntry
            {
                Fields = new Dictionary<string, string>
                {
                    { "32.1", "" },
                    { "32.3", "Sammy" },
                    { "32", "Test" }
                }
            };
            
            Assert.Equal("Sammy", (string)method.Invoke(service, new object[] { entry, new string[] { "32.3", "32.1", "32" } })!);
            Assert.Equal("Test", (string)method.Invoke(service, new object[] { entry, new string[] { "32" } })!);
            Assert.Equal("", (string)method.Invoke(service, new object[] { entry, new string[] { "32.2", "32.4" } })!);
        }

        [Fact]
        public void UrlEncode_EncodesSpecialCharactersCorrectly()
        {
            var service = CreateService();
            var method = typeof(SintLeoWebsiteService).GetMethod("UrlEncode", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            Assert.NotNull(method);
            
            Assert.Equal("test%20value", (string)method.Invoke(service, new object[] { "test value" })!);
            Assert.Equal("test%2Avalue", (string)method.Invoke(service, new object[] { "test*value" })!);
            Assert.Equal("test%27value", (string)method.Invoke(service, new object[] { "test'value" })!);
            Assert.Equal("test%28value%29", (string)method.Invoke(service, new object[] { "test(value)" })!);
        }

        [Fact]
        public void GenerateNonce_ReturnsNonEmptyString()
        {
            var service = CreateService();
            var method = typeof(SintLeoWebsiteService).GetMethod("GenerateNonce", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            var nonce1 = (string)method.Invoke(service, null)!;
            var nonce2 = (string)method.Invoke(service, null)!;
            
            Assert.NotEmpty(nonce1);
            Assert.NotEmpty(nonce2);
            Assert.Equal(32, nonce1.Length);
            Assert.NotEqual(nonce1, nonce2);
        }

        private static SintLeoWebsiteService CreateService()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "SintLeoWebsite:BaseUrl", "https://www.events-sintleo.be/wp-json/gf/v2" },
                    { "SintLeoWebsite:FormId", "67" },
                    { "SintLeoWebsite:OAuthConsumerKey", "test_key" },
                    { "SintLeoWebsite:OAuthConsumerSecret", "test_secret" },
                    { "SintLeoWebsite:BatchSize", "40" },
                    { "SintLeoWebsite:StartDateMonthsBeforeJanuary", "3" }
                })
                .Build();

            // Create a minimal HttpClient - we won't actually make HTTP calls in these tests
            var httpClient = new HttpClient();
            return new SintLeoWebsiteService(httpClient, configuration);
        }
    }
}
