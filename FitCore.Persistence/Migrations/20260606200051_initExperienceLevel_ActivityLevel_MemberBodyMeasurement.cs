using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitCore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class initExperienceLevel_ActivityLevel_MemberBodyMeasurement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Members");

            migrationBuilder.AddColumn<int>(
                name: "ActivityLevelId",
                table: "Members",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExperienceLevelId",
                table: "Members",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FoodAllergies",
                table: "Members",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Injuries",
                table: "Members",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedicalConditions",
                table: "Members",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "activityLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activityLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "experiences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_experiences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "memberBodyMeasurements",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<long>(type: "bigint", nullable: false),
                    RecordDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Height = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BodyFatPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Waist = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Hip = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Chest = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InsertTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    RemoveTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_memberBodyMeasurements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_memberBodyMeasurements_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Members_ActivityLevelId",
                table: "Members",
                column: "ActivityLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_ExperienceLevelId",
                table: "Members",
                column: "ExperienceLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_memberBodyMeasurements_MemberId",
                table: "memberBodyMeasurements",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_activityLevels_ActivityLevelId",
                table: "Members",
                column: "ActivityLevelId",
                principalTable: "activityLevels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_experiences_ExperienceLevelId",
                table: "Members",
                column: "ExperienceLevelId",
                principalTable: "experiences",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_activityLevels_ActivityLevelId",
                table: "Members");

            migrationBuilder.DropForeignKey(
                name: "FK_Members_experiences_ExperienceLevelId",
                table: "Members");

            migrationBuilder.DropTable(
                name: "activityLevels");

            migrationBuilder.DropTable(
                name: "experiences");

            migrationBuilder.DropTable(
                name: "memberBodyMeasurements");

            migrationBuilder.DropIndex(
                name: "IX_Members_ActivityLevelId",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Members_ExperienceLevelId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "ActivityLevelId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "ExperienceLevelId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "FoodAllergies",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Injuries",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "MedicalConditions",
                table: "Members");

            migrationBuilder.AddColumn<decimal>(
                name: "Height",
                table: "Members",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Weight",
                table: "Members",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
