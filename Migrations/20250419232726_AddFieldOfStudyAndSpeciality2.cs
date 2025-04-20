using Microsoft.EntityFrameworkCore.Migrations;
using OntuPhdApi.Models.Programs.Components;

#nullable disable

namespace OntuPhdApi.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldOfStudyAndSpeciality2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Speciality",
                table: "Programs");

            migrationBuilder.AlterColumn<int>(
                name: "SpecialityId",
                table: "Programs",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "FieldOfStudyId",
                table: "Programs",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Programs_SpecialityId",
                table: "Programs",
                column: "SpecialityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Programs_Specialities_SpecialityId",
                table: "Programs",
                column: "SpecialityId",
                principalTable: "Specialities",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Programs_Specialities_SpecialityId",
                table: "Programs");

            migrationBuilder.DropIndex(
                name: "IX_Programs_SpecialityId",
                table: "Programs");

            migrationBuilder.AlterColumn<int>(
                name: "SpecialityId",
                table: "Programs",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "FieldOfStudyId",
                table: "Programs",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<Speciality>(
                name: "Speciality",
                table: "Programs",
                type: "jsonb",
                nullable: true);
        }
    }
}
