using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using OntuPhdApi.Models.Programs;

#nullable disable

namespace OntuPhdApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Programdocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: true),
                    ContentType = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programdocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Program",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Degree = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Name_Code = table.Column<string>(type: "text", nullable: true),
                    Field_Of_Study = table.Column<FieldOfStudy>(type: "jsonb", nullable: false),
                    Speciality = table.Column<Speciality>(type: "jsonb", nullable: false),
                    Form = table.Column<List<string>>(type: "jsonb", nullable: false),
                    Objects = table.Column<string>(type: "text", nullable: true),
                    Directions = table.Column<List<string>>(type: "jsonb", nullable: true),
                    Descriptions = table.Column<string>(type: "text", nullable: true),
                    Purpose = table.Column<string>(type: "text", nullable: true),
                    Years = table.Column<int>(type: "integer", nullable: true),
                    Credits = table.Column<int>(type: "integer", nullable: true),
                    Program_Characteristics = table.Column<ProgramCharacteristics>(type: "jsonb", nullable: true),
                    Program_Competence = table.Column<ProgramCompetence>(type: "jsonb", nullable: true),
                    Results = table.Column<List<string>>(type: "jsonb", nullable: true),
                    Link_Faculty = table.Column<string>(type: "jsonb", nullable: true),
                    ProgramDocumentId = table.Column<int>(type: "integer", nullable: true),
                    Accredited = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Program", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Program_Programdocuments_ProgramDocumentId",
                        column: x => x.ProgramDocumentId,
                        principalTable: "Programdocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Job",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    ProgramModelId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Job", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Job_Program_ProgramModelId",
                        column: x => x.ProgramModelId,
                        principalTable: "Program",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProgramComponent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProgramId = table.Column<int>(type: "integer", nullable: true),
                    ComponentType = table.Column<string>(type: "text", nullable: true),
                    ComponentName = table.Column<string>(type: "text", nullable: true),
                    ComponentCredits = table.Column<int>(type: "integer", nullable: true),
                    ComponentHours = table.Column<int>(type: "integer", nullable: true),
                    ControlForm = table.Column<List<string>>(type: "text[]", nullable: true),
                    ProgramModelId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramComponent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgramComponent_Program_ProgramModelId",
                        column: x => x.ProgramModelId,
                        principalTable: "Program",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Job_ProgramModelId",
                table: "Job",
                column: "ProgramModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Program_ProgramDocumentId",
                table: "Program",
                column: "ProgramDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramComponent_ProgramModelId",
                table: "ProgramComponent",
                column: "ProgramModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Job");

            migrationBuilder.DropTable(
                name: "ProgramComponent");

            migrationBuilder.DropTable(
                name: "Program");

            migrationBuilder.DropTable(
                name: "Programdocuments");
        }
    }
}
