using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class LikeEntityAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    SourceUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    LikedUserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => new { x.SourceUserId, x.LikedUserId });
                    table.ForeignKey(
                        name: "FK_Likes_Users_LikedUserId",
                        column: x => x.LikedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Likes_Users_SourceUserId",
                        column: x => x.SourceUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLikeUserLike",
                columns: table => new
                {
                    LikedByUsersSourceUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    LikedByUsersLikedUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    LikedUsersSourceUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    LikedUsersLikedUserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLikeUserLike", x => new { x.LikedByUsersSourceUserId, x.LikedByUsersLikedUserId, x.LikedUsersSourceUserId, x.LikedUsersLikedUserId });
                    table.ForeignKey(
                        name: "FK_UserLikeUserLike_Likes_LikedByUsersSourceUserId_LikedByUsersLikedUserId",
                        columns: x => new { x.LikedByUsersSourceUserId, x.LikedByUsersLikedUserId },
                        principalTable: "Likes",
                        principalColumns: new[] { "SourceUserId", "LikedUserId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLikeUserLike_Likes_LikedUsersSourceUserId_LikedUsersLikedUserId",
                        columns: x => new { x.LikedUsersSourceUserId, x.LikedUsersLikedUserId },
                        principalTable: "Likes",
                        principalColumns: new[] { "SourceUserId", "LikedUserId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Likes_LikedUserId",
                table: "Likes",
                column: "LikedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLikeUserLike_LikedUsersSourceUserId_LikedUsersLikedUserId",
                table: "UserLikeUserLike",
                columns: new[] { "LikedUsersSourceUserId", "LikedUsersLikedUserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserLikeUserLike");

            migrationBuilder.DropTable(
                name: "Likes");
        }
    }
}
