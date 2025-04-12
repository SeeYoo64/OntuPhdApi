using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using OntuPhdApi.Models.Defense;

#nullable disable

namespace OntuPhdApi.Migrations
{
    /// <inheritdoc />
    public partial class Defenses3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Defenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name_surname = table.Column<string>(type: "text", nullable: false),
                    defense_name = table.Column<string>(type: "text", nullable: false),
                    science_teachers = table.Column<List<string>>(type: "jsonb", nullable: true),
                    date_of_defense = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    address = table.Column<string>(type: "text", nullable: true),
                    message = table.Column<string>(type: "text", nullable: true),
                    placeholder = table.Column<string>(type: "text", nullable: true),
                    members = table.Column<List<CompositionOfRada>>(type: "jsonb", nullable: true),
                    files = table.Column<List<DefenseFile>>(type: "jsonb", nullable: true),
                    date_of_publication = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    program_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Defenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Defenses_Programs_program_id",
                        column: x => x.program_id,
                        principalTable: "Programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Defenses_program_id",
                table: "Defenses",
                column: "program_id");

            migrationBuilder.DropForeignKey(
                name: "FK_Defenses_Programs_program_id",
                table: "Defenses");


            migrationBuilder.AddForeignKey(
                name: "FK_Defenses_Programs_ProgramId",
                table: "Defenses",
                column: "ProgramId",
                principalTable: "Programs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
    name: "Defenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Defenses_Programs_ProgramId",
                table: "Defenses");

            migrationBuilder.AddForeignKey(
                name: "FK_Defenses_ProgramDefense_ProgramId",
                table: "Defenses",
                column: "ProgramId",
                principalTable: "ProgramDefense",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
