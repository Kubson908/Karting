using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TorKartingowyCoreMVC.Migrations
{
    public partial class DropG08FromDostepneGodziny : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "G08",
                table: "DostepneGodziny");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "G08",
                table: "DostepneGodziny",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
