using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OntuPhdApi.Migrations
{
    /// <inheritdoc />
    public partial class Defenses2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Defenses_ProgramDefense_program_id",
                table: "Defenses");

            migrationBuilder.RenameColumn(
                name: "placeholder",
                table: "Defenses",
                newName: "Placeholder");

            migrationBuilder.RenameColumn(
                name: "message",
                table: "Defenses",
                newName: "Message");

            migrationBuilder.RenameColumn(
                name: "members",
                table: "Defenses",
                newName: "Members");

            migrationBuilder.RenameColumn(
                name: "files",
                table: "Defenses",
                newName: "Files");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "Defenses",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "science_teachers",
                table: "Defenses",
                newName: "ScienceTeachers");

            migrationBuilder.RenameColumn(
                name: "program_id",
                table: "Defenses",
                newName: "ProgramId");

            migrationBuilder.RenameColumn(
                name: "name_surname",
                table: "Defenses",
                newName: "CandidateNameSurname");

            migrationBuilder.RenameColumn(
                name: "defense_name",
                table: "Defenses",
                newName: "DefenseTitle");

            migrationBuilder.RenameColumn(
                name: "date_of_publication",
                table: "Defenses",
                newName: "PublicationDate");

            migrationBuilder.RenameColumn(
                name: "date_of_defense",
                table: "Defenses",
                newName: "DefenseDate");

            migrationBuilder.RenameIndex(
                name: "IX_Defenses_program_id",
                table: "Defenses",
                newName: "IX_Defenses_ProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_Defenses_ProgramDefense_ProgramId",
                table: "Defenses",
                column: "ProgramId",
                principalTable: "ProgramDefense",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Defenses_ProgramDefense_ProgramId",
                table: "Defenses");

            migrationBuilder.RenameColumn(
                name: "Placeholder",
                table: "Defenses",
                newName: "placeholder");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "Defenses",
                newName: "message");

            migrationBuilder.RenameColumn(
                name: "Members",
                table: "Defenses",
                newName: "members");

            migrationBuilder.RenameColumn(
                name: "Files",
                table: "Defenses",
                newName: "files");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Defenses",
                newName: "address");

            migrationBuilder.RenameColumn(
                name: "ScienceTeachers",
                table: "Defenses",
                newName: "science_teachers");

            migrationBuilder.RenameColumn(
                name: "PublicationDate",
                table: "Defenses",
                newName: "date_of_publication");

            migrationBuilder.RenameColumn(
                name: "ProgramId",
                table: "Defenses",
                newName: "program_id");

            migrationBuilder.RenameColumn(
                name: "DefenseTitle",
                table: "Defenses",
                newName: "defense_name");

            migrationBuilder.RenameColumn(
                name: "DefenseDate",
                table: "Defenses",
                newName: "date_of_defense");

            migrationBuilder.RenameColumn(
                name: "CandidateNameSurname",
                table: "Defenses",
                newName: "name_surname");

            migrationBuilder.RenameIndex(
                name: "IX_Defenses_ProgramId",
                table: "Defenses",
                newName: "IX_Defenses_program_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Defenses_ProgramDefense_program_id",
                table: "Defenses",
                column: "program_id",
                principalTable: "ProgramDefense",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
