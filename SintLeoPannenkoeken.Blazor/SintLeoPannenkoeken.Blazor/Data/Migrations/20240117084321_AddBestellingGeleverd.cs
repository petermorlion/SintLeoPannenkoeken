using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SintLeoPannenkoeken.Blazor.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBestellingGeleverd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Geleverd",
                table: "Bestellingen",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Geleverd",
                table: "Bestellingen");
        }
    }
}
