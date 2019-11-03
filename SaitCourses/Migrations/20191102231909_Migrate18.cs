using Microsoft.EntityFrameworkCore.Migrations;

namespace SaitCourses.Migrations
{
    public partial class Migrate18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sex",
                table: "tshirts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "sex",
                table: "baskets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "size",
                table: "baskets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sex",
                table: "tshirts");

            migrationBuilder.DropColumn(
                name: "sex",
                table: "baskets");

            migrationBuilder.DropColumn(
                name: "size",
                table: "baskets");
        }
    }
}
