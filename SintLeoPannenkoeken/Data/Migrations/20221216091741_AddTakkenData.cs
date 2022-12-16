using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SintLeoPannenkoeken.Data.Migrations
{
    public partial class AddTakkenData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertTak("Zeewolfjes");
            migrationBuilder.InsertTak("Zeepaardjes");
            migrationBuilder.InsertTak("Dolfijnen");
            migrationBuilder.InsertTak("Zeerobben");
            migrationBuilder.InsertTak("Scheepsmakkers");
            migrationBuilder.InsertTak("Zeeverkenners");
            migrationBuilder.InsertTak("Loodsen");
            migrationBuilder.InsertTak("Bootslui");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
