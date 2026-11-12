#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
using SintLeoPannenkoeken.Blazor.Client.Server.Contracts;
using SintLeoPannenkoeken.Blazor.External.SintLeoWebsite;
using SintLeoPannenkoeken.Blazor.Models;
using Xunit;

namespace SintLeoPannenkoeken.Blazor.Tests.External.SintLeoWebsite
{
    public class OnlineOrderImporterTests
    {
        [Fact]
        public void MatchOrder_FirstNameLastName_ShouldMatch()
        {
            // Arrange
            var importer = new OnlineOrderImporter(null, null, null);
            
            var onlineBestelling = new PendingOnlineBestellingDto
            {
                Lid = "Sammy Neels",
                Tak = "Zeehondjes"
            };
            
            var tak = new Tak("Zeehondjes");
            var lid = new Lid("Sammy", "Neels") { Tak = tak };
            var leden = new List<Lid> { lid };
            var straten = new List<Straat>();
            
            // Act
            var (straatMatch, lidMatch) = importer.MatchOrder(onlineBestelling, straten, leden);
            
            // Assert
            Assert.Null(straatMatch);
            Assert.NotNull(lidMatch);
            Assert.Equal(lid.Id, lidMatch.Id);
        }

        [Fact]
        public void MatchOrder_LastNameFirstName_ShouldMatch()
        {
            // Arrange
            var importer = new OnlineOrderImporter(null, null, null);
            
            var onlineBestelling = new PendingOnlineBestellingDto
            {
                Lid = "Neels Sammy",
                Tak = "Zeehondjes"
            };
            
            var tak = new Tak("Zeehondjes");
            var lid = new Lid("Sammy", "Neels") { Tak = tak };
            var leden = new List<Lid> { lid };
            var straten = new List<Straat>();
            
            // Act
            var (straatMatch, lidMatch) = importer.MatchOrder(onlineBestelling, straten, leden);
            
            // Assert
            Assert.Null(straatMatch);
            Assert.NotNull(lidMatch);
            Assert.Equal(lid.Id, lidMatch.Id);
        }

        [Fact]
        public void MatchOrder_MultipleMiddleNames_FirstNameLastName_ShouldMatch()
        {
            // Arrange
            var importer = new OnlineOrderImporter(null, null, null);
            
            var onlineBestelling = new PendingOnlineBestellingDto
            {
                Lid = "Jan Michael De Smet",
                Tak = "Welpen"
            };
            
            var tak = new Tak("Welpen");
            var lid = new Lid("Jan Michael", "De Smet") { Tak = tak };
            var leden = new List<Lid> { lid };
            var straten = new List<Straat>();
            
            // Act
            var (straatMatch, lidMatch) = importer.MatchOrder(onlineBestelling, straten, leden);
            
            // Assert
            Assert.Null(straatMatch);
            Assert.NotNull(lidMatch);
            Assert.Equal(lid.Id, lidMatch.Id);
        }

        [Fact]
        public void MatchOrder_MultipleMiddleNames_LastNameFirstName_ShouldMatch()
        {
            // Arrange
            var importer = new OnlineOrderImporter(null, null, null);
            
            var onlineBestelling = new PendingOnlineBestellingDto
            {
                Lid = "De Smet Jan Michael",
                Tak = "Welpen"
            };
            
            var tak = new Tak("Welpen");
            var lid = new Lid("Jan Michael", "De Smet") { Tak = tak };
            var leden = new List<Lid> { lid };
            var straten = new List<Straat>();
            
            // Act
            var (straatMatch, lidMatch) = importer.MatchOrder(onlineBestelling, straten, leden);
            
            // Assert
            Assert.Null(straatMatch);
            Assert.NotNull(lidMatch);
            Assert.Equal(lid.Id, lidMatch.Id);
        }

        [Fact]
        public void MatchOrder_WrongTak_ShouldNotMatch()
        {
            // Arrange
            var importer = new OnlineOrderImporter(null, null, null);
            
            var onlineBestelling = new PendingOnlineBestellingDto
            {
                Lid = "Sammy Neels",
                Tak = "Welpen"
            };
            
            var tak = new Tak("Zeehondjes");
            var lid = new Lid("Sammy", "Neels") { Tak = tak };
            var leden = new List<Lid> { lid };
            var straten = new List<Straat>();
            
            // Act
            var (straatMatch, lidMatch) = importer.MatchOrder(onlineBestelling, straten, leden);
            
            // Assert
            Assert.Null(straatMatch);
            Assert.Null(lidMatch);
        }

        [Fact]
        public void MatchOrder_CaseInsensitive_ShouldMatch()
        {
            // Arrange
            var importer = new OnlineOrderImporter(null, null, null);
            
            var onlineBestelling = new PendingOnlineBestellingDto
            {
                Lid = "SAMMY NEELS",
                Tak = "zeeHONDJES"
            };
            
            var tak = new Tak("Zeehondjes");
            var lid = new Lid("Sammy", "Neels") { Tak = tak };
            var leden = new List<Lid> { lid };
            var straten = new List<Straat>();
            
            // Act
            var (straatMatch, lidMatch) = importer.MatchOrder(onlineBestelling, straten, leden);
            
            // Assert
            Assert.Null(straatMatch);
            Assert.NotNull(lidMatch);
            Assert.Equal(lid.Id, lidMatch.Id);
        }

        [Fact]
        public void MatchOrder_WithExtraSpaces_ShouldMatch()
        {
            // Arrange
            var importer = new OnlineOrderImporter(null, null, null);
            
            var onlineBestelling = new PendingOnlineBestellingDto
            {
                Lid = "  Sammy   Neels  ",
                Tak = "Zeehondjes"
            };
            
            var tak = new Tak("Zeehondjes");
            var lid = new Lid("Sammy", "Neels") { Tak = tak };
            var leden = new List<Lid> { lid };
            var straten = new List<Straat>();
            
            // Act
            var (straatMatch, lidMatch) = importer.MatchOrder(onlineBestelling, straten, leden);
            
            // Assert
            Assert.Null(straatMatch);
            Assert.NotNull(lidMatch);
            Assert.Equal(lid.Id, lidMatch.Id);
        }

        [Fact]
        public void MatchOrder_EmptyLidName_ShouldNotMatch()
        {
            // Arrange
            var importer = new OnlineOrderImporter(null, null, null);
            
            var onlineBestelling = new PendingOnlineBestellingDto
            {
                Lid = "",
                Tak = "Zeehondjes"
            };
            
            var tak = new Tak("Zeehondjes");
            var lid = new Lid("Sammy", "Neels") { Tak = tak };
            var leden = new List<Lid> { lid };
            var straten = new List<Straat>();
            
            // Act
            var (straatMatch, lidMatch) = importer.MatchOrder(onlineBestelling, straten, leden);
            
            // Assert
            Assert.Null(straatMatch);
            Assert.Null(lidMatch);
        }

        [Fact]
        public void MatchOrder_SingleName_ShouldNotMatch()
        {
            // Arrange
            var importer = new OnlineOrderImporter(null, null, null);
            
            var onlineBestelling = new PendingOnlineBestellingDto
            {
                Lid = "Sammy",
                Tak = "Zeehondjes"
            };
            
            var tak = new Tak("Zeehondjes");
            var lid = new Lid("Sammy", "Neels") { Tak = tak };
            var leden = new List<Lid> { lid };
            var straten = new List<Straat>();
            
            // Act
            var (straatMatch, lidMatch) = importer.MatchOrder(onlineBestelling, straten, leden);
            
            // Assert
            Assert.Null(straatMatch);
            Assert.Null(lidMatch);
        }

        [Fact]
        public void MatchOrder_StreetAndLid_ShouldMatch()
        {
            // Arrange
            var importer = new OnlineOrderImporter(null, null, null);
            
            var onlineBestelling = new PendingOnlineBestellingDto
            {
                Straat = "Kerkstraat",
                Postcode = "8310",
                Lid = "Sammy Neels",
                Tak = "Zeehondjes"
            };
            
            var straat = new Straat("Kerkstraat", "Assebroek") { PostNummer = 8310 };
            var tak = new Tak("Zeehondjes");
            var lid = new Lid("Sammy", "Neels") { Tak = tak };
            var straten = new List<Straat> { straat };
            var leden = new List<Lid> { lid };
            
            // Act
            var (straatMatch, lidMatch) = importer.MatchOrder(onlineBestelling, straten, leden);
            
            // Assert
            Assert.NotNull(straatMatch);
            Assert.Equal(straat.Id, straatMatch.Id);
            Assert.NotNull(lidMatch);
            Assert.Equal(lid.Id, lidMatch.Id);
        }

        [Fact]
        public void MatchOrder_PrefersFirstNameLastName_WhenBothExist()
        {
            // Arrange
            var importer = new OnlineOrderImporter(null, null, null);
            
            // Two members with reversed names
            var tak = new Tak("Zeehondjes");
            var lid1 = new Lid("Sammy", "Neels") { Tak = tak };
            var lid2 = new Lid("Neels", "Sammy") { Tak = tak };
            
            var onlineBestelling = new PendingOnlineBestellingDto
            {
                Lid = "Sammy Neels",
                Tak = "Zeehondjes"
            };
            
            var leden = new List<Lid> { lid1, lid2 };
            var straten = new List<Straat>();
            
            // Act
            var (straatMatch, lidMatch) = importer.MatchOrder(onlineBestelling, straten, leden);
            
            // Assert - should match the first one (Sammy Neels)
            Assert.Null(straatMatch);
            Assert.NotNull(lidMatch);
            Assert.Equal(lid1.Id, lidMatch.Id);
        }

        [Fact]
        public void MatchTak_ExactMatch_ShouldReturnTak()
        {
            // Arrange
            var importer = new OnlineOrderImporter(null, null, null);
            var tak = new Tak("Zeehondjes");
            var takken = new List<Tak> { tak };
            
            // Act
            var result = importer.MatchTak("Zeehondjes", takken);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(tak.Id, result.Id);
        }

        [Fact]
        public void MatchTak_CaseInsensitive_ShouldMatch()
        {
            // Arrange
            var importer = new OnlineOrderImporter(null, null, null);
            var tak = new Tak("Zeehondjes");
            var takken = new List<Tak> { tak };
            
            // Act
            var result = importer.MatchTak("zeeHONDJES", takken);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(tak.Id, result.Id);
        }

        [Fact]
        public void MatchTak_WithSpaces_ShouldMatch()
        {
            // Arrange
            var importer = new OnlineOrderImporter(null, null, null);
            var tak = new Tak("Zeehondjes");
            var takken = new List<Tak> { tak };
            
            // Act
            var result = importer.MatchTak("  Zeehondjes  ", takken);
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal(tak.Id, result.Id);
        }

        [Fact]
        public void MatchTak_NotFound_ShouldReturnNull()
        {
            // Arrange
            var importer = new OnlineOrderImporter(null, null, null);
            var tak = new Tak("Zeehondjes");
            var takken = new List<Tak> { tak };
            
            // Act
            var result = importer.MatchTak("Welpen", takken);
            
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void MatchTak_EmptyInput_ShouldReturnNull()
        {
            // Arrange
            var importer = new OnlineOrderImporter(null, null, null);
            var tak = new Tak("Zeehondjes");
            var takken = new List<Tak> { tak };
            
            // Act
            var result = importer.MatchTak("", takken);
            
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void MatchOrder_WithDifferentCasesAndSpaces_BothFormats_ShouldMatch()
        {
            // Arrange
            var importer = new OnlineOrderImporter(null, null, null);
            
            var tak = new Tak("  ZEEHONDJES  ");
            var lid = new Lid("  Sammy  ", "  Neels  ") { Tak = tak };
            var leden = new List<Lid> { lid };
            var straten = new List<Straat>();
            
            // Test firstname lastname format
            var onlineBestelling1 = new PendingOnlineBestellingDto
            {
                Lid = "  SAMMY   NEELS  ",
                Tak = "  zeehondJES  "
            };
            
            // Test lastname firstname format
            var onlineBestelling2 = new PendingOnlineBestellingDto
            {
                Lid = "  NEELS   SAMMY  ",
                Tak = "  zeehondJES  "
            };
            
            // Act & Assert for firstname lastname
            var (straatMatch1, lidMatch1) = importer.MatchOrder(onlineBestelling1, straten, leden);
            Assert.Null(straatMatch1);
            Assert.NotNull(lidMatch1);
            Assert.Equal(lid.Id, lidMatch1.Id);
            
            // Act & Assert for lastname firstname
            var (straatMatch2, lidMatch2) = importer.MatchOrder(onlineBestelling2, straten, leden);
            Assert.Null(straatMatch2);
            Assert.NotNull(lidMatch2);
            Assert.Equal(lid.Id, lidMatch2.Id);
        }
    }
}
