using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class UniquenessFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Username_EmailAddress",
                schema: "public",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmailAddress",
                schema: "public",
                table: "Users",
                column: "EmailAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                schema: "public",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_EmailAddress",
                schema: "public",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Username",
                schema: "public",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username_EmailAddress",
                schema: "public",
                table: "Users",
                columns: new[] { "Username", "EmailAddress" },
                unique: true);
        }
    }
}
