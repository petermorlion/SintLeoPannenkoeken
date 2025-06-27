using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SintLeoPannenkoeken.Blazor.Data.Migrations
{
    public partial class CreateInitialTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Leden",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Voornaam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Achternaam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Functie = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leden", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Scoutsjaren",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Begin = table.Column<int>(type: "int", nullable: false),
                    Einde = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scoutsjaren", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bestelling",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AantalPakken = table.Column<int>(type: "int", nullable: false),
                    Opmerkingen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Betaald = table.Column<bool>(type: "bit", nullable: false),
                    ScoutsjaarId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bestelling", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bestelling_Scoutsjaren_ScoutsjaarId",
                        column: x => x.ScoutsjaarId,
                        principalTable: "Scoutsjaren",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bestelling_ScoutsjaarId",
                table: "Bestelling",
                column: "ScoutsjaarId");

            migrationBuilder.CreateIndex(
                name: "IX_Scoutsjaren_Begin",
                table: "Scoutsjaren",
                column: "Begin",
                unique: true);

            migrationBuilder.InsertScoutsjaar(2022, 2023);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bestelling");

            migrationBuilder.DropTable(
                name: "Leden");

            migrationBuilder.DropTable(
                name: "Scoutsjaren");
        }
    }
}
