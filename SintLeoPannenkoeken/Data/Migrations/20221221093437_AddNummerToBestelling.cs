using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SintLeoPannenkoeken.Data.Migrations
{
    public partial class AddNummerToBestelling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nummer",
                table: "Bestellingen",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nummer",
                table: "Bestellingen");
        }
    }
}
