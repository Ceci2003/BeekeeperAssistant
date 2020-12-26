using Microsoft.EntityFrameworkCore.Migrations;

namespace BeekeeperAssistant.Data.Migrations
{
    public partial class AddUserIdToQueen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Queens",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Queens_UserId",
                table: "Queens",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Queens_AspNetUsers_UserId",
                table: "Queens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Queens_AspNetUsers_UserId",
                table: "Queens");

            migrationBuilder.DropIndex(
                name: "IX_Queens_UserId",
                table: "Queens");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Queens");
        }
    }
}
