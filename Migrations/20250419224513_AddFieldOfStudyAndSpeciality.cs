using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using OntuPhdApi.Models.Programs.Components;

#nullable disable

namespace OntuPhdApi.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldOfStudyAndSpeciality : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FieldOfStudy",
                table: "Programs");

            migrationBuilder.AddColumn<int>(
                name: "FieldOfStudyId",
                table: "Programs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpecialityId",
                table: "Programs",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FieldOfStudies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Degree = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FieldOfStudies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specialities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    FieldCode = table.Column<string>(type: "text", nullable: true),
                    FieldOfStudyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Specialities_FieldOfStudies_FieldOfStudyId",
                        column: x => x.FieldOfStudyId,
                        principalTable: "FieldOfStudies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Programs_FieldOfStudyId",
                table: "Programs",
                column: "FieldOfStudyId");

            migrationBuilder.CreateIndex(
                name: "IX_Specialities_FieldOfStudyId",
                table: "Specialities",
                column: "FieldOfStudyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Programs_FieldOfStudies_FieldOfStudyId",
                table: "Programs",
                column: "FieldOfStudyId",
                principalTable: "FieldOfStudies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Programs_FieldOfStudies_FieldOfStudyId",
                table: "Programs");

            migrationBuilder.DropTable(
                name: "Specialities");

            migrationBuilder.DropTable(
                name: "FieldOfStudies");

            migrationBuilder.DropIndex(
                name: "IX_Programs_FieldOfStudyId",
                table: "Programs");

            migrationBuilder.DropColumn(
                name: "FieldOfStudyId",
                table: "Programs");

            migrationBuilder.DropColumn(
                name: "SpecialityId",
                table: "Programs");

            migrationBuilder.AddColumn<FieldOfStudy>(
                name: "FieldOfStudy",
                table: "Programs",
                type: "jsonb",
                nullable: true);
        }
    }
}
