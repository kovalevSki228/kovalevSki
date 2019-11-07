using Microsoft.EntityFrameworkCore.Migrations;

namespace SaitCourses.Migrations
{
    public partial class qwerty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_baskets_AspNetUsers_userId",
                table: "baskets");

            migrationBuilder.DropIndex(
                name: "IX_baskets_userId",
                table: "baskets");

            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "baskets",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "baskets",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_baskets_userId",
                table: "baskets",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_baskets_AspNetUsers_userId",
                table: "baskets",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
