using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TorKartingowyCoreMVC.Migrations
{
    public partial class AddPlatnoscToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlatnoscNumer",
                table: "Rezerwacje",
                type: "int",
                nullable: false);

            migrationBuilder.CreateTable(
               name: "Platnosci",
               columns: table => new
               {
                   Numer = table.Column<int>(type: "int", nullable: false)
                       .Annotation("SqlServer:Identity", "1, 1"),
                   Sposob = table.Column<string>(type: "nvarchar(max)", nullable: true),
                   Kwota = table.Column<double>(type: "float", nullable: false),
                   Status = table.Column<bool>(type: "bit", nullable: false),
                   RodzajDokumentu = table.Column<string>(type: "nvarchar(max)", nullable: true),
               });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Platnosci",
                table: "Platnosci",
                column: "Numer");

            migrationBuilder.AddForeignKey(
                name: "FK_Rezerwacje_Platnosci_PlatnoscNumer",
                table: "Rezerwacje",
                column: "PlatnoscNumer",
                principalTable: "Platnosci",
                principalColumn: "Numer",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rezerwacje_Platnosci_PlatnoscNumer",
                table: "Rezerwacje");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Platnosci",
                table: "Platnosci");

            migrationBuilder.DropTable(
                name: "Platnosci");

            migrationBuilder.DropIndex(
               name: "IX_Rezerwacje_PlatnoscNumer",
               table: "Rezerwacje");

            migrationBuilder.DropColumn(
                name: "PlatnoscNumer",
                table: "Rezerwacje");
        }
    }
}
