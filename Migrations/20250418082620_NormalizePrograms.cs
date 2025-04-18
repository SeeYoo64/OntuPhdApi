using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OntuPhdApi.Migrations
{
    /// <inheritdoc />
    public partial class NormalizePrograms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LinkFaculties_Programs_ProgramModelId",
                table: "LinkFaculties");

            migrationBuilder.DropColumn(
                name: "ControlForm",
                table: "ProgramComponents");

            migrationBuilder.AlterColumn<int>(
                name: "ProgramId",
                table: "ProgramComponents",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ComponentType",
                table: "ProgramComponents",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ComponentName",
                table: "ProgramComponents",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "ProgramModelId",
                table: "ProgramComponents",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProgramModelId",
                table: "LinkFaculties",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "ProgramId",
                table: "LinkFaculties",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProgramModelId",
                table: "Jobs",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ControlForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProgramComponentId = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    ProgramComponentId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ControlForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ControlForms_ProgramComponents_ProgramComponentId",
                        column: x => x.ProgramComponentId,
                        principalTable: "ProgramComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ControlForms_ProgramComponents_ProgramComponentId1",
                        column: x => x.ProgramComponentId1,
                        principalTable: "ProgramComponents",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProgramCharacteristics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProgramId = table.Column<int>(type: "integer", nullable: false),
                    Focus = table.Column<string>(type: "text", nullable: true),
                    Features = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramCharacteristics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgramCharacteristics_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "Programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProgramCompetences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProgramId = table.Column<int>(type: "integer", nullable: false),
                    IntegralCompetence = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramCompetences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgramCompetences_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "Programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProgramCharacteristicsId = table.Column<int>(type: "integer", nullable: false),
                    Object = table.Column<string>(type: "text", nullable: true),
                    Aim = table.Column<string>(type: "text", nullable: true),
                    Theory = table.Column<string>(type: "text", nullable: true),
                    Methods = table.Column<string>(type: "text", nullable: true),
                    Instruments = table.Column<string>(type: "text", nullable: true),
                    ProgramCharacteristicsId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Areas_ProgramCharacteristics_ProgramCharacteristicsId",
                        column: x => x.ProgramCharacteristicsId,
                        principalTable: "ProgramCharacteristics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Areas_ProgramCharacteristics_ProgramCharacteristicsId1",
                        column: x => x.ProgramCharacteristicsId1,
                        principalTable: "ProgramCharacteristics",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OverallCompetences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProgramCompetenceId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ProgramCompetenceId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OverallCompetences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OverallCompetences_ProgramCompetences_ProgramCompetenceId",
                        column: x => x.ProgramCompetenceId,
                        principalTable: "ProgramCompetences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OverallCompetences_ProgramCompetences_ProgramCompetenceId1",
                        column: x => x.ProgramCompetenceId1,
                        principalTable: "ProgramCompetences",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SpecialCompetences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProgramCompetenceId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ProgramCompetenceId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialCompetences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialCompetences_ProgramCompetences_ProgramCompetenceId",
                        column: x => x.ProgramCompetenceId,
                        principalTable: "ProgramCompetences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpecialCompetences_ProgramCompetences_ProgramCompetenceId1",
                        column: x => x.ProgramCompetenceId1,
                        principalTable: "ProgramCompetences",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProgramComponents_ProgramModelId",
                table: "ProgramComponents",
                column: "ProgramModelId");

            migrationBuilder.CreateIndex(
                name: "IX_LinkFaculties_ProgramId",
                table: "LinkFaculties",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_ProgramModelId",
                table: "Jobs",
                column: "ProgramModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_ProgramCharacteristicsId",
                table: "Areas",
                column: "ProgramCharacteristicsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Areas_ProgramCharacteristicsId1",
                table: "Areas",
                column: "ProgramCharacteristicsId1",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ControlForms_ProgramComponentId",
                table: "ControlForms",
                column: "ProgramComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_ControlForms_ProgramComponentId1",
                table: "ControlForms",
                column: "ProgramComponentId1");

            migrationBuilder.CreateIndex(
                name: "IX_OverallCompetences_ProgramCompetenceId",
                table: "OverallCompetences",
                column: "ProgramCompetenceId");

            migrationBuilder.CreateIndex(
                name: "IX_OverallCompetences_ProgramCompetenceId1",
                table: "OverallCompetences",
                column: "ProgramCompetenceId1");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramCharacteristics_ProgramId",
                table: "ProgramCharacteristics",
                column: "ProgramId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProgramCompetences_ProgramId",
                table: "ProgramCompetences",
                column: "ProgramId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpecialCompetences_ProgramCompetenceId",
                table: "SpecialCompetences",
                column: "ProgramCompetenceId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialCompetences_ProgramCompetenceId1",
                table: "SpecialCompetences",
                column: "ProgramCompetenceId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Programs_ProgramModelId",
                table: "Jobs",
                column: "ProgramModelId",
                principalTable: "Programs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LinkFaculties_Programs_ProgramId",
                table: "LinkFaculties",
                column: "ProgramId",
                principalTable: "Programs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LinkFaculties_Programs_ProgramModelId",
                table: "LinkFaculties",
                column: "ProgramModelId",
                principalTable: "Programs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProgramComponents_Programs_ProgramModelId",
                table: "ProgramComponents",
                column: "ProgramModelId",
                principalTable: "Programs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Programs_ProgramModelId",
                table: "Jobs");

            migrationBuilder.DropForeignKey(
                name: "FK_LinkFaculties_Programs_ProgramId",
                table: "LinkFaculties");

            migrationBuilder.DropForeignKey(
                name: "FK_LinkFaculties_Programs_ProgramModelId",
                table: "LinkFaculties");

            migrationBuilder.DropForeignKey(
                name: "FK_ProgramComponents_Programs_ProgramModelId",
                table: "ProgramComponents");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "ControlForms");

            migrationBuilder.DropTable(
                name: "OverallCompetences");

            migrationBuilder.DropTable(
                name: "SpecialCompetences");

            migrationBuilder.DropTable(
                name: "ProgramCharacteristics");

            migrationBuilder.DropTable(
                name: "ProgramCompetences");

            migrationBuilder.DropIndex(
                name: "IX_ProgramComponents_ProgramModelId",
                table: "ProgramComponents");

            migrationBuilder.DropIndex(
                name: "IX_LinkFaculties_ProgramId",
                table: "LinkFaculties");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_ProgramModelId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "ProgramModelId",
                table: "ProgramComponents");

            migrationBuilder.DropColumn(
                name: "ProgramId",
                table: "LinkFaculties");

            migrationBuilder.DropColumn(
                name: "ProgramModelId",
                table: "Jobs");

            migrationBuilder.AlterColumn<int>(
                name: "ProgramId",
                table: "ProgramComponents",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "ComponentType",
                table: "ProgramComponents",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ComponentName",
                table: "ProgramComponents",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<List<string>>(
                name: "ControlForm",
                table: "ProgramComponents",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ProgramModelId",
                table: "LinkFaculties",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LinkFaculties_Programs_ProgramModelId",
                table: "LinkFaculties",
                column: "ProgramModelId",
                principalTable: "Programs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
