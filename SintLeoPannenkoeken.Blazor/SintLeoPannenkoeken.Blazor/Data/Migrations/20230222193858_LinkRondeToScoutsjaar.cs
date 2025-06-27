using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SintLeoPannenkoeken.Blazor.Data.Migrations
{
    public partial class LinkRondeToScoutsjaar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScoutsjaarId",
                table: "Rondes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rondes_ScoutsjaarId",
                table: "Rondes",
                column: "ScoutsjaarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rondes_Scoutsjaren_ScoutsjaarId",
                table: "Rondes",
                column: "ScoutsjaarId",
                principalTable: "Scoutsjaren",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rondes_Scoutsjaren_ScoutsjaarId",
                table: "Rondes");

            migrationBuilder.DropIndex(
                name: "IX_Rondes_ScoutsjaarId",
                table: "Rondes");

            migrationBuilder.DropColumn(
                name: "ScoutsjaarId",
                table: "Rondes");
        }
    }
}
