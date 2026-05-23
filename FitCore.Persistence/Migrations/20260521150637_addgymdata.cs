using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitCore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addgymdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SubDomain",
                table: "Gyms",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Gyms",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Gyms",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Gyms",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "AllowOnlineRegistration",
                table: "Gyms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "BrandName",
                table: "Gyms",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Gyms",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Gyms",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Gyms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Gyms",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "Gyms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Gyms",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxMembers",
                table: "Gyms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxOtpRequestPerMinute",
                table: "Gyms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MobileNumber",
                table: "Gyms",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OtpExpireSeconds",
                table: "Gyms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Gyms",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Gyms",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Province",
                table: "Gyms",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubscriptionExpireDate",
                table: "Gyms",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "Gyms",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Gyms");

            migrationBuilder.DropColumn(
                name: "AllowOnlineRegistration",
                table: "Gyms");

            migrationBuilder.DropColumn(
                name: "BrandName",
                table: "Gyms");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Gyms");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Gyms");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Gyms");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Gyms");

            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Gyms");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Gyms");

            migrationBuilder.DropColumn(
                name: "MaxMembers",
                table: "Gyms");

            migrationBuilder.DropColumn(
                name: "MaxOtpRequestPerMinute",
                table: "Gyms");

            migrationBuilder.DropColumn(
                name: "MobileNumber",
                table: "Gyms");

            migrationBuilder.DropColumn(
                name: "OtpExpireSeconds",
                table: "Gyms");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Gyms");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Gyms");

            migrationBuilder.DropColumn(
                name: "Province",
                table: "Gyms");

            migrationBuilder.DropColumn(
                name: "SubscriptionExpireDate",
                table: "Gyms");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "Gyms");

            migrationBuilder.AlterColumn<string>(
                name: "SubDomain",
                table: "Gyms",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Gyms",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Gyms",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }
    }
}
