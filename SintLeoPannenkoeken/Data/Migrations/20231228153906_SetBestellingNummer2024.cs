using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SintLeoPannenkoeken.Data.Migrations
{
    /// <inheritdoc />
    public partial class SetBestellingNummer2024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("" +
                "UPDATE Bestellingen " +
                "SET BestellingNummer = (Id - (SELECT TOP 1 Id FROM Bestellingen WHERE ScoutsjaarId = " +
                "    (SELECT Id FROM Scoutsjaren WHERE [Begin] = 2023) " +
                "    ORDER BY Id ASC) + 1) " +
                "WHERE ScoutsjaarId = (SELECT Id FROM Scoutsjaren WHERE [Begin] = 2023) ");

            migrationBuilder.Sql("" +
                "UPDATE Bestellingen " +
                "SET BestellingNummer = (Id - (SELECT TOP 1 Id FROM Bestellingen WHERE ScoutsjaarId = " +
                "    (SELECT Id FROM Scoutsjaren WHERE [Begin] = 2024) " +
                "    ORDER BY Id ASC) + 1) " +
                "WHERE ScoutsjaarId = (SELECT Id FROM Scoutsjaren WHERE [Begin] = 2024) ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
