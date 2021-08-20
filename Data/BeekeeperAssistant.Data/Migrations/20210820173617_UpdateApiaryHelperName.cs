using Microsoft.EntityFrameworkCore.Migrations;

namespace BeekeeperAssistant.Data.Migrations
{
    public partial class UpdateApiaryHelperName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApiaryHelper_Apiaries_ApiaryId",
                table: "ApiaryHelper");

            migrationBuilder.DropForeignKey(
                name: "FK_ApiaryHelper_AspNetUsers_UserId",
                table: "ApiaryHelper");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApiaryHelper",
                table: "ApiaryHelper");

            migrationBuilder.RenameTable(
                name: "ApiaryHelper",
                newName: "ApiaryHelpers");

            migrationBuilder.RenameIndex(
                name: "IX_ApiaryHelper_UserId",
                table: "ApiaryHelpers",
                newName: "IX_ApiaryHelpers_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApiaryHelpers",
                table: "ApiaryHelpers",
                columns: new[] { "ApiaryId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ApiaryHelpers_Apiaries_ApiaryId",
                table: "ApiaryHelpers",
                column: "ApiaryId",
                principalTable: "Apiaries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApiaryHelpers_AspNetUsers_UserId",
                table: "ApiaryHelpers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApiaryHelpers_Apiaries_ApiaryId",
                table: "ApiaryHelpers");

            migrationBuilder.DropForeignKey(
                name: "FK_ApiaryHelpers_AspNetUsers_UserId",
                table: "ApiaryHelpers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApiaryHelpers",
                table: "ApiaryHelpers");

            migrationBuilder.RenameTable(
                name: "ApiaryHelpers",
                newName: "ApiaryHelper");

            migrationBuilder.RenameIndex(
                name: "IX_ApiaryHelpers_UserId",
                table: "ApiaryHelper",
                newName: "IX_ApiaryHelper_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApiaryHelper",
                table: "ApiaryHelper",
                columns: new[] { "ApiaryId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ApiaryHelper_Apiaries_ApiaryId",
                table: "ApiaryHelper",
                column: "ApiaryId",
                principalTable: "Apiaries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApiaryHelper_AspNetUsers_UserId",
                table: "ApiaryHelper",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
