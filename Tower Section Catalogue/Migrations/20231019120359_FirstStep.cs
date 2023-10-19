using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tower_Section_Catalogue.Migrations
{
    public partial class FirstStep : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TowerSection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    partNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    bottomDiameter = table.Column<double>(type: "float", nullable: false),
                    topDiameter = table.Column<double>(type: "float", nullable: false),
                    lenght = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TowerSection", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shell",
                columns: table => new
                {
                    ShellPosition = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Height = table.Column<double>(type: "float", nullable: false),
                    BottomDiameter = table.Column<double>(type: "float", nullable: false),
                    TopDiameter = table.Column<double>(type: "float", nullable: false),
                    Thickness = table.Column<double>(type: "float", nullable: false),
                    SteelDensity = table.Column<double>(type: "float", nullable: false),
                    TowerSectionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shell", x => x.ShellPosition);
                    table.ForeignKey(
                        name: "FK_Shell_TowerSection_TowerSectionId",
                        column: x => x.TowerSectionId,
                        principalTable: "TowerSection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shell_TowerSectionId",
                table: "Shell",
                column: "TowerSectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shell");

            migrationBuilder.DropTable(
                name: "TowerSection");
        }
    }
}
