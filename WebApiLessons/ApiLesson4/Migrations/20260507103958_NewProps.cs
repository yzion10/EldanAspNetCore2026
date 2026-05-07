using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiLesson4.Migrations
{
    /// <inheritdoc />
    public partial class NewProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "LandMarks",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Population",
                table: "Cities",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "LandMarks");

            migrationBuilder.DropColumn(
                name: "Population",
                table: "Cities");
        }
    }
}
