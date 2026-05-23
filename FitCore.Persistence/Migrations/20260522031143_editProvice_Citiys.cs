using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitCore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class editProvice_Citiys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ciltys");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Gyms");

            migrationBuilder.AddColumn<int>(
                name: "CitiesId",
                table: "Gyms",
                type: "int",
                maxLength: 100,
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProvincesId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_Provinces_ProvincesId",
                        column: x => x.ProvincesId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gyms_CitiesId",
                table: "Gyms",
                column: "CitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_ProvincesId",
                table: "Cities",
                column: "ProvincesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gyms_Cities_CitiesId",
                table: "Gyms",
                column: "CitiesId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gyms_Cities_CitiesId",
                table: "Gyms");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Gyms_CitiesId",
                table: "Gyms");

            migrationBuilder.DropColumn(
                name: "CitiesId",
                table: "Gyms");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Gyms",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Ciltys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProvincesId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ciltys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ciltys_Provinces_ProvincesId",
                        column: x => x.ProvincesId,
                        principalTable: "Provinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ciltys_ProvincesId",
                table: "Ciltys",
                column: "ProvincesId");
        }
    }
}
