using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitCore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class editGym : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gyms_Cities_CitiesId",
                table: "Gyms");

            migrationBuilder.AlterColumn<int>(
                name: "CitiesId",
                table: "Gyms",
                type: "int",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 100);

            migrationBuilder.AddForeignKey(
                name: "FK_Gyms_Cities_CitiesId",
                table: "Gyms",
                column: "CitiesId",
                principalTable: "Cities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gyms_Cities_CitiesId",
                table: "Gyms");

            migrationBuilder.AlterColumn<int>(
                name: "CitiesId",
                table: "Gyms",
                type: "int",
                maxLength: 100,
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Gyms_Cities_CitiesId",
                table: "Gyms",
                column: "CitiesId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
