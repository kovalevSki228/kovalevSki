using Microsoft.EntityFrameworkCore.Migrations;

namespace SaitCourses.Migrations
{
    public partial class MigrateDB4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "images",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "images",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "description",
                table: "images");

            migrationBuilder.DropColumn(
                name: "name",
                table: "images");
        }
    }
}
