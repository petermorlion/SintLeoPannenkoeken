using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SintLeoPannenkoeken.Data.Migrations
{
    public partial class FixZoneChildren : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Zones_Zones_ZoneId",
                table: "Zones");

            migrationBuilder.DropIndex(
                name: "IX_Zones_ZoneId",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "ZoneId",
                table: "Zones");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ZoneId",
                table: "Zones",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Zones_ZoneId",
                table: "Zones",
                column: "ZoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Zones_Zones_ZoneId",
                table: "Zones",
                column: "ZoneId",
                principalTable: "Zones",
                principalColumn: "Id");
        }
    }
}
