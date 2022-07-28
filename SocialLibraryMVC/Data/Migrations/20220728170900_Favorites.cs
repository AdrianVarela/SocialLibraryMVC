using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialLibraryMVC.Data.Migrations
{
    public partial class Favorites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserFavorites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ISBN_13 = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFavorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFavorites_AspNetUsers_User_id",
                        column: x => x.User_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFavorites_Books_ISBN_13",
                        column: x => x.ISBN_13,
                        principalTable: "Books",
                        principalColumn: "ISBN_13",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserFavorites_ISBN_13",
                table: "UserFavorites",
                column: "ISBN_13");

            migrationBuilder.CreateIndex(
                name: "IX_UserFavorites_User_id",
                table: "UserFavorites",
                column: "User_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFavorites");
        }
    }
}
