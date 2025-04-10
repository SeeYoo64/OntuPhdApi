using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OntuPhdApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFeaturesTostring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            UPDATE program
            SET ""program_characteristics"" = jsonb_set(
                ""program_characteristics"",
                '{features}',
                to_jsonb(
                    array_to_string(
                        (SELECT array_agg(value::text)
                         FROM jsonb_array_elements(""program_characteristics""->'features')),
                        ', '
                    )
                ),
                true
            )
            WHERE ""program_characteristics""->'features' IS NOT NULL
            AND jsonb_typeof(""program_characteristics""->'features') = 'array';
        ");
            migrationBuilder.DropForeignKey(
                name: "FK_Job_Program_ProgramModelId",
                table: "Job");

            migrationBuilder.DropForeignKey(
                name: "FK_Program_Programdocuments_ProgramDocumentId",
                table: "Program");

            migrationBuilder.DropForeignKey(
                name: "FK_ProgramComponent_Program_ProgramModelId",
                table: "ProgramComponent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Programdocuments",
                table: "Programdocuments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Program",
                table: "Program");

            migrationBuilder.RenameTable(
                name: "Programdocuments",
                newName: "programdocuments");

            migrationBuilder.RenameTable(
                name: "Program",
                newName: "program");

            migrationBuilder.RenameColumn(
                name: "UploadDate",
                table: "programdocuments",
                newName: "uploaddate");

            migrationBuilder.RenameColumn(
                name: "FileSize",
                table: "programdocuments",
                newName: "filesize");

            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "programdocuments",
                newName: "filepath");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "programdocuments",
                newName: "filename");

            migrationBuilder.RenameColumn(
                name: "ContentType",
                table: "programdocuments",
                newName: "contenttype");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "programdocuments",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Years",
                table: "program",
                newName: "years");

            migrationBuilder.RenameColumn(
                name: "Speciality",
                table: "program",
                newName: "speciality");

            migrationBuilder.RenameColumn(
                name: "Results",
                table: "program",
                newName: "results");

            migrationBuilder.RenameColumn(
                name: "Purpose",
                table: "program",
                newName: "purpose");

            migrationBuilder.RenameColumn(
                name: "Program_Competence",
                table: "program",
                newName: "program_competence");

            migrationBuilder.RenameColumn(
                name: "Program_Characteristics",
                table: "program",
                newName: "program_characteristics");

            migrationBuilder.RenameColumn(
                name: "ProgramDocumentId",
                table: "program",
                newName: "programdocumentid");

            migrationBuilder.RenameColumn(
                name: "Objects",
                table: "program",
                newName: "objects");

            migrationBuilder.RenameColumn(
                name: "Name_Code",
                table: "program",
                newName: "name_code");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "program",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Link_Faculty",
                table: "program",
                newName: "link_faculty");

            migrationBuilder.RenameColumn(
                name: "Form",
                table: "program",
                newName: "form");

            migrationBuilder.RenameColumn(
                name: "Field_Of_Study",
                table: "program",
                newName: "field_of_study");

            migrationBuilder.RenameColumn(
                name: "Directions",
                table: "program",
                newName: "directions");

            migrationBuilder.RenameColumn(
                name: "Descriptions",
                table: "program",
                newName: "descriptions");

            migrationBuilder.RenameColumn(
                name: "Degree",
                table: "program",
                newName: "degree");

            migrationBuilder.RenameColumn(
                name: "Credits",
                table: "program",
                newName: "credits");

            migrationBuilder.RenameColumn(
                name: "Accredited",
                table: "program",
                newName: "accredited");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "program",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_Program_ProgramDocumentId",
                table: "program",
                newName: "IX_program_programdocumentid");

            migrationBuilder.AlterColumn<List<string>>(
                name: "ControlForm",
                table: "ProgramComponent",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(List<string>),
                oldType: "text[]",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "link_faculty",
                table: "program",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "jsonb",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_programdocuments",
                table: "programdocuments",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_program",
                table: "program",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Job_program_ProgramModelId",
                table: "Job",
                column: "ProgramModelId",
                principalTable: "program",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_program_programdocuments_programdocumentid",
                table: "program",
                column: "programdocumentid",
                principalTable: "programdocuments",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ProgramComponent_program_ProgramModelId",
                table: "ProgramComponent",
                column: "ProgramModelId",
                principalTable: "program",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            UPDATE program
            SET ""program_characteristics"" = jsonb_set(
                ""program_characteristics"",
                '{features}',
                to_jsonb(string_to_array(""program_characteristics""->>'features', ', ')),
                true
            )
            WHERE ""program_characteristics""->>'features' IS NOT NULL;
        ");
            migrationBuilder.DropForeignKey(
                name: "FK_Job_program_ProgramModelId",
                table: "Job");

            migrationBuilder.DropForeignKey(
                name: "FK_program_programdocuments_programdocumentid",
                table: "program");

            migrationBuilder.DropForeignKey(
                name: "FK_ProgramComponent_program_ProgramModelId",
                table: "ProgramComponent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_programdocuments",
                table: "programdocuments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_program",
                table: "program");

            migrationBuilder.RenameTable(
                name: "programdocuments",
                newName: "Programdocuments");

            migrationBuilder.RenameTable(
                name: "program",
                newName: "Program");

            migrationBuilder.RenameColumn(
                name: "uploaddate",
                table: "Programdocuments",
                newName: "UploadDate");

            migrationBuilder.RenameColumn(
                name: "filesize",
                table: "Programdocuments",
                newName: "FileSize");

            migrationBuilder.RenameColumn(
                name: "filepath",
                table: "Programdocuments",
                newName: "FilePath");

            migrationBuilder.RenameColumn(
                name: "filename",
                table: "Programdocuments",
                newName: "FileName");

            migrationBuilder.RenameColumn(
                name: "contenttype",
                table: "Programdocuments",
                newName: "ContentType");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Programdocuments",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "years",
                table: "Program",
                newName: "Years");

            migrationBuilder.RenameColumn(
                name: "speciality",
                table: "Program",
                newName: "Speciality");

            migrationBuilder.RenameColumn(
                name: "results",
                table: "Program",
                newName: "Results");

            migrationBuilder.RenameColumn(
                name: "purpose",
                table: "Program",
                newName: "Purpose");

            migrationBuilder.RenameColumn(
                name: "programdocumentid",
                table: "Program",
                newName: "ProgramDocumentId");

            migrationBuilder.RenameColumn(
                name: "program_competence",
                table: "Program",
                newName: "Program_Competence");

            migrationBuilder.RenameColumn(
                name: "program_characteristics",
                table: "Program",
                newName: "Program_Characteristics");

            migrationBuilder.RenameColumn(
                name: "objects",
                table: "Program",
                newName: "Objects");

            migrationBuilder.RenameColumn(
                name: "name_code",
                table: "Program",
                newName: "Name_Code");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Program",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "link_faculty",
                table: "Program",
                newName: "Link_Faculty");

            migrationBuilder.RenameColumn(
                name: "form",
                table: "Program",
                newName: "Form");

            migrationBuilder.RenameColumn(
                name: "field_of_study",
                table: "Program",
                newName: "Field_Of_Study");

            migrationBuilder.RenameColumn(
                name: "directions",
                table: "Program",
                newName: "Directions");

            migrationBuilder.RenameColumn(
                name: "descriptions",
                table: "Program",
                newName: "Descriptions");

            migrationBuilder.RenameColumn(
                name: "degree",
                table: "Program",
                newName: "Degree");

            migrationBuilder.RenameColumn(
                name: "credits",
                table: "Program",
                newName: "Credits");

            migrationBuilder.RenameColumn(
                name: "accredited",
                table: "Program",
                newName: "Accredited");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Program",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_program_programdocumentid",
                table: "Program",
                newName: "IX_Program_ProgramDocumentId");

            migrationBuilder.AlterColumn<List<string>>(
                name: "ControlForm",
                table: "ProgramComponent",
                type: "text[]",
                nullable: true,
                oldClrType: typeof(List<string>),
                oldType: "jsonb",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Link_Faculty",
                table: "Program",
                type: "jsonb",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Programdocuments",
                table: "Programdocuments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Program",
                table: "Program",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Job_Program_ProgramModelId",
                table: "Job",
                column: "ProgramModelId",
                principalTable: "Program",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Program_Programdocuments_ProgramDocumentId",
                table: "Program",
                column: "ProgramDocumentId",
                principalTable: "Programdocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ProgramComponent_Program_ProgramModelId",
                table: "ProgramComponent",
                column: "ProgramModelId",
                principalTable: "Program",
                principalColumn: "Id");
        }
    }
}
