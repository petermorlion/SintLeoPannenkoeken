using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SintLeoPannenkoeken.Blazor.Data.Migrations
{
    public partial class AddStraatIdToBestelling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StraatId",
                table: "Bestellingen",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bestellingen_StraatId",
                table: "Bestellingen",
                column: "StraatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bestellingen_Straten_StraatId",
                table: "Bestellingen",
                column: "StraatId",
                principalTable: "Straten",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bestellingen_Straten_StraatId",
                table: "Bestellingen");

            migrationBuilder.DropIndex(
                name: "IX_Bestellingen_StraatId",
                table: "Bestellingen");

            migrationBuilder.DropColumn(
                name: "StraatId",
                table: "Bestellingen");
        }
    }
}
