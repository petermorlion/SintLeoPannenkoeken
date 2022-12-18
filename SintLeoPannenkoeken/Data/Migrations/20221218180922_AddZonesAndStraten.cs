using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SintLeoPannenkoeken.Data.Migrations
{
    public partial class AddZonesAndStraten : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bestelling_Leden_LidId",
                table: "Bestelling");

            migrationBuilder.DropForeignKey(
                name: "FK_Bestelling_Scoutsjaren_ScoutsjaarId",
                table: "Bestelling");

            migrationBuilder.DropForeignKey(
                name: "FK_Bestelling_Takken_TakId",
                table: "Bestelling");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bestelling",
                table: "Bestelling");

            migrationBuilder.RenameTable(
                name: "Bestelling",
                newName: "Bestellingen");

            migrationBuilder.RenameIndex(
                name: "IX_Bestelling_TakId",
                table: "Bestellingen",
                newName: "IX_Bestellingen_TakId");

            migrationBuilder.RenameIndex(
                name: "IX_Bestelling_ScoutsjaarId",
                table: "Bestellingen",
                newName: "IX_Bestellingen_ScoutsjaarId");

            migrationBuilder.RenameIndex(
                name: "IX_Bestelling_LidId",
                table: "Bestellingen",
                newName: "IX_Bestellingen_LidId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bestellingen",
                table: "Bestellingen",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Zones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostNummer = table.Column<int>(type: "int", nullable: true),
                    Gemeente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    KaartNummer = table.Column<int>(type: "int", nullable: true),
                    Omschrijving = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bestuurder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZoneId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zones_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Straten",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostNummer = table.Column<int>(type: "int", nullable: false),
                    Gemeente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Omschrijving = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZoneId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Straten", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Straten_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Straten_ZoneId",
                table: "Straten",
                column: "ZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_Zones_ZoneId",
                table: "Zones",
                column: "ZoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bestellingen_Leden_LidId",
                table: "Bestellingen",
                column: "LidId",
                principalTable: "Leden",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bestellingen_Scoutsjaren_ScoutsjaarId",
                table: "Bestellingen",
                column: "ScoutsjaarId",
                principalTable: "Scoutsjaren",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bestellingen_Takken_TakId",
                table: "Bestellingen",
                column: "TakId",
                principalTable: "Takken",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bestellingen_Leden_LidId",
                table: "Bestellingen");

            migrationBuilder.DropForeignKey(
                name: "FK_Bestellingen_Scoutsjaren_ScoutsjaarId",
                table: "Bestellingen");

            migrationBuilder.DropForeignKey(
                name: "FK_Bestellingen_Takken_TakId",
                table: "Bestellingen");

            migrationBuilder.DropTable(
                name: "Straten");

            migrationBuilder.DropTable(
                name: "Zones");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bestellingen",
                table: "Bestellingen");

            migrationBuilder.RenameTable(
                name: "Bestellingen",
                newName: "Bestelling");

            migrationBuilder.RenameIndex(
                name: "IX_Bestellingen_TakId",
                table: "Bestelling",
                newName: "IX_Bestelling_TakId");

            migrationBuilder.RenameIndex(
                name: "IX_Bestellingen_ScoutsjaarId",
                table: "Bestelling",
                newName: "IX_Bestelling_ScoutsjaarId");

            migrationBuilder.RenameIndex(
                name: "IX_Bestellingen_LidId",
                table: "Bestelling",
                newName: "IX_Bestelling_LidId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bestelling",
                table: "Bestelling",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bestelling_Leden_LidId",
                table: "Bestelling",
                column: "LidId",
                principalTable: "Leden",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bestelling_Scoutsjaren_ScoutsjaarId",
                table: "Bestelling",
                column: "ScoutsjaarId",
                principalTable: "Scoutsjaren",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bestelling_Takken_TakId",
                table: "Bestelling",
                column: "TakId",
                principalTable: "Takken",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
