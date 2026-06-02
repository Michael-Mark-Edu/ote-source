using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class AllEntitiesAndDtos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // This migration is a no-op. The ISBN column handling is done by the RenamedBookISBN migration.
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // This migration is a no-op. The ISBN column handling is done by the RenamedBookISBN migration.
        }
    }
}
