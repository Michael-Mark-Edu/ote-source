using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class SchoolEntityRenaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SchoolID",
                schema: "public",
                table: "Schools",
                newName: "SchoolId");

            migrationBuilder.RenameColumn(
                name: "SchoolName",
                schema: "public",
                table: "Schools",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "SchoolAcronym",
                schema: "public",
                table: "Schools",
                newName: "Acronym");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SchoolId",
                schema: "public",
                table: "Schools",
                newName: "SchoolID");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "public",
                table: "Schools",
                newName: "SchoolName");

            migrationBuilder.RenameColumn(
                name: "Acronym",
                schema: "public",
                table: "Schools",
                newName: "SchoolAcronym");
        }
    }
}
