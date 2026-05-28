using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitCore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class editmemberFildAddAppuserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_Gyms_GymId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Mobile",
                table: "Members");

            migrationBuilder.RenameColumn(
                name: "GymId",
                table: "Members",
                newName: "AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Members_GymId",
                table: "Members",
                newName: "IX_Members_AppUserId");

            migrationBuilder.AddColumn<long>(
                name: "GymsId",
                table: "Members",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_GymsId",
                table: "Members",
                column: "GymsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_AspNetUsers_AppUserId",
                table: "Members",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Gyms_GymsId",
                table: "Members",
                column: "GymsId",
                principalTable: "Gyms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_AspNetUsers_AppUserId",
                table: "Members");

            migrationBuilder.DropForeignKey(
                name: "FK_Members_Gyms_GymsId",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Members_GymsId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "GymsId",
                table: "Members");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "Members",
                newName: "GymId");

            migrationBuilder.RenameIndex(
                name: "IX_Members_AppUserId",
                table: "Members",
                newName: "IX_Members_GymId");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Members",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Members",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Mobile",
                table: "Members",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Gyms_GymId",
                table: "Members",
                column: "GymId",
                principalTable: "Gyms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
