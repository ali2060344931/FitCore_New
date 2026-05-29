using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitCore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class initNutritionProgramEntitys_FixCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FoodCategoryTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodCategoryTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GetGoalTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GetGoalTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MealTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NutritionProgramTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NutritionProgramTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NutritionUnitTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NutritionUnitTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NutritionPrograms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GymId = table.Column<long>(type: "bigint", nullable: false),
                    MemberId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProgramTypeId = table.Column<int>(type: "int", nullable: false),
                    GoalTypeId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InsertTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    RemoveTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NutritionPrograms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NutritionPrograms_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NutritionPrograms_GetGoalTypes_GoalTypeId",
                        column: x => x.GoalTypeId,
                        principalTable: "GetGoalTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NutritionPrograms_Gyms_GymId",
                        column: x => x.GymId,
                        principalTable: "Gyms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NutritionPrograms_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NutritionPrograms_NutritionProgramTypes_ProgramTypeId",
                        column: x => x.ProgramTypeId,
                        principalTable: "NutritionProgramTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Foods",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnglishTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategoryTypeId = table.Column<int>(type: "int", nullable: false),
                    CaloriesPerUnit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProteinPerUnit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CarbohydratePerUnit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FatPerUnit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DefaultUnitId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    InsertTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    RemoveTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Foods_FoodCategoryTypes_CategoryTypeId",
                        column: x => x.CategoryTypeId,
                        principalTable: "FoodCategoryTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Foods_NutritionUnitTypes_DefaultUnitId",
                        column: x => x.DefaultUnitId,
                        principalTable: "NutritionUnitTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NutritionProgramDays",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NutritionProgramId = table.Column<long>(type: "bigint", nullable: false),
                    DayNumber = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    RemoveTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NutritionProgramDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NutritionProgramDays_NutritionPrograms_NutritionProgramId",
                        column: x => x.NutritionProgramId,
                        principalTable: "NutritionPrograms",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NutritionMeals",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NutritionProgramDayId = table.Column<long>(type: "bigint", nullable: false),
                    MealTypeId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MealTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    TotalCalories = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalProtein = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCarbohydrate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalFat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InsertTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    RemoveTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NutritionMeals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NutritionMeals_MealTypes_MealTypeId",
                        column: x => x.MealTypeId,
                        principalTable: "MealTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NutritionMeals_NutritionProgramDays_NutritionProgramDayId",
                        column: x => x.NutritionProgramDayId,
                        principalTable: "NutritionProgramDays",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "NutritionMealItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NutritionMealId = table.Column<long>(type: "bigint", nullable: false),
                    FoodId = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitTypeId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Calories = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Protein = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Carbohydrate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Fat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NutritionUnitTypeId = table.Column<int>(type: "int", nullable: true),
                    InsertTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    RemoveTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NutritionMealItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NutritionMealItems_Foods_FoodId",
                        column: x => x.FoodId,
                        principalTable: "Foods",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NutritionMealItems_NutritionMeals_NutritionMealId",
                        column: x => x.NutritionMealId,
                        principalTable: "NutritionMeals",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NutritionMealItems_NutritionUnitTypes_NutritionUnitTypeId",
                        column: x => x.NutritionUnitTypeId,
                        principalTable: "NutritionUnitTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_NutritionMealItems_NutritionUnitTypes_UnitTypeId",
                        column: x => x.UnitTypeId,
                        principalTable: "NutritionUnitTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Foods_CategoryTypeId",
                table: "Foods",
                column: "CategoryTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Foods_DefaultUnitId",
                table: "Foods",
                column: "DefaultUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_NutritionMealItems_FoodId",
                table: "NutritionMealItems",
                column: "FoodId");

            migrationBuilder.CreateIndex(
                name: "IX_NutritionMealItems_NutritionMealId",
                table: "NutritionMealItems",
                column: "NutritionMealId");

            migrationBuilder.CreateIndex(
                name: "IX_NutritionMealItems_NutritionUnitTypeId",
                table: "NutritionMealItems",
                column: "NutritionUnitTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NutritionMealItems_UnitTypeId",
                table: "NutritionMealItems",
                column: "UnitTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NutritionMeals_MealTypeId",
                table: "NutritionMeals",
                column: "MealTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NutritionMeals_NutritionProgramDayId",
                table: "NutritionMeals",
                column: "NutritionProgramDayId");

            migrationBuilder.CreateIndex(
                name: "IX_NutritionProgramDays_NutritionProgramId",
                table: "NutritionProgramDays",
                column: "NutritionProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_NutritionPrograms_CreatedByUserId",
                table: "NutritionPrograms",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NutritionPrograms_GoalTypeId",
                table: "NutritionPrograms",
                column: "GoalTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NutritionPrograms_GymId",
                table: "NutritionPrograms",
                column: "GymId");

            migrationBuilder.CreateIndex(
                name: "IX_NutritionPrograms_MemberId",
                table: "NutritionPrograms",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_NutritionPrograms_ProgramTypeId",
                table: "NutritionPrograms",
                column: "ProgramTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NutritionMealItems");

            migrationBuilder.DropTable(
                name: "Foods");

            migrationBuilder.DropTable(
                name: "NutritionMeals");

            migrationBuilder.DropTable(
                name: "FoodCategoryTypes");

            migrationBuilder.DropTable(
                name: "NutritionUnitTypes");

            migrationBuilder.DropTable(
                name: "MealTypes");

            migrationBuilder.DropTable(
                name: "NutritionProgramDays");

            migrationBuilder.DropTable(
                name: "NutritionPrograms");

            migrationBuilder.DropTable(
                name: "GetGoalTypes");

            migrationBuilder.DropTable(
                name: "NutritionProgramTypes");
        }
    }
}
