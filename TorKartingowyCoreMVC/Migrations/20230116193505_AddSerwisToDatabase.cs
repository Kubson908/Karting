using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TorKartingowyCoreMVC.Migrations
{
    public partial class AddSerwisToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Serwisy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Usterka = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notatka = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Wykonano = table.Column<bool>(type: "bit", nullable: false),
                    DataUtworzenia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GokartNumer = table.Column<int>(type: "int", nullable: false),
                    InstruktorId = table.Column<int>(type: "int", nullable: false),
                    IntruktorId = table.Column<int>(type: "int", nullable: true),
                    MechanikId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Serwisy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Serwisy_Gokarty_GokartNumer",
                        column: x => x.GokartNumer,
                        principalTable: "Gokarty",
                        principalColumn: "Numer",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Serwisy_Pracownicy_IntruktorId",
                        column: x => x.IntruktorId,
                        principalTable: "Pracownicy",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Serwisy_Pracownicy_MechanikId",
                        column: x => x.MechanikId,
                        principalTable: "Pracownicy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Serwisy_GokartNumer",
                table: "Serwisy",
                column: "GokartNumer");

            migrationBuilder.CreateIndex(
                name: "IX_Serwisy_IntruktorId",
                table: "Serwisy",
                column: "IntruktorId");

            migrationBuilder.CreateIndex(
                name: "IX_Serwisy_MechanikId",
                table: "Serwisy",
                column: "MechanikId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Serwisy");
        }
    }
}
