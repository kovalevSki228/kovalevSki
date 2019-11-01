using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SaitCourses.Migrations
{
    public partial class MigratieDB16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "baskets",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    nameShirt = table.Column<string>(nullable: true),
                    dataOfPurchase = table.Column<string>(nullable: true),
                    userId = table.Column<int>(nullable: false),
                    userId1 = table.Column<string>(nullable: true),
                    shirtid = table.Column<int>(nullable: false),
                    purchaseStatus = table.Column<bool>(nullable: false),
                    amount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_baskets", x => x.id);
                    table.ForeignKey(
                        name: "FK_baskets_tshirts_shirtid",
                        column: x => x.shirtid,
                        principalTable: "tshirts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_baskets_AspNetUsers_userId1",
                        column: x => x.userId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tagInTShirts",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    tagid = table.Column<int>(nullable: false),
                    shirtid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tagInTShirts", x => x.id);
                    table.ForeignKey(
                        name: "FK_tagInTShirts_tshirts_shirtid",
                        column: x => x.shirtid,
                        principalTable: "tshirts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tagInTShirts_tags_tagid",
                        column: x => x.tagid,
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_baskets_shirtid",
                table: "baskets",
                column: "shirtid");

            migrationBuilder.CreateIndex(
                name: "IX_baskets_userId1",
                table: "baskets",
                column: "userId1");

            migrationBuilder.CreateIndex(
                name: "IX_tagInTShirts_shirtid",
                table: "tagInTShirts",
                column: "shirtid");

            migrationBuilder.CreateIndex(
                name: "IX_tagInTShirts_tagid",
                table: "tagInTShirts",
                column: "tagid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "baskets");

            migrationBuilder.DropTable(
                name: "tagInTShirts");

            migrationBuilder.DropTable(
                name: "tags");
        }
    }
}
