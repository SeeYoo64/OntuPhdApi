using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using OntuPhdApi.Models.Programs;

#nullable disable

namespace OntuPhdApi.Migrations
{
    /// <inheritdoc />
    public partial class LinksFacultyInPrograms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<List<LinksFaculties>>(
                name: "LinkFaculty",
                table: "Programs",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LinkFaculty",
                table: "Programs",
                type: "text",
                nullable: true,
                oldClrType: typeof(List<LinksFaculties>),
                oldType: "jsonb",
                oldNullable: true);
        }
    }
}
