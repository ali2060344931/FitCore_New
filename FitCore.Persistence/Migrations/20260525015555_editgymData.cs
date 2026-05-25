using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitCore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class editgymData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gyms_Provinces_ProvincesId",
                table: "Gyms");

            migrationBuilder.DropIndex(
                name: "IX_Gyms_ProvincesId",
                table: "Gyms");

            migrationBuilder.DropColumn(
                name: "ProvincesId",
                table: "Gyms");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProvincesId",
                table: "Gyms",
                type: "int",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gyms_ProvincesId",
                table: "Gyms",
                column: "ProvincesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gyms_Provinces_ProvincesId",
                table: "Gyms",
                column: "ProvincesId",
                principalTable: "Provinces",
                principalColumn: "Id");
        }
    }
}
