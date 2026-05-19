using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitCore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UserOtpCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Gyms_GymId",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "Setings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Gyms");

            migrationBuilder.RenameColumn(
                name: "Mobile",
                table: "Gyms",
                newName: "SubDomain");

            migrationBuilder.AlterColumn<long>(
                name: "GymId",
                table: "AspNetUsers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "UserOtpCodes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpireTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOtpCodes", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Gyms_GymId",
                table: "AspNetUsers",
                column: "GymId",
                principalTable: "Gyms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Gyms_GymId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "UserOtpCodes");

            migrationBuilder.RenameColumn(
                name: "SubDomain",
                table: "Gyms",
                newName: "Mobile");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Gyms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "GymId",
                table: "AspNetUsers",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.InsertData(
                table: "Setings",
                columns: new[] { "Id", "BoolFilde", "Code", "Email", "Logo", "NumberFilde", "Phone", "TextFilde" },
                values: new object[] { 1, false, "01", "Ali@Gmail.com", null, 0, "09111161996", "نرم افزار فیتکو" });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Gyms_GymId",
                table: "AspNetUsers",
                column: "GymId",
                principalTable: "Gyms",
                principalColumn: "Id");
        }
    }
}
