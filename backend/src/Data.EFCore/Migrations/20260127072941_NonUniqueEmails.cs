using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class NonUniqueEmails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_EmailAddress",
                schema: "public",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_EmailAddress",
                schema: "public",
                table: "Users",
                column: "EmailAddress",
                unique: true);
        }
    }
}
