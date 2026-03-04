using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class Books : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                schema: "public",
                columns: table => new
                {
                    ISBN = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Authors = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Publishers = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    PublishDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.ISBN);
                });

            migrationBuilder.CreateTable(
                name: "BookListings",
                schema: "public",
                columns: table => new
                {
                    BookListingId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Condition = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PurchaseType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Price = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SellerId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ISBN = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookListings", x => x.BookListingId);
                    table.ForeignKey(
                        name: "FK_BookListings_Books_ISBN",
                        column: x => x.ISBN,
                        principalSchema: "public",
                        principalTable: "Books",
                        principalColumn: "ISBN",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookListings_Users_SellerId",
                        column: x => x.SellerId,
                        principalSchema: "public",
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookListings",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Books",
                schema: "public");
        }
    }
}
