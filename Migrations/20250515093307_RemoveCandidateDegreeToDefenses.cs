using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OntuPhdApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCandidateDegreeToDefenses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CandidateDegree",
                table: "Defenses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CandidateDegree",
                table: "Defenses",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
