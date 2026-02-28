using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "Submissions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Submissions",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAtUtc",
                table: "Submissions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Submissions",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "Palettes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Palettes",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAtUtc",
                table: "Palettes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Palettes",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "Challenges",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Challenges",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAtUtc",
                table: "Challenges",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Challenges",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAtUtc",
                table: "AppIdeas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "AppIdeas",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAtUtc",
                table: "AppIdeas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "AppIdeas",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "LastModifiedAtUtc",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Submissions");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "Palettes");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Palettes");

            migrationBuilder.DropColumn(
                name: "LastModifiedAtUtc",
                table: "Palettes");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Palettes");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "Challenges");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Challenges");

            migrationBuilder.DropColumn(
                name: "LastModifiedAtUtc",
                table: "Challenges");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Challenges");

            migrationBuilder.DropColumn(
                name: "CreatedAtUtc",
                table: "AppIdeas");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AppIdeas");

            migrationBuilder.DropColumn(
                name: "LastModifiedAtUtc",
                table: "AppIdeas");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "AppIdeas");
        }
    }
}
