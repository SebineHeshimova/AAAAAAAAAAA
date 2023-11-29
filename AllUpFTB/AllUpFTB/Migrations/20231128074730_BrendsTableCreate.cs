using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AllUpFTB.Migrations
{
    public partial class BrendsTableCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Brend",
                table: "Products");

            migrationBuilder.AddColumn<int>(
                name: "BrendId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Brends",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brends", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrendId",
                table: "Products",
                column: "BrendId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Brends_BrendId",
                table: "Products",
                column: "BrendId",
                principalTable: "Brends",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Brends_BrendId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Brends");

            migrationBuilder.DropIndex(
                name: "IX_Products_BrendId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BrendId",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "Brend",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
