using Microsoft.EntityFrameworkCore.Migrations;

namespace SaitCourses.Migrations
{
    public partial class MigrateDB7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "shirts",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_shirts_userId",
                table: "shirts",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_shirts_AspNetUsers_userId",
                table: "shirts",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_shirts_AspNetUsers_userId",
                table: "shirts");

            migrationBuilder.DropIndex(
                name: "IX_shirts_userId",
                table: "shirts");

            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "shirts",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
