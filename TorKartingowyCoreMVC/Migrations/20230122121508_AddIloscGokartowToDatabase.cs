using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TorKartingowyCoreMVC.Migrations
{
    public partial class AddIloscGokartowToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IloscGokartow",
                columns: table => new
                {
                    DataGodzina = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Spalinowe = table.Column<int>(type: "int", nullable: false),
                    Elektryczne = table.Column<int>(type: "int", nullable: false),
                    DlaDzieci = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IloscGokartow", x => x.DataGodzina);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IloscGokartow");
        }
    }
}
