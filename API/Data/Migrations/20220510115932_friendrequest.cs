using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.Migrations
{
    public partial class friendrequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FriendshipRequests");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FriendshipRequests",
                columns: table => new
                {
                    SourceUserId = table.Column<string>(type: "TEXT", nullable: false),
                    DestinationUserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendshipRequests", x => new { x.SourceUserId, x.DestinationUserId });
                    table.ForeignKey(
                        name: "FK_FriendshipRequests_AspNetUsers_DestinationUserId",
                        column: x => x.DestinationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FriendshipRequests_AspNetUsers_SourceUserId",
                        column: x => x.SourceUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FriendshipRequests_DestinationUserId",
                table: "FriendshipRequests",
                column: "DestinationUserId");
        }
    }
}
