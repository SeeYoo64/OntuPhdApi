using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using OntuPhdApi.Models.ApplyDocuments;
using OntuPhdApi.Models.Defense;
using OntuPhdApi.Models.Programs;
using OntuPhdApi.Models.Programs.Components;

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
                name: "ApplyDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Requirements = table.Column<List<Requirements>>(type: "jsonb", nullable: false),
                    OriginalsRequired = table.Column<List<Requirements>>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplyDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProgramId = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Link = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Position = table.Column<string>(type: "text", nullable: false),
                    PhotoPath = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Institutes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Institutes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Summary = table.Column<string>(type: "text", nullable: false),
                    MainTag = table.Column<string>(type: "text", nullable: false),
                    OtherTags = table.Column<List<string>>(type: "jsonb", nullable: true),
                    PublicationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ThumbnailPath = table.Column<string>(type: "text", nullable: false),
                    PhotoPaths = table.Column<List<string>>(type: "jsonb", nullable: true),
                    Body = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProgramDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    ContentType = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    EmailVerified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Image = table.Column<string>(type: "text", nullable: true),
                    MustChangePassword = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VerificationTokens",
                columns: table => new
                {
                    Identifier = table.Column<string>(type: "text", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    Expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerificationTokens", x => new { x.Identifier, x.Token });
                });

            migrationBuilder.CreateTable(
                name: "Programs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Degree = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    NameCode = table.Column<string>(type: "text", nullable: false),
                    FieldOfStudy = table.Column<FieldOfStudy>(type: "jsonb", nullable: false),
                    Speciality = table.Column<Speciality>(type: "jsonb", nullable: false),
                    Form = table.Column<List<string>>(type: "jsonb", nullable: false),
                    Objects = table.Column<string>(type: "text", nullable: false),
                    Directions = table.Column<List<string>>(type: "jsonb", nullable: false),
                    Descriptions = table.Column<string>(type: "text", nullable: false),
                    Purpose = table.Column<string>(type: "text", nullable: false),
                    InstituteId = table.Column<int>(type: "integer", nullable: true),
                    Years = table.Column<int>(type: "integer", nullable: true),
                    Credits = table.Column<int>(type: "integer", nullable: true),
                    Results = table.Column<List<string>>(type: "jsonb", nullable: false),
                    ProgramDocumentId = table.Column<int>(type: "integer", nullable: true),
                    Accredited = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Programs_Institutes_InstituteId",
                        column: x => x.InstituteId,
                        principalTable: "Institutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Programs_ProgramDocuments_ProgramDocumentId",
                        column: x => x.ProgramDocumentId,
                        principalTable: "ProgramDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Provider = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ProviderAccountId = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    AccessToken = table.Column<string>(type: "text", nullable: true),
                    ExpiresAt = table.Column<long>(type: "bigint", nullable: true),
                    IdToken = table.Column<string>(type: "text", nullable: true),
                    Scope = table.Column<string>(type: "text", nullable: true),
                    SessionState = table.Column<string>(type: "text", nullable: true),
                    TokenType = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Defenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CandidateNameSurname = table.Column<string>(type: "text", nullable: false),
                    DefenseTitle = table.Column<string>(type: "text", nullable: false),
                    ScienceTeachers = table.Column<List<string>>(type: "jsonb", nullable: true),
                    DefenseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: true),
                    Message = table.Column<string>(type: "text", nullable: true),
                    Placeholder = table.Column<string>(type: "text", nullable: true),
                    Members = table.Column<List<CompositionOfRada>>(type: "jsonb", nullable: true),
                    Files = table.Column<List<DefenseFile>>(type: "jsonb", nullable: true),
                    PublicationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProgramId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Defenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Defenses_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "Programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    ProgramId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jobs_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "Programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LinkFaculties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Link = table.Column<string>(type: "text", nullable: false),
                    ProgramId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkFaculties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LinkFaculties_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "Programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "ProgramComponents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProgramId = table.Column<int>(type: "integer", nullable: false),
                    ComponentType = table.Column<string>(type: "text", nullable: true),
                    ComponentName = table.Column<string>(type: "text", nullable: true),
                    ComponentCredits = table.Column<int>(type: "integer", nullable: true),
                    ComponentHours = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgramComponents_Programs_ProgramId",
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
                    Instruments = table.Column<string>(type: "text", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "OverallCompetences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProgramCompetenceId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "SpecialCompetences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProgramCompetenceId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "ControlForms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "text", nullable: false),
                    ProgramComponentId = table.Column<int>(type: "integer", nullable: false)
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
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UserId",
                table: "Accounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Areas_ProgramCharacteristicsId",
                table: "Areas",
                column: "ProgramCharacteristicsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ControlForms_ProgramComponentId",
                table: "ControlForms",
                column: "ProgramComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_Defenses_ProgramId",
                table: "Defenses",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_ProgramId",
                table: "Jobs",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_LinkFaculties_ProgramId",
                table: "LinkFaculties",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_OverallCompetences_ProgramCompetenceId",
                table: "OverallCompetences",
                column: "ProgramCompetenceId");

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
                name: "IX_ProgramComponents_ProgramId",
                table: "ProgramComponents",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_Programs_InstituteId",
                table: "Programs",
                column: "InstituteId");

            migrationBuilder.CreateIndex(
                name: "IX_Programs_ProgramDocumentId",
                table: "Programs",
                column: "ProgramDocumentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpecialCompetences_ProgramCompetenceId",
                table: "SpecialCompetences",
                column: "ProgramCompetenceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "ApplyDocuments");

            migrationBuilder.DropTable(
                name: "Areas");

            migrationBuilder.DropTable(
                name: "ControlForms");

            migrationBuilder.DropTable(
                name: "Defenses");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "LinkFaculties");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "OverallCompetences");

            migrationBuilder.DropTable(
                name: "SpecialCompetences");

            migrationBuilder.DropTable(
                name: "VerificationTokens");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ProgramCharacteristics");

            migrationBuilder.DropTable(
                name: "ProgramComponents");

            migrationBuilder.DropTable(
                name: "ProgramCompetences");

            migrationBuilder.DropTable(
                name: "Programs");

            migrationBuilder.DropTable(
                name: "Institutes");

            migrationBuilder.DropTable(
                name: "ProgramDocuments");
        }
    }
}
