using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExampleApp.Api.Migrations.Shared
{
    /// <inheritdoc />
    public partial class AddingBadgeToStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Badge",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Badge",
                table: "Students");
        }
    }
}
