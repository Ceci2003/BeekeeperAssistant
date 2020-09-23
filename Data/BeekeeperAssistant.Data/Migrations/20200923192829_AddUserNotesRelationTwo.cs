using Microsoft.EntityFrameworkCore.Migrations;

namespace BeekeeperAssistant.Data.Migrations
{
    public partial class AddUserNotesRelationTwo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserApiaries_Apiaries_ApiaryId",
                table: "UserApiaries");

            migrationBuilder.DropForeignKey(
                name: "FK_UserApiaries_AspNetUsers_UserId",
                table: "UserApiaries");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNotes1_UserNotes_NoteId",
                table: "UserNotes1");

            migrationBuilder.DropForeignKey(
                name: "FK_UserNotes1_AspNetUsers_UserId",
                table: "UserNotes1");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserNotes1",
                table: "UserNotes1");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserApiaries",
                table: "UserApiaries");

            migrationBuilder.RenameTable(
                name: "UserNotes1",
                newName: "UsersNotes");

            migrationBuilder.RenameTable(
                name: "UserApiaries",
                newName: "UsersApiaries");

            migrationBuilder.RenameIndex(
                name: "IX_UserNotes1_NoteId",
                table: "UsersNotes",
                newName: "IX_UsersNotes_NoteId");

            migrationBuilder.RenameIndex(
                name: "IX_UserApiaries_ApiaryId",
                table: "UsersApiaries",
                newName: "IX_UsersApiaries_ApiaryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersNotes",
                table: "UsersNotes",
                columns: new[] { "UserId", "NoteId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersApiaries",
                table: "UsersApiaries",
                columns: new[] { "UserId", "ApiaryId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UsersApiaries_Apiaries_ApiaryId",
                table: "UsersApiaries",
                column: "ApiaryId",
                principalTable: "Apiaries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersApiaries_AspNetUsers_UserId",
                table: "UsersApiaries",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersNotes_UserNotes_NoteId",
                table: "UsersNotes",
                column: "NoteId",
                principalTable: "UserNotes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersNotes_AspNetUsers_UserId",
                table: "UsersNotes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersApiaries_Apiaries_ApiaryId",
                table: "UsersApiaries");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersApiaries_AspNetUsers_UserId",
                table: "UsersApiaries");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersNotes_UserNotes_NoteId",
                table: "UsersNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersNotes_AspNetUsers_UserId",
                table: "UsersNotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersNotes",
                table: "UsersNotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersApiaries",
                table: "UsersApiaries");

            migrationBuilder.RenameTable(
                name: "UsersNotes",
                newName: "UserNotes1");

            migrationBuilder.RenameTable(
                name: "UsersApiaries",
                newName: "UserApiaries");

            migrationBuilder.RenameIndex(
                name: "IX_UsersNotes_NoteId",
                table: "UserNotes1",
                newName: "IX_UserNotes1_NoteId");

            migrationBuilder.RenameIndex(
                name: "IX_UsersApiaries_ApiaryId",
                table: "UserApiaries",
                newName: "IX_UserApiaries_ApiaryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserNotes1",
                table: "UserNotes1",
                columns: new[] { "UserId", "NoteId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserApiaries",
                table: "UserApiaries",
                columns: new[] { "UserId", "ApiaryId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserApiaries_Apiaries_ApiaryId",
                table: "UserApiaries",
                column: "ApiaryId",
                principalTable: "Apiaries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserApiaries_AspNetUsers_UserId",
                table: "UserApiaries",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotes1_UserNotes_NoteId",
                table: "UserNotes1",
                column: "NoteId",
                principalTable: "UserNotes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotes1_AspNetUsers_UserId",
                table: "UserNotes1",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
