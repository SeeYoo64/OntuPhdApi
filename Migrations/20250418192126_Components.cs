using Microsoft.EntityFrameworkCore.Migrations;
using OntuPhdApi.Models.Programs.Components;

#nullable disable

namespace OntuPhdApi.Migrations
{
    /// <inheritdoc />
    public partial class Components : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProgramCharacteristics",
                table: "Programs");

            migrationBuilder.DropColumn(
                name: "ProgramCompetence",
                table: "Programs");

            migrationBuilder.AddColumn<int>(
                name: "ProgramCharacteristicsId",
                table: "Programs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProgramCompetenceId",
                table: "Programs",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Programs_ProgramCharacteristicsId",
                table: "Programs",
                column: "ProgramCharacteristicsId");

            migrationBuilder.CreateIndex(
                name: "IX_Programs_ProgramCompetenceId",
                table: "Programs",
                column: "ProgramCompetenceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Programs_ProgramCharacteristics_ProgramCharacteristicsId",
                table: "Programs",
                column: "ProgramCharacteristicsId",
                principalTable: "ProgramCharacteristics",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Programs_ProgramCompetences_ProgramCompetenceId",
                table: "Programs",
                column: "ProgramCompetenceId",
                principalTable: "ProgramCompetences",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Programs_ProgramCharacteristics_ProgramCharacteristicsId",
                table: "Programs");

            migrationBuilder.DropForeignKey(
                name: "FK_Programs_ProgramCompetences_ProgramCompetenceId",
                table: "Programs");

            migrationBuilder.DropIndex(
                name: "IX_Programs_ProgramCharacteristicsId",
                table: "Programs");

            migrationBuilder.DropIndex(
                name: "IX_Programs_ProgramCompetenceId",
                table: "Programs");

            migrationBuilder.DropColumn(
                name: "ProgramCharacteristicsId",
                table: "Programs");

            migrationBuilder.DropColumn(
                name: "ProgramCompetenceId",
                table: "Programs");

            migrationBuilder.AddColumn<ProgramCharacteristics>(
                name: "ProgramCharacteristics",
                table: "Programs",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<ProgramCompetence>(
                name: "ProgramCompetence",
                table: "Programs",
                type: "jsonb",
                nullable: true);
        }
    }
}
