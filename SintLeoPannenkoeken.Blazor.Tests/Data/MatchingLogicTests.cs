using SintLeoPannenkoeken.Blazor.Client.Server.Contracts;
using SintLeoPannenkoeken.Blazor.Models;
using Xunit;

namespace SintLeoPannenkoeken.Blazor.Tests.Data
{
    public class MatchingLogicTests
    {
        [Fact]
        public void StreetMatching_LowercaseTrimmedWithPostcode()
        {
            // Arrange - create test data
            var street1 = new Straat("Jacob van Maerlantstraat", "Brugge")
            {
                Id = 1,
                PostNummer = 8000,
                Naam = "Jacob van Maerlantstraat"
            };
            
            var street2 = new Straat("Jacob van Maerlantstraat", "Gent")
            {
                Id = 2,
                PostNummer = 9000,
                Naam = "Jacob van Maerlantstraat"
            };
            
            var straten = new List<Straat> { street1, street2 };
            
            // Create test DTO with lowercase and trimmed street
            var dto = new PendingOnlineBestellingDto
            {
                Id = 1,
                Straat = "  jacob van maerlantstraat  ",
                Postcode = "8000"
            };
            
            // Act - find match
            var match = straten.FirstOrDefault(s => 
                s.Naam.ToLower().Trim() == dto.Straat.ToLower().Trim() && 
                s.PostNummer.ToString() == dto.Postcode);
            
            // Assert
            Assert.NotNull(match);
            Assert.Equal(1, match.Id);
            Assert.Equal(8000, match.PostNummer);
        }

        [Fact]
        public void StreetMatching_DifferentPostcodes_DifferentStreets()
        {
            // Arrange
            var street1 = new Straat("Main Street", "City A")
            {
                Id = 1,
                PostNummer = 1000,
                Naam = "Main Street"
            };
            
            var street2 = new Straat("Main Street", "City B")
            {
                Id = 2,
                PostNummer = 2000,
                Naam = "Main Street"
            };
            
            var straten = new List<Straat> { street1, street2 };
            
            var dto = new PendingOnlineBestellingDto
            {
                Id = 1,
                Straat = "Main Street",
                Postcode = "2000"
            };
            
            // Act
            var match = straten.FirstOrDefault(s => 
                s.Naam.ToLower().Trim() == dto.Straat.ToLower().Trim() && 
                s.PostNummer.ToString() == dto.Postcode);
            
            // Assert
            Assert.NotNull(match);
            Assert.Equal(2, match.Id);
            Assert.Equal(2000, match.PostNummer);
        }

        [Fact]
        public void MemberMatching_LowercaseTrimmedWithTak()
        {
            // Arrange
            var tak1 = new Tak("Scheepsmakkers") { Id = 1 };
            var tak2 = new Tak("Financieploeg") { Id = 2 };
            
            var lid1 = new Lid("Peter", "Morlion") { Id = 1, Tak = tak1 };
            var lid2 = new Lid("Peter", "Morlion") { Id = 2, Tak = tak2 };
            
            var leden = new List<Lid> { lid1, lid2 };
            
            var dto = new PendingOnlineBestellingDto
            {
                Id = 1,
                Naam = "  Peter   Morlion  ",
                Tak = "Scheepsmakkers"
            };
            
            // Act - simulate the matching logic from ServerDirectClient
            var naamParts = dto.Naam?.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
            var voornaam = naamParts.Length > 0 ? naamParts[0].ToLower().Trim() : "";
            var achternaam = naamParts.Length > 1 ? string.Join(" ", naamParts.Skip(1)).ToLower().Trim() : "";
            var takNaam = dto.Tak?.Trim() ?? "";
            
            var match = leden.FirstOrDefault(l => 
                l.Voornaam.ToLower().Trim() == voornaam && 
                l.Achternaam.ToLower().Trim() == achternaam &&
                l.Tak.Naam.Trim() == takNaam);
            
            // Assert
            Assert.NotNull(match);
            Assert.Equal(1, match.Id);
            Assert.Equal("Scheepsmakkers", match.Tak.Naam);
        }

        [Fact]
        public void MemberMatching_DifferentTak_DifferentMembers()
        {
            // Arrange
            var tak1 = new Tak("Scheepsmakkers") { Id = 1 };
            var tak2 = new Tak("Financieploeg") { Id = 2 };
            
            var lid1 = new Lid("Peter", "Morlion") { Id = 1, Tak = tak1 };
            var lid2 = new Lid("Peter", "Morlion") { Id = 2, Tak = tak2 };
            
            var leden = new List<Lid> { lid1, lid2 };
            
            var dto = new PendingOnlineBestellingDto
            {
                Id = 1,
                Naam = "Peter Morlion",
                Tak = "Financieploeg"
            };
            
            // Act
            var naamParts = dto.Naam?.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
            var voornaam = naamParts.Length > 0 ? naamParts[0].ToLower().Trim() : "";
            var achternaam = naamParts.Length > 1 ? string.Join(" ", naamParts.Skip(1)).ToLower().Trim() : "";
            var takNaam = dto.Tak?.Trim() ?? "";
            
            var match = leden.FirstOrDefault(l => 
                l.Voornaam.ToLower().Trim() == voornaam && 
                l.Achternaam.ToLower().Trim() == achternaam &&
                l.Tak.Naam.Trim() == takNaam);
            
            // Assert
            Assert.NotNull(match);
            Assert.Equal(2, match.Id);
            Assert.Equal("Financieploeg", match.Tak.Naam);
        }

        [Fact]
        public void FullMatchingLogic_CombinedStreetAndMember()
        {
            // Arrange
            var tak1 = new Tak("Scheepsmakkers") { Id = 1 };
            
            var street1 = new Straat("Jacob van Maerlantstraat", "Brugge")
            {
                Id = 1,
                PostNummer = 8000,
                Naam = "Jacob van Maerlantstraat"
            };
            
            var lid1 = new Lid("Peter", "Morlion") { Id = 1, Tak = tak1 };
            
            var straten = new List<Straat> { street1 };
            var leden = new List<Lid> { lid1 };
            
            var dto = new PendingOnlineBestellingDto
            {
                Id = 1,
                Naam = "Peter Morlion",
                Straat = "Jacob van Maerlantstraat",
                Postcode = "8000",
                Tak = "Scheepsmakkers"
            };
            
            // Act - simulate the matching logic from ServerDirectClient
            var straatNaam = dto.Straat?.ToLower().Trim() ?? "";
            var postcode = dto.Postcode?.Trim() ?? "";
            var straatMatch = straten.FirstOrDefault(s => 
                s.Naam.ToLower().Trim() == straatNaam && s.PostNummer.ToString() == postcode);
            
            var naamParts = dto.Naam?.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
            var lidVoornaam = naamParts.Length > 0 ? naamParts[0].ToLower().Trim() : "";
            var lidAchternaam = naamParts.Length > 1 ? string.Join(" ", naamParts.Skip(1)).ToLower().Trim() : "";
            var takNaam = dto.Tak?.Trim() ?? "";
            
            var lidMatch = leden.FirstOrDefault(l => 
                l.Voornaam.ToLower().Trim() == lidVoornaam && 
                l.Achternaam.ToLower().Trim() == lidAchternaam &&
                l.Tak.Naam.Trim() == takNaam);
            
            // Assert
            Assert.NotNull(straatMatch);
            Assert.Equal(1, straatMatch.Id);
            Assert.NotNull(lidMatch);
            Assert.Equal(1, lidMatch.Id);
            Assert.True(straatMatch != null && lidMatch != null);
        }

        [Fact]
        public void FullMatchingLogic_NoMatch_WhenStreetAndMemberDontMatch()
        {
            // Arrange
            var tak1 = new Tak("Scheepsmakkers") { Id = 1 };
            var tak2 = new Tak("Financieploeg") { Id = 2 };
            
            var street1 = new Straat("Jacob van Maerlantstraat", "Brugge")
            {
                Id = 1,
                PostNummer = 8000,
                Naam = "Jacob van Maerlantstraat"
            };
            
            var street2 = new Straat("Other Street", "City")
            {
                Id = 2,
                PostNummer = 9000,
                Naam = "Other Street"
            };
            
            var lid1 = new Lid("Peter", "Morlion") { Id = 1, Tak = tak1 };
            var lid2 = new Lid("John", "Doe") { Id = 2, Tak = tak2 };
            
            var straten = new List<Straat> { street1, street2 };
            var leden = new List<Lid> { lid1, lid2 };
            
            var dto = new PendingOnlineBestellingDto
            {
                Id = 1,
                Naam = "Peter Morlion",
                Straat = "Other Street",  // Different street
                Postcode = "8000",  // Different postcode
                Tak = "Scheepsmakkers"
            };
            
            // Act
            var straatNaam = dto.Straat?.ToLower().Trim() ?? "";
            var postcode = dto.Postcode?.Trim() ?? "";
            var straatMatch = straten.FirstOrDefault(s => 
                s.Naam.ToLower().Trim() == straatNaam && s.PostNummer.ToString() == postcode);
            
            var naamParts = dto.Naam?.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
            var lidVoornaam = naamParts.Length > 0 ? naamParts[0].ToLower().Trim() : "";
            var lidAchternaam = naamParts.Length > 1 ? string.Join(" ", naamParts.Skip(1)).ToLower().Trim() : "";
            var takNaam = dto.Tak?.Trim() ?? "";
            
            var lidMatch = leden.FirstOrDefault(l => 
                l.Voornaam.ToLower().Trim() == lidVoornaam && 
                l.Achternaam.ToLower().Trim() == lidAchternaam &&
                l.Tak.Naam.Trim() == takNaam);
            
            // Assert
            Assert.NotNull(lidMatch);  // Member matches
            Assert.Null(straatMatch);   // Street doesn't match (wrong postcode)
            Assert.False(straatMatch != null && lidMatch != null);  // Combined match fails
        }
    }
}
