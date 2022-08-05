using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Battleship_API.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Player1Coordinates",
                columns: table => new
                {
                    CoordinateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    X = table.Column<int>(type: "int", nullable: false),
                    Y = table.Column<int>(type: "int", nullable: false),
                    IsShip = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsHit = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player1Coordinates", x => x.CoordinateId);
                });

            migrationBuilder.CreateTable(
                name: "Player2Coordinates",
                columns: table => new
                {
                    CoordinateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    X = table.Column<int>(type: "int", nullable: false),
                    Y = table.Column<int>(type: "int", nullable: false),
                    IsShip = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsHit = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player2Coordinates", x => x.CoordinateId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Player1Coordinates");

            migrationBuilder.DropTable(
                name: "Player2Coordinates");
        }
    }
}
