using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace OntuPhdApi.Migrations
{
    /// <inheritdoc />
    public partial class AddInstituteToProgram : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InstituteId",
                table: "Programs",
                type: "integer",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_Programs_InstituteId",
                table: "Programs",
                column: "InstituteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Programs_Institutes_InstituteId",
                table: "Programs",
                column: "InstituteId",
                principalTable: "Institutes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Programs_Institutes_InstituteId",
                table: "Programs");

            migrationBuilder.DropTable(
                name: "Institutes");

            migrationBuilder.DropIndex(
                name: "IX_Programs_InstituteId",
                table: "Programs");

            migrationBuilder.DropColumn(
                name: "InstituteId",
                table: "Programs");
        }
    }
}
