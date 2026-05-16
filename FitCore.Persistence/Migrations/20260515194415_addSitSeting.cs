using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitCore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addSitSeting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Setings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "Setings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Setings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "InsertTime",
                value: new DateTime(2026, 5, 15, 23, 14, 12, 77, DateTimeKind.Local).AddTicks(835));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "InsertTime",
                value: new DateTime(2026, 5, 15, 23, 14, 12, 77, DateTimeKind.Local).AddTicks(898));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "InsertTime",
                value: new DateTime(2026, 5, 15, 23, 14, 12, 77, DateTimeKind.Local).AddTicks(915));

            migrationBuilder.UpdateData(
                table: "Setings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Email", "Logo", "Phone" },
                values: new object[] { "Ali@Gmail.com", null, "09111161996" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Setings");

            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Setings");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Setings");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "InsertTime",
                value: new DateTime(2026, 5, 15, 20, 30, 23, 359, DateTimeKind.Local).AddTicks(8943));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "InsertTime",
                value: new DateTime(2026, 5, 15, 20, 30, 23, 359, DateTimeKind.Local).AddTicks(8987));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "InsertTime",
                value: new DateTime(2026, 5, 15, 20, 30, 23, 359, DateTimeKind.Local).AddTicks(8998));
        }
    }
}
