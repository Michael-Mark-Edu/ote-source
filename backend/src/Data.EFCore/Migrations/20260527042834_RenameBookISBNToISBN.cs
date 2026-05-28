using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class RenameBookISBNToISBN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookListings_Books_BookISBN",
                schema: "public",
                table: "BookListings");

            migrationBuilder.DropIndex(
                name: "IX_BookListings_BookISBN",
                schema: "public",
                table: "BookListings");

            migrationBuilder.DropColumn(
                name: "BookISBN",
                schema: "public",
                table: "BookListings");

            migrationBuilder.CreateIndex(
                name: "IX_BookListings_ISBN",
                schema: "public",
                table: "BookListings",
                column: "ISBN");

            migrationBuilder.AddForeignKey(
                name: "FK_BookListings_Books_ISBN",
                schema: "public",
                table: "BookListings",
                column: "ISBN",
                principalSchema: "public",
                principalTable: "Books",
                principalColumn: "ISBN",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookListings_Books_ISBN",
                schema: "public",
                table: "BookListings");

            migrationBuilder.DropIndex(
                name: "IX_BookListings_ISBN",
                schema: "public",
                table: "BookListings");

            migrationBuilder.AddColumn<string>(
                name: "BookISBN",
                schema: "public",
                table: "BookListings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_BookListings_BookISBN",
                schema: "public",
                table: "BookListings",
                column: "BookISBN");

            migrationBuilder.AddForeignKey(
                name: "FK_BookListings_Books_BookISBN",
                schema: "public",
                table: "BookListings",
                column: "BookISBN",
                principalSchema: "public",
                principalTable: "Books",
                principalColumn: "ISBN",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
