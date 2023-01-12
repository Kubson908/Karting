using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TorKartingowyCoreMVC.Migrations
{
    public partial class AddTorToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TorId",
                table: "Rezerwacje",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Tory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rodzaj = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pojemnosc = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rezerwacje_TorId",
                table: "Rezerwacje",
                column: "TorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rezerwacje_Tory_TorId",
                table: "Rezerwacje",
                column: "TorId",
                principalTable: "Tory",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rezerwacje_Tory_TorId",
                table: "Rezerwacje");

            migrationBuilder.DropTable(
                name: "Tory");

            migrationBuilder.DropIndex(
                name: "IX_Rezerwacje_TorId",
                table: "Rezerwacje");

            migrationBuilder.DropColumn(
                name: "TorId",
                table: "Rezerwacje");
        }
    }
}
