using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class OtherISBNFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.RenameColumn(
            //     name: "BookISBN",
            //     schema: "public",
            //     table: "Books",
            //     newName: "ISBN");
            //
            // migrationBuilder.CreateIndex(
            //     name: "IX_BookListings_BookISBN",
            //     schema: "public",
            //     table: "BookListings",
            //     column: "BookISBN");
            //
            // migrationBuilder.AddForeignKey(
            //     name: "FK_BookListings_Books_BookISBN",
            //     schema: "public",
            //     table: "BookListings",
            //     column: "BookISBN",
            //     principalSchema: "public",
            //     principalTable: "Books",
            //     principalColumn: "ISBN",
            //     onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_BookListings_Books_BookISBN",
            //     schema: "public",
            //     table: "BookListings");
            //
            // migrationBuilder.DropIndex(
            //     name: "IX_BookListings_BookISBN",
            //     schema: "public",
            //     table: "BookListings");
            //
            // migrationBuilder.RenameColumn(
            //     name: "ISBN",
            //     schema: "public",
            //     table: "Books",
            //     newName: "BookISBN");
        }
    }
}
