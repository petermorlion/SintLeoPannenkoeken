using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SintLeoPannenkoeken.Blazor.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingTakken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertTak("Afdelingsleiding", "AL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
