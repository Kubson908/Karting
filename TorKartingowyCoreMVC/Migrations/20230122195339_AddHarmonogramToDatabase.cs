using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TorKartingowyCoreMVC.Migrations
{
    public partial class AddHarmonogramToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Harmonogram",
                columns: table => new
                {
                    OdKiedy = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ListaGodzin = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Harmonogram", x => x.OdKiedy);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Harmonogram");
        }
    }
}
