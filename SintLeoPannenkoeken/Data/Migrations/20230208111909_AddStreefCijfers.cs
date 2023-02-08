using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SintLeoPannenkoeken.Data.Migrations
{
    public partial class AddStreefCijfers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StreefCijfer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TakId = table.Column<int>(type: "int", nullable: false),
                    Aantal = table.Column<int>(type: "int", nullable: false),
                    ScoutsjaarId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreefCijfer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StreefCijfer_Scoutsjaren_ScoutsjaarId",
                        column: x => x.ScoutsjaarId,
                        principalTable: "Scoutsjaren",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StreefCijfer_Takken_TakId",
                        column: x => x.TakId,
                        principalTable: "Takken",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StreefCijfer_ScoutsjaarId",
                table: "StreefCijfer",
                column: "ScoutsjaarId");

            migrationBuilder.CreateIndex(
                name: "IX_StreefCijfer_TakId",
                table: "StreefCijfer",
                column: "TakId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StreefCijfer");
        }
    }
}
