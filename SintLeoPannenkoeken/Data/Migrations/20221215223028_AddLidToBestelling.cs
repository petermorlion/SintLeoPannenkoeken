using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SintLeoPannenkoeken.Data.Migrations
{
    public partial class AddLidToBestelling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LidId",
                table: "Bestelling",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bestelling_LidId",
                table: "Bestelling",
                column: "LidId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bestelling_Leden_LidId",
                table: "Bestelling",
                column: "LidId",
                principalTable: "Leden",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bestelling_Leden_LidId",
                table: "Bestelling");

            migrationBuilder.DropIndex(
                name: "IX_Bestelling_LidId",
                table: "Bestelling");

            migrationBuilder.DropColumn(
                name: "LidId",
                table: "Bestelling");
        }
    }
}
