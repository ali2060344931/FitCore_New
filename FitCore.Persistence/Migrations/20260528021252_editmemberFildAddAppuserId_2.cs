using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitCore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class editmemberFildAddAppuserId_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddForeignKey(
                name: "FK_Members_AspNetUsers_AppUserId",
                table: "Members",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_AspNetUsers_AppUserId",
                table: "Members");

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
    }
}
