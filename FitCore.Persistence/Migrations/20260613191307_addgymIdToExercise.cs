using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitCore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addgymIdToExercise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "GymId",
                table: "Exercises",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_GymId",
                table: "Exercises",
                column: "GymId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Gyms_GymId",
                table: "Exercises",
                column: "GymId",
                principalTable: "Gyms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Gyms_GymId",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_GymId",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "GymId",
                table: "Exercises");
        }
    }
}
