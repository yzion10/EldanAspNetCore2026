using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarCatalogApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Manufacturers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Country = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    FoundedYear = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    YearFrom = table.Column<int>(type: "INTEGER", nullable: true),
                    BodyType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    ManufacturerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarModels_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarSubModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    EngineCode = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    HorsePower = table.Column<int>(type: "INTEGER", nullable: true),
                    CarModelId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarSubModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarSubModels_CarModels_CarModelId",
                        column: x => x.CarModelId,
                        principalTable: "CarModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Manufacturers",
                columns: new[] { "Id", "Country", "FoundedYear", "Name" },
                values: new object[,]
                {
                    { 1, "United States", 1911, "Chevrolet" },
                    { 2, "Japan", 1937, "Toyota" }
                });

            migrationBuilder.InsertData(
                table: "CarModels",
                columns: new[] { "Id", "BodyType", "ManufacturerId", "Name", "YearFrom" },
                values: new object[,]
                {
                    { 1, "Sports Car", 1, "Corvette", 1953 },
                    { 2, "Muscle Car", 1, "Camaro", 1966 },
                    { 3, "Sedan", 2, "Corolla", 1966 }
                });

            migrationBuilder.InsertData(
                table: "CarSubModels",
                columns: new[] { "Id", "CarModelId", "EngineCode", "HorsePower", "Name" },
                values: new object[,]
                {
                    { 1, 1, "6.2L V8", 490, "LT2" },
                    { 2, 1, "5.5L V8", 670, "Z06" },
                    { 3, 2, "6.2L V8", 455, "SS" },
                    { 4, 3, "1.8L Hybrid", 138, "Hybrid" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarModels_ManufacturerId_Name",
                table: "CarModels",
                columns: new[] { "ManufacturerId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarSubModels_CarModelId_Name",
                table: "CarSubModels",
                columns: new[] { "CarModelId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Manufacturers_Name",
                table: "Manufacturers",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarSubModels");

            migrationBuilder.DropTable(
                name: "CarModels");

            migrationBuilder.DropTable(
                name: "Manufacturers");
        }
    }
}
