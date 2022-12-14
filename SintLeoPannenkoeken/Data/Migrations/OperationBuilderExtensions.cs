using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace SintLeoPannenkoeken.Data.Migrations
{
    public static class OperationBuilderExtensions
    {
        public static OperationBuilder<SqlOperation> InsertScoutsjaar(
            this MigrationBuilder migrationBuilder,
            int begin,
            int einde)
        {
            return migrationBuilder.Sql($"INSERT INTO Scoutsjaren ([Begin], [Einde]) VALUES ('{begin}', '{einde}');");
        }
    }
}
