using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EKids.Chatbot.Users.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddChildrenCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Children_AspNetUsers_ChildUserId",
                table: "Children");

            migrationBuilder.AddForeignKey(
                name: "FK_Children_AspNetUsers_ChildUserId",
                table: "Children",
                column: "ChildUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Children_AspNetUsers_ChildUserId",
                table: "Children");

            migrationBuilder.AddForeignKey(
                name: "FK_Children_AspNetUsers_ChildUserId",
                table: "Children",
                column: "ChildUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
