using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ApniDukaan.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenamePasswordToPasswordHash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ApplicationUsers",
                keyColumn: "UserId",
                keyValue: new Guid("9194ab4e-f176-4e28-9bfb-e11d377b4489"));

            migrationBuilder.DeleteData(
                table: "ApplicationUsers",
                keyColumn: "UserId",
                keyValue: new Guid("b6b7e2e0-1c43-4f89-8d1b-4d7d7b4a0001"));

            migrationBuilder.DeleteData(
                table: "ApplicationUsers",
                keyColumn: "UserId",
                keyValue: new Guid("b6b7e2e0-1c43-4f89-8d1b-4d7d7b4a0002"));

            migrationBuilder.DeleteData(
                table: "ApplicationUsers",
                keyColumn: "UserId",
                keyValue: new Guid("b6b7e2e0-1c43-4f89-8d1b-4d7d7b4a0003"));

            migrationBuilder.DeleteData(
                table: "ApplicationUsers",
                keyColumn: "UserId",
                keyValue: new Guid("b6b7e2e0-1c43-4f89-8d1b-4d7d7b4a0004"));

            migrationBuilder.DeleteData(
                table: "ApplicationUsers",
                keyColumn: "UserId",
                keyValue: new Guid("b6b7e2e0-1c43-4f89-8d1b-4d7d7b4a0005"));

            migrationBuilder.DeleteData(
                table: "ApplicationUsers",
                keyColumn: "UserId",
                keyValue: new Guid("b6b7e2e0-1c43-4f89-8d1b-4d7d7b4a0006"));

            migrationBuilder.DeleteData(
                table: "ApplicationUsers",
                keyColumn: "UserId",
                keyValue: new Guid("b6b7e2e0-1c43-4f89-8d1b-4d7d7b4a0007"));

            migrationBuilder.DeleteData(
                table: "ApplicationUsers",
                keyColumn: "UserId",
                keyValue: new Guid("b6b7e2e0-1c43-4f89-8d1b-4d7d7b4a0008"));

            migrationBuilder.DeleteData(
                table: "ApplicationUsers",
                keyColumn: "UserId",
                keyValue: new Guid("b6b7e2e0-1c43-4f89-8d1b-4d7d7b4a0009"));

            migrationBuilder.DeleteData(
                table: "ApplicationUsers",
                keyColumn: "UserId",
                keyValue: new Guid("b6b7e2e0-1c43-4f89-8d1b-4d7d7b4a0010"));

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "ApplicationUsers",
                newName: "PasswordHash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "ApplicationUsers",
                newName: "Password");
        }
    }
}
