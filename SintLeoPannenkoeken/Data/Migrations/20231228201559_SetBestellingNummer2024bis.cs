using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SintLeoPannenkoeken.Data.Migrations
{
    /// <inheritdoc />
    public partial class SetBestellingNummer2024bis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bestellingen_Scoutsjaren_ScoutsjaarId",
                table: "Bestellingen");

            migrationBuilder.AlterColumn<int>(
                name: "ScoutsjaarId",
                table: "Bestellingen",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bestellingen_Scoutsjaren_ScoutsjaarId",
                table: "Bestellingen",
                column: "ScoutsjaarId",
                principalTable: "Scoutsjaren",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bestellingen_Scoutsjaren_ScoutsjaarId",
                table: "Bestellingen");

            migrationBuilder.AlterColumn<int>(
                name: "ScoutsjaarId",
                table: "Bestellingen",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Bestellingen_Scoutsjaren_ScoutsjaarId",
                table: "Bestellingen",
                column: "ScoutsjaarId",
                principalTable: "Scoutsjaren",
                principalColumn: "Id");
        }
    }
}
