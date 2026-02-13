using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class PasswordStoresUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                schema: "public",
                table: "Argon2idPasswords",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Argon2idPasswords_UserId",
                schema: "public",
                table: "Argon2idPasswords",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Argon2idPasswords_Users_UserId",
                schema: "public",
                table: "Argon2idPasswords",
                column: "UserId",
                principalSchema: "public",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            // migrationBuilder.Sql(
            // """
            // INSERT INTO "Argon2idPasswords" AS a ("UserId")
            // SELECT "UserId" FROM "Users" AS u WHERE u."Argon2idPasswordId" = a."Argon2idPasswordId";
            // """);

            migrationBuilder.Sql(
            """
            UPDATE "Argon2idPasswords" AS a SET "UserId" = (
                SELECT "UserId" FROM "Users" AS u
                WHERE a."Argon2idPasswordId" = u."Argon2idPasswordId"
            );
            """);

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Argon2idPasswords_Argon2idPasswordId",
                schema: "public",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Argon2idPasswordId",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Argon2idPasswordId",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                schema: "public",
                table: "Argon2idPasswords");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Argon2idPasswordId",
                schema: "public",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                schema: "public",
                table: "Argon2idPasswords",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Argon2idPasswordId",
                schema: "public",
                table: "Users",
                column: "Argon2idPasswordId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Argon2idPasswords_Argon2idPasswordId",
                schema: "public",
                table: "Users",
                column: "Argon2idPasswordId",
                principalSchema: "public",
                principalTable: "Argon2idPasswords",
                principalColumn: "Argon2idPasswordId",
                onDelete: ReferentialAction.Cascade);

            // migrationBuilder.Sql(
            // """
            // INSERT INTO "Users" AS u ("Argon2idPasswordId")
            // SELECT "Argon2idPasswordId" FROM "Argon2idPasswords" AS a WHERE a."Argon2idPasswordId" = u."Argon2idPasswordId";
            // """);

            migrationBuilder.Sql(
            """
            UPDATE "Users" AS u SET "Argon2idPasswordId" = (
                SELECT "Argon2idPasswordId" FROM "Argon2idPasswords" AS a
                WHERE a."Argon2idPasswordId" = u."Argon2idPasswordId"
            );
            """);

            migrationBuilder.DropForeignKey(
                name: "FK_Argon2idPasswords_Users_UserId",
                schema: "public",
                table: "Argon2idPasswords");

            migrationBuilder.DropIndex(
                name: "IX_Argon2idPasswords_UserId",
                schema: "public",
                table: "Argon2idPasswords");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "public",
                table: "Argon2idPasswords");
        }
    }
}
