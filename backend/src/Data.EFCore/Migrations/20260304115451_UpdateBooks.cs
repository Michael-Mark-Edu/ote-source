using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookListings_Books_ISBN",
                schema: "public",
                table: "BookListings");

            migrationBuilder.DropForeignKey(
                name: "FK_BookListings_Users_SellerId",
                schema: "public",
                table: "BookListings");

            migrationBuilder.DropIndex(
                name: "IX_BookListings_ISBN",
                schema: "public",
                table: "BookListings");

            migrationBuilder.DropIndex(
                name: "IX_BookListings_SellerId",
                schema: "public",
                table: "BookListings");

            migrationBuilder.DropColumn(
                name: "SellerId",
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

            migrationBuilder.CreateIndex(
                name: "IX_BookListings_UserId",
                schema: "public",
                table: "BookListings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookListings_Books_BookISBN",
                schema: "public",
                table: "BookListings",
                column: "BookISBN",
                principalSchema: "public",
                principalTable: "Books",
                principalColumn: "ISBN",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookListings_Users_UserId",
                schema: "public",
                table: "BookListings",
                column: "UserId",
                principalSchema: "public",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookListings_Books_BookISBN",
                schema: "public",
                table: "BookListings");

            migrationBuilder.DropForeignKey(
                name: "FK_BookListings_Users_UserId",
                schema: "public",
                table: "BookListings");

            migrationBuilder.DropIndex(
                name: "IX_BookListings_BookISBN",
                schema: "public",
                table: "BookListings");

            migrationBuilder.DropIndex(
                name: "IX_BookListings_UserId",
                schema: "public",
                table: "BookListings");

            migrationBuilder.DropColumn(
                name: "BookISBN",
                schema: "public",
                table: "BookListings");

            migrationBuilder.AddColumn<int>(
                name: "SellerId",
                schema: "public",
                table: "BookListings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BookListings_ISBN",
                schema: "public",
                table: "BookListings",
                column: "ISBN");

            migrationBuilder.CreateIndex(
                name: "IX_BookListings_SellerId",
                schema: "public",
                table: "BookListings",
                column: "SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookListings_Books_ISBN",
                schema: "public",
                table: "BookListings",
                column: "ISBN",
                principalSchema: "public",
                principalTable: "Books",
                principalColumn: "ISBN",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookListings_Users_SellerId",
                schema: "public",
                table: "BookListings",
                column: "SellerId",
                principalSchema: "public",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
