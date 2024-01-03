using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SintLeoPannenkoeken.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveScoutsjaarEinde : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Einde",
                table: "Scoutsjaren");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Einde",
                table: "Scoutsjaren",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
