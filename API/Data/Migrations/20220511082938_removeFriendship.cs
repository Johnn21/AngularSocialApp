using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class removeFriendship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friendships_AspNetUsers_FriendUserId",
                table: "Friendships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Friendships",
                table: "Friendships");

            migrationBuilder.RenameTable(
                name: "Friendships",
                newName: "Friendship");

            migrationBuilder.RenameIndex(
                name: "IX_Friendships_FriendUserId",
                table: "Friendship",
                newName: "IX_Friendship_FriendUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Friendship",
                table: "Friendship",
                columns: new[] { "UserId", "FriendUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Friendship_AspNetUsers_FriendUserId",
                table: "Friendship",
                column: "FriendUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friendship_AspNetUsers_FriendUserId",
                table: "Friendship");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Friendship",
                table: "Friendship");

            migrationBuilder.RenameTable(
                name: "Friendship",
                newName: "Friendships");

            migrationBuilder.RenameIndex(
                name: "IX_Friendship_FriendUserId",
                table: "Friendships",
                newName: "IX_Friendships_FriendUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Friendships",
                table: "Friendships",
                columns: new[] { "UserId", "FriendUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Friendships_AspNetUsers_FriendUserId",
                table: "Friendships",
                column: "FriendUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
