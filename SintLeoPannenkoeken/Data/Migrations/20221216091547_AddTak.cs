using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SintLeoPannenkoeken.Data.Migrations
{
    public partial class AddTak : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TakId",
                table: "Leden",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Takken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Takken", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Leden_TakId",
                table: "Leden",
                column: "TakId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leden_Takken_TakId",
                table: "Leden",
                column: "TakId",
                principalTable: "Takken",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leden_Takken_TakId",
                table: "Leden");

            migrationBuilder.DropTable(
                name: "Takken");

            migrationBuilder.DropIndex(
                name: "IX_Leden_TakId",
                table: "Leden");

            migrationBuilder.DropColumn(
                name: "TakId",
                table: "Leden");
        }
    }
}
