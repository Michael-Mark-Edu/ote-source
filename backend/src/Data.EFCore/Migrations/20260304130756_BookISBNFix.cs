using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class BookISBNFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ISBN",
                schema: "public",
                table: "BookListings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ISBN",
                schema: "public",
                table: "BookListings",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
