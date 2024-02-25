using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExampleApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexToBadge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Badge",
                table: "Students",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Students_Badge",
                table: "Students",
                column: "Badge",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Students_Badge",
                table: "Students");

            migrationBuilder.AlterColumn<string>(
                name: "Badge",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
