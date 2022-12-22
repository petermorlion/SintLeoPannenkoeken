using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SintLeoPannenkoeken.Data.Migrations
{
    public partial class AddTakAfkorting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Afkorting",
                table: "Takken",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.SetTakAfkorting("Zeewolfjes", "ZW");
            migrationBuilder.SetTakAfkorting("Zeepaardjes", "ZP");
            migrationBuilder.SetTakAfkorting("Dolfijnen", "DO");
            migrationBuilder.SetTakAfkorting("Zeerobben", "ZR");
            migrationBuilder.SetTakAfkorting("Scheepsmakkers", "SM");
            migrationBuilder.SetTakAfkorting("Zeeverkenners", "ZV");
            migrationBuilder.SetTakAfkorting("Loodsen", "LO");
            migrationBuilder.SetTakAfkorting("Bootslui", "BL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Afkorting",
                table: "Takken");
        }
    }
}
