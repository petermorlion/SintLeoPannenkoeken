using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SintLeoPannenkoeken.Blazor.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveProposedTakIdFromHangendeBestellingen : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProposedTakId",
                table: "HangendeBestellingen");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProposedTakId",
                table: "HangendeBestellingen",
                type: "int",
                nullable: true);
        }
    }
}
