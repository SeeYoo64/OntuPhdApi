using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using OntuPhdApi.Models.Programs;
using OntuPhdApi.Models.Programs.Components;

#nullable disable

namespace OntuPhdApi.Migrations
{
    /// <inheritdoc />
    public partial class NullableFieldsInPrograms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Speciality>(
                name: "Speciality",
                table: "Programs",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(Speciality),
                oldType: "jsonb");

            migrationBuilder.AlterColumn<List<string>>(
                name: "Results",
                table: "Programs",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(List<string>),
                oldType: "jsonb");

            migrationBuilder.AlterColumn<string>(
                name: "Purpose",
                table: "Programs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Objects",
                table: "Programs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "NameCode",
                table: "Programs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<List<string>>(
                name: "Form",
                table: "Programs",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(List<string>),
                oldType: "jsonb");

            migrationBuilder.AlterColumn<FieldOfStudy>(
                name: "FieldOfStudy",
                table: "Programs",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(FieldOfStudy),
                oldType: "jsonb");

            migrationBuilder.AlterColumn<List<string>>(
                name: "Directions",
                table: "Programs",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(List<string>),
                oldType: "jsonb");

            migrationBuilder.AlterColumn<string>(
                name: "Descriptions",
                table: "Programs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Speciality>(
                name: "Speciality",
                table: "Programs",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(Speciality),
                oldType: "jsonb",
                oldNullable: true);

            migrationBuilder.AlterColumn<List<string>>(
                name: "Results",
                table: "Programs",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(List<string>),
                oldType: "jsonb",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Purpose",
                table: "Programs",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Objects",
                table: "Programs",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameCode",
                table: "Programs",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<List<string>>(
                name: "Form",
                table: "Programs",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(List<string>),
                oldType: "jsonb",
                oldNullable: true);

            migrationBuilder.AlterColumn<FieldOfStudy>(
                name: "FieldOfStudy",
                table: "Programs",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(FieldOfStudy),
                oldType: "jsonb",
                oldNullable: true);

            migrationBuilder.AlterColumn<List<string>>(
                name: "Directions",
                table: "Programs",
                type: "jsonb",
                nullable: false,
                oldClrType: typeof(List<string>),
                oldType: "jsonb",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Descriptions",
                table: "Programs",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
