using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OntuPhdApi.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Programs_Institutes_InstituteId",
                table: "Programs");

            migrationBuilder.AlterColumn<int>(
                name: "InstituteId",
                table: "Programs",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Programs_Institutes_InstituteId",
                table: "Programs",
                column: "InstituteId",
                principalTable: "Institutes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Programs_Institutes_InstituteId",
                table: "Programs");

            migrationBuilder.AlterColumn<int>(
                name: "InstituteId",
                table: "Programs",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Programs_Institutes_InstituteId",
                table: "Programs",
                column: "InstituteId",
                principalTable: "Institutes",
                principalColumn: "Id");
        }
    }
}
