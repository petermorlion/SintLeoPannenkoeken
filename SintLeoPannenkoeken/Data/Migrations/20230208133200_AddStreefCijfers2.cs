using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SintLeoPannenkoeken.Data.Migrations
{
    public partial class AddStreefCijfers2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StreefCijfer_Scoutsjaren_ScoutsjaarId",
                table: "StreefCijfer");

            migrationBuilder.DropForeignKey(
                name: "FK_StreefCijfer_Takken_TakId",
                table: "StreefCijfer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StreefCijfer",
                table: "StreefCijfer");

            migrationBuilder.RenameTable(
                name: "StreefCijfer",
                newName: "StreefCijfers");

            migrationBuilder.RenameIndex(
                name: "IX_StreefCijfer_TakId",
                table: "StreefCijfers",
                newName: "IX_StreefCijfers_TakId");

            migrationBuilder.RenameIndex(
                name: "IX_StreefCijfer_ScoutsjaarId",
                table: "StreefCijfers",
                newName: "IX_StreefCijfers_ScoutsjaarId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StreefCijfers",
                table: "StreefCijfers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StreefCijfers_Scoutsjaren_ScoutsjaarId",
                table: "StreefCijfers",
                column: "ScoutsjaarId",
                principalTable: "Scoutsjaren",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StreefCijfers_Takken_TakId",
                table: "StreefCijfers",
                column: "TakId",
                principalTable: "Takken",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StreefCijfers_Scoutsjaren_ScoutsjaarId",
                table: "StreefCijfers");

            migrationBuilder.DropForeignKey(
                name: "FK_StreefCijfers_Takken_TakId",
                table: "StreefCijfers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StreefCijfers",
                table: "StreefCijfers");

            migrationBuilder.RenameTable(
                name: "StreefCijfers",
                newName: "StreefCijfer");

            migrationBuilder.RenameIndex(
                name: "IX_StreefCijfers_TakId",
                table: "StreefCijfer",
                newName: "IX_StreefCijfer_TakId");

            migrationBuilder.RenameIndex(
                name: "IX_StreefCijfers_ScoutsjaarId",
                table: "StreefCijfer",
                newName: "IX_StreefCijfer_ScoutsjaarId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StreefCijfer",
                table: "StreefCijfer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StreefCijfer_Scoutsjaren_ScoutsjaarId",
                table: "StreefCijfer",
                column: "ScoutsjaarId",
                principalTable: "Scoutsjaren",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StreefCijfer_Takken_TakId",
                table: "StreefCijfer",
                column: "TakId",
                principalTable: "Takken",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
