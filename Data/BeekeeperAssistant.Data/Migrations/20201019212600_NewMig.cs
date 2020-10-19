using Microsoft.EntityFrameworkCore.Migrations;

namespace BeekeeperAssistant.Data.Migrations
{
    public partial class NewMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Apiaries_AspNetUsers_UserId",
                table: "Apiaries");

            migrationBuilder.DropForeignKey(
                name: "FK_Beehives_AspNetUsers_UserId",
                table: "Beehives");

            migrationBuilder.DropIndex(
                name: "IX_Beehives_UserId",
                table: "Beehives");

            migrationBuilder.DropIndex(
                name: "IX_Apiaries_UserId",
                table: "Apiaries");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Beehives");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Apiaries");

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Beehives",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Apiaries",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Beehives_CreatorId",
                table: "Beehives",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Apiaries_CreatorId",
                table: "Apiaries",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Apiaries_AspNetUsers_CreatorId",
                table: "Apiaries",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Beehives_AspNetUsers_CreatorId",
                table: "Beehives",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Apiaries_AspNetUsers_CreatorId",
                table: "Apiaries");

            migrationBuilder.DropForeignKey(
                name: "FK_Beehives_AspNetUsers_CreatorId",
                table: "Beehives");

            migrationBuilder.DropIndex(
                name: "IX_Beehives_CreatorId",
                table: "Beehives");

            migrationBuilder.DropIndex(
                name: "IX_Apiaries_CreatorId",
                table: "Apiaries");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Beehives");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Apiaries");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Beehives",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Apiaries",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Beehives_UserId",
                table: "Beehives",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Apiaries_UserId",
                table: "Apiaries",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Apiaries_AspNetUsers_UserId",
                table: "Apiaries",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Beehives_AspNetUsers_UserId",
                table: "Beehives",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
