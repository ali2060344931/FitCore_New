using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitCore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addFoodUnitConversiongym : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "GymId",
                table: "FoodUnitConversions",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FoodUnitConversions_GymId",
                table: "FoodUnitConversions",
                column: "GymId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodUnitConversions_Gyms_GymId",
                table: "FoodUnitConversions",
                column: "GymId",
                principalTable: "Gyms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodUnitConversions_Gyms_GymId",
                table: "FoodUnitConversions");

            migrationBuilder.DropIndex(
                name: "IX_FoodUnitConversions_GymId",
                table: "FoodUnitConversions");

            migrationBuilder.DropColumn(
                name: "GymId",
                table: "FoodUnitConversions");
        }
    }
}
