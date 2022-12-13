using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace SintLeoPannenkoeken.Data.Migrations
{
    public static class OperationBuilderExtensions
    {
        public static OperationBuilder<SqlOperation> InsertScoutsjaar(
            this MigrationBuilder migrationBuilder,
            DateTime begin,
            DateTime einde)
        {
            return migrationBuilder.Sql($"INSERT INTO Scoutsjaren ([Begin], [Einde]) VALUES ('{begin.ToString("yyyy-MM-dd HH:mm:ss")}', '{einde.ToString("yyyy-MM-dd HH:mm:ss")}');");
        }
    }
}
