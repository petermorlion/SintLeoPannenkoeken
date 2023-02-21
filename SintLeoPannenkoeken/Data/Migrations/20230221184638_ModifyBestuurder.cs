using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SintLeoPannenkoeken.Data.Migrations
{
    public partial class ModifyBestuurder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Naam",
                table: "Bestuurders",
                newName: "Voornaam");

            migrationBuilder.AddColumn<string>(
                name: "Achternaam",
                table: "Bestuurders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Achternaam",
                table: "Bestuurders");

            migrationBuilder.RenameColumn(
                name: "Voornaam",
                table: "Bestuurders",
                newName: "Naam");
        }
    }
}
