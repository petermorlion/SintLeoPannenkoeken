using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SintLeoPannenkoeken.Data.Migrations
{
    public partial class AddTakIdToBestelling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TakId",
                table: "Bestelling",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Bestelling_TakId",
                table: "Bestelling",
                column: "TakId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bestelling_Takken_TakId",
                table: "Bestelling",
                column: "TakId",
                principalTable: "Takken",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bestelling_Takken_TakId",
                table: "Bestelling");

            migrationBuilder.DropIndex(
                name: "IX_Bestelling_TakId",
                table: "Bestelling");

            migrationBuilder.DropColumn(
                name: "TakId",
                table: "Bestelling");
        }
    }
}
