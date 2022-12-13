using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SintLeoPannenkoeken.Data.Migrations
{
    public partial class AddScoutsjarenData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertScoutsjaar(new DateTime(2022, 9, 1), new DateTime(2023, 8, 31, 23, 59, 59));
            migrationBuilder.InsertScoutsjaar(new DateTime(2023, 9, 1), new DateTime(2024, 8, 31, 23, 59, 59));

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
