using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiLesson5.Migrations
{
    /// <inheritdoc />
    public partial class InitialDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Population = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LandMarks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    CityId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandMarks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LandMarks_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Description", "Name", "Population" },
                values: new object[,]
                {
                    { 1, "The city that never sleeps", "New York", 8000000 },
                    { 2, "The city of love", "Paris", 2000000 },
                    { 3, "The city of the rising sun", "Tokyo", 9000000 }
                });

            migrationBuilder.InsertData(
                table: "LandMarks",
                columns: new[] { "Id", "CityId", "Description", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, 1, "A symbol of freedom", false, "Statue of Liberty" },
                    { 2, 2, "A global cultural icon of France", false, "Eiffel Tower" },
                    { 3, 3, "A communications and observation tower", false, "Tokyo Tower" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LandMarks_CityId",
                table: "LandMarks",
                column: "CityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LandMarks");

            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
