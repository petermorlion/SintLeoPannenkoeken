﻿using Microsoft.EntityFrameworkCore.Migrations;
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

        public static OperationBuilder<SqlOperation> InsertTak(
            this MigrationBuilder migrationBuilder,
            string naam,
            string afkorting)
        {
            return migrationBuilder.Sql($"INSERT INTO Takken ([Naam], [Afkorting]) VALUES ('{naam}', '{afkorting}');");
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

        public static OperationBuilder<SqlOperation> InsertLid(
            this MigrationBuilder migrationBuilder,
            string voorNaam,
            string achterNaam,
            string takNaam,
            string functie)
        {
            return migrationBuilder.Sql($"INSERT INTO Leden (Voornaam, Achternaam, TakId, Functie) SELECT '{voorNaam}', '{achterNaam}', Id, '{functie}' FROM Takken WHERE Afkorting = '{takNaam}';");
        }
        public static OperationBuilder<SqlOperation> UpdateStraatNummer(
            this MigrationBuilder migrationBuilder,
            string straatNaam,
            string zoneNaam,
            string straatNummer)
        {
            return migrationBuilder.Sql($"UPDATE Straten SET Nummer = '{straatNummer}' WHERE Naam = '{straatNaam}' AND ZoneId = (SELECT Id FROM Zones WHERE Naam = '{zoneNaam}');");
        }
    }
}
