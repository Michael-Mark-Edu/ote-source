using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class Argon2idPasswords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Argon2idPasswords",
                schema: "public",
                columns: table => new
                {
                    Argon2idPasswordId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Version = table.Column<byte>(type: "smallint", nullable: false),
                    MemoryCost = table.Column<int>(type: "integer", nullable: false),
                    Iterations = table.Column<int>(type: "integer", nullable: false),
                    Parallelism = table.Column<byte>(type: "smallint", nullable: false),
                    Salt = table.Column<byte[]>(type: "bytea", nullable: false),
                    Hash = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Argon2idPasswords", x => x.Argon2idPasswordId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Argon2idPasswords",
                schema: "public");
        }
    }
}
