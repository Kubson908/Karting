using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TorKartingowyCoreMVC.Migrations
{
    public partial class UpdateRezerwacjaAddGokarty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Gokarty",
                table: "Rezerwacje",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gokarty",
                table: "Rezerwacje");
        }
    }
}
