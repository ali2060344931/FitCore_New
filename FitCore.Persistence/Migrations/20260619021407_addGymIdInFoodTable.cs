using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitCore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addGymIdInFoodTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "GymId",
                table: "Foods",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Foods_GymId",
                table: "Foods",
                column: "GymId");

            migrationBuilder.AddForeignKey(
                name: "FK_Foods_Gyms_GymId",
                table: "Foods",
                column: "GymId",
                principalTable: "Gyms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Foods_Gyms_GymId",
                table: "Foods");

            migrationBuilder.DropIndex(
                name: "IX_Foods_GymId",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "GymId",
                table: "Foods");
        }
    }
}
