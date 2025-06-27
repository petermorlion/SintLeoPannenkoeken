using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SintLeoPannenkoeken.Blazor.Data.Migrations
{
    /// <inheritdoc />
    public partial class SetFinanciePloegUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO dbo.AspNetRoles (Id, [Name], NormalizedName, ConcurrencyStamp) VALUES (NEWID(), 'FinanciePloeg', 'FINANCIEPLOEG', NEWID())");
            migrationBuilder.Sql("INSERT INTO dbo.AspNetUserRoles (UserId, RoleId) " +
                "SELECT u.Id AS 'UserId', r2.Id as 'RoleId' " +
                "FROM dbo.AspNetUsers u " +
                "LEFT OUTER JOIN dbo.AspNetUserRoles ur ON ur.UserId = u.Id " +
                "LEFT OUTER JOIN dbo.AspNetRoles r ON ur.RoleId = r.Id " +
                "LEFT OUTER JOIN dbo.AspNetRoles r2 ON r2.NormalizedName = 'FINANCIEPLOEG' " +
                "WHERE r.Id IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM dbo.AspNetUserRoles WHERE RoleId IN (SELECT Id FROM dbo.AspNetRoles WHERE NormalizedName = 'FINANCIEPLOEG')");
            migrationBuilder.Sql("DELETE FROM dbo.AspNetRoles WHERE NormalizedName = 'FINANCIEPLOEG'");
        }
    }
}
