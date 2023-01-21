using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TorKartingowyCoreMVC.Migrations
{
    public partial class AddDostepneGodzinyToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DostepneGodziny",
                columns: table => new
                {
                    TorData = table.Column<string>(type: "nvarchar(13)", nullable: false),
                    G08 = table.Column<int>(type: "int", nullable: false),
                    G09 = table.Column<int>(type: "int", nullable: false),
                    G10 = table.Column<int>(type: "int", nullable: false),
                    G11 = table.Column<int>(type: "int", nullable: false),
                    G12 = table.Column<int>(type: "int", nullable: false),
                    G13 = table.Column<int>(type: "int", nullable: false),
                    G14 = table.Column<int>(type: "int", nullable: false),
                    G15 = table.Column<int>(type: "int", nullable: false),
                    G16 = table.Column<int>(type: "int", nullable: false),
                    G17 = table.Column<int>(type: "int", nullable: false),
                    G18 = table.Column<int>(type: "int", nullable: false),
                    G19 = table.Column<int>(type: "int", nullable: false),
                    G20 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DostepneGodziny", x => x.TorData);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DostepneGodziny");
        }
    }
}
