using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SintLeoPannenkoeken.Blazor.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddHangendeBestellingen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HangendeBestellingen",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OnlineBestellingId = table.Column<int>(type: "int", nullable: false),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefoon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AantalPakken = table.Column<int>(type: "int", nullable: false),
                    Opmerkingen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Betaald = table.Column<bool>(type: "bit", nullable: false),
                    Geleverd = table.Column<bool>(type: "bit", nullable: false),
                    Straat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nummer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Postcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gemeente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LidNaam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tak = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Levering = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatumAangemaakt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ScoutsjaarId = table.Column<int>(type: "int", nullable: false),
                    ProposedStraatId = table.Column<int>(type: "int", nullable: true),
                    ProposedLidId = table.Column<int>(type: "int", nullable: true),
                    ProposedTakId = table.Column<int>(type: "int", nullable: true),
                    Verwerkt = table.Column<bool>(type: "bit", nullable: false),
                    RawData = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangendeBestellingen", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HangendeBestellingen");
        }
    }
}
