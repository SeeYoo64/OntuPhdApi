using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using OntuPhdApi.Models.Programs;

#nullable disable

namespace OntuPhdApi.Migrations
{
    /// <inheritdoc />
    public partial class ProgramsUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Programs_ProgramDocuments_ProgramDocumentId",
                table: "Programs");

            migrationBuilder.DropForeignKey(
                name: "FK_Programs_ProgramDocuments_ProgramDocumentId1",
                table: "Programs");

            migrationBuilder.DropTable(
                name: "ProgramDocuments");

            migrationBuilder.DropIndex(
                name: "IX_Programs_ProgramDocumentId",
                table: "Programs");

            migrationBuilder.DropIndex(
                name: "IX_Programs_ProgramDocumentId1",
                table: "Programs");

            migrationBuilder.DropColumn(
                name: "ProgramDocumentId",
                table: "Programs");

            migrationBuilder.DropColumn(
                name: "ProgramDocumentId1",
                table: "Programs");

            migrationBuilder.AddColumn<List<ProgramFiles>>(
                name: "LinksFile",
                table: "Programs",
                type: "jsonb",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LinksFile",
                table: "Programs");

            migrationBuilder.AddColumn<int>(
                name: "ProgramDocumentId",
                table: "Programs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProgramDocumentId1",
                table: "Programs",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProgramDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContentType = table.Column<string>(type: "text", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramDocuments", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Programs_ProgramDocumentId",
                table: "Programs",
                column: "ProgramDocumentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Programs_ProgramDocumentId1",
                table: "Programs",
                column: "ProgramDocumentId1",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Programs_ProgramDocuments_ProgramDocumentId",
                table: "Programs",
                column: "ProgramDocumentId",
                principalTable: "ProgramDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Programs_ProgramDocuments_ProgramDocumentId1",
                table: "Programs",
                column: "ProgramDocumentId1",
                principalTable: "ProgramDocuments",
                principalColumn: "Id");
        }
    }
}
