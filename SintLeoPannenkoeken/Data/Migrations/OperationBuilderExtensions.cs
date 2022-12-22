using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace SintLeoPannenkoeken.Data.Migrations
{
    public static class OperationBuilderExtensions
    {
        public static OperationBuilder<SqlOperation> InsertScoutsjaar(
            this MigrationBuilder migrationBuilder,
            int begin,
            int einde)
        {
            return migrationBuilder.Sql($"INSERT INTO Scoutsjaren ([Begin], [Einde]) VALUES ('{begin}', '{einde}');");
        }

        public static OperationBuilder<SqlOperation> InsertTak(
            this MigrationBuilder migrationBuilder,
            string naam)
        {
            return migrationBuilder.Sql($"INSERT INTO Takken ([Naam]) VALUES ('{naam}');");
        }

        public static OperationBuilder<SqlOperation> InsertZone(
            this MigrationBuilder migrationBuilder,
            string naam,
            int postNummer,
            string gemeente,
            string kaartNummer,
            string omschrijving)
        {
            return migrationBuilder.Sql($"INSERT INTO Zones ([Naam], [PostNummer], [Gemeente], [KaartNummer], [Omschrijving]) " +
                $"VALUES ('{naam}', {postNummer}, '{gemeente}', '{kaartNummer}', '{omschrijving}');");
        }

        public static OperationBuilder<SqlOperation> InsertStraat(
            this MigrationBuilder migrationBuilder,
            string zone,
            string naam,
            int postNummer,
            string gemeente,
            string omschrijving)
        {
            return migrationBuilder.Sql($"INSERT INTO Straten ([Naam], [PostNummer], [Gemeente], [Omschrijving], [ZoneId]) " +
                $"SELECT '{naam}', {postNummer}, '{gemeente}', '{omschrijving}', Id FROM Zones WHERE Zones.Naam = '{zone}';");
        }

        public static OperationBuilder<SqlOperation> SetTakAfkorting(
            this MigrationBuilder migrationBuilder,
            string takNaam,
            string afkorting)
        {
            return migrationBuilder.Sql($"UPDATE Takken SET Afkorting = '{afkorting}' WHERE Naam = '{takNaam}';");
        }
    }
}
