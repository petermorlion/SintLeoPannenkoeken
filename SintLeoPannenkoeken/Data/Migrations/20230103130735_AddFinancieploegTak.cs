using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SintLeoPannenkoeken.Data.Migrations
{
    public partial class AddFinancieploegTak : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertTak("Financieploeg");
            migrationBuilder.SetTakAfkorting("Financieploeg", "FP");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            return migrationBuilder.Sql($"DELETE FROM Takken WHERE [Naam] = '{naam}';");
        }
    }
}
