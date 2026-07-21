using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitCore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class editGym2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "GymCodeSequence",
                startValue: 100L);

            migrationBuilder.AlterColumn<int>(
                name: "Code",
                table: "Gyms",
                type: "int",
                maxLength: 50,
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR GymCodeSequence",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "GymCodeSequence");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Gyms",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 50,
                oldDefaultValueSql: "NEXT VALUE FOR GymCodeSequence");
        }
    }
}
