using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitCore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixProvinceAndCityRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Province",
                table: "Gyms");

            migrationBuilder.AlterColumn<string>(
                name: "MobileNumber",
                table: "Gyms",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "MobileNumber",
                table: "Gyms",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<string>(
                name: "Province",
                table: "Gyms",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
