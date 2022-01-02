using Microsoft.EntityFrameworkCore.Migrations;

namespace BeekeeperAssistant.Data.Migrations
{
    public partial class UpdateBeehiveNoteFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BeehiveNotes_AspNetUsers_CreatorId",
                table: "BeehiveNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_BeehiveNotes_AspNetUsers_OwnerId",
                table: "BeehiveNotes");

            migrationBuilder.DropIndex(
                name: "IX_BeehiveNotes_CreatorId",
                table: "BeehiveNotes");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "BeehiveNotes");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "BeehiveNotes",
                newName: "ModifiendById");

            migrationBuilder.RenameIndex(
                name: "IX_BeehiveNotes_OwnerId",
                table: "BeehiveNotes",
                newName: "IX_BeehiveNotes_ModifiendById");

            migrationBuilder.AddForeignKey(
                name: "FK_BeehiveNotes_AspNetUsers_ModifiendById",
                table: "BeehiveNotes",
                column: "ModifiendById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BeehiveNotes_AspNetUsers_ModifiendById",
                table: "BeehiveNotes");

            migrationBuilder.RenameColumn(
                name: "ModifiendById",
                table: "BeehiveNotes",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_BeehiveNotes_ModifiendById",
                table: "BeehiveNotes",
                newName: "IX_BeehiveNotes_OwnerId");

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "BeehiveNotes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BeehiveNotes_CreatorId",
                table: "BeehiveNotes",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_BeehiveNotes_AspNetUsers_CreatorId",
                table: "BeehiveNotes",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BeehiveNotes_AspNetUsers_OwnerId",
                table: "BeehiveNotes",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
