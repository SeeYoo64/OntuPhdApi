using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OntuPhdApi.Migrations
{
    /// <inheritdoc />
    public partial class News : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Programs_ProgramModelId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_ProgramModelId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "ProgramModelId",
                table: "Jobs");

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

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_ProgramId",
                table: "Jobs",
                column: "ProgramId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Programs_ProgramId",
                table: "Jobs",
                column: "ProgramId",
                principalTable: "Programs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Programs_ProgramId",
                table: "Jobs");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_ProgramId",
                table: "Jobs");

            migrationBuilder.AddColumn<int>(
                name: "ProgramModelId",
                table: "Jobs",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_ProgramModelId",
                table: "Jobs",
                column: "ProgramModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Programs_ProgramModelId",
                table: "Jobs",
                column: "ProgramModelId",
                principalTable: "Programs",
                principalColumn: "Id");
        }
    }
}
