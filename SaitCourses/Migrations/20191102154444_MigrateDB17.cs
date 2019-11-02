using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SaitCourses.Migrations
{
    public partial class MigrateDB17 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_baskets_AspNetUsers_userId1",
                table: "baskets");

            migrationBuilder.DropIndex(
                name: "IX_baskets_userId1",
                table: "baskets");

            migrationBuilder.DropColumn(
                name: "userId1",
                table: "baskets");

            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "baskets",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "achievements",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(nullable: true),
                    achievementRequirements = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_achievements", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "achievementsUsers",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    achievementid = table.Column<int>(nullable: false),
                    userid = table.Column<string>(nullable: true),
                    achievementRequirements = table.Column<int>(nullable: false),
                    progressUser = table.Column<int>(nullable: false),
                    image = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_achievementsUsers", x => x.id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_baskets_AspNetUsers_userId",
                table: "baskets");

            migrationBuilder.DropTable(
                name: "achievements");

            migrationBuilder.DropTable(
                name: "achievementsUsers");

            migrationBuilder.DropIndex(
                name: "IX_baskets_userId",
                table: "baskets");

            migrationBuilder.AlterColumn<int>(
                name: "userId",
                table: "baskets",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "userId1",
                table: "baskets",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_baskets_userId1",
                table: "baskets",
                column: "userId1");

            migrationBuilder.AddForeignKey(
                name: "FK_baskets_AspNetUsers_userId1",
                table: "baskets",
                column: "userId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
