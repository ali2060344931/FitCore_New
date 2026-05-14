using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bugeto_Store.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addTelToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tel",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "InsertTime",
                value: new DateTime(2026, 5, 14, 10, 26, 48, 859, DateTimeKind.Local).AddTicks(2980));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "InsertTime",
                value: new DateTime(2026, 5, 14, 10, 26, 48, 859, DateTimeKind.Local).AddTicks(3016));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "InsertTime",
                value: new DateTime(2026, 5, 14, 10, 26, 48, 859, DateTimeKind.Local).AddTicks(3025));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tel",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1L,
                column: "InsertTime",
                value: new DateTime(2026, 5, 13, 20, 39, 25, 803, DateTimeKind.Local).AddTicks(8866));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2L,
                column: "InsertTime",
                value: new DateTime(2026, 5, 13, 20, 39, 25, 803, DateTimeKind.Local).AddTicks(8908));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3L,
                column: "InsertTime",
                value: new DateTime(2026, 5, 13, 20, 39, 25, 803, DateTimeKind.Local).AddTicks(8921));
        }
    }
}
