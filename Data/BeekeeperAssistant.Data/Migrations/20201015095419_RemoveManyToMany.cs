using Microsoft.EntityFrameworkCore.Migrations;

namespace BeekeeperAssistant.Data.Migrations
{
    public partial class RemoveManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersApiaries");

            migrationBuilder.DropTable(
                name: "UsersNotes");

            migrationBuilder.DropTable(
                name: "UsersTasks");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UsersApiaries",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApiaryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersApiaries", x => new { x.UserId, x.ApiaryId });
                    table.ForeignKey(
                        name: "FK_UsersApiaries_Apiaries_ApiaryId",
                        column: x => x.ApiaryId,
                        principalTable: "Apiaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersApiaries_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsersNotes",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NoteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersNotes", x => new { x.UserId, x.NoteId });
                    table.ForeignKey(
                        name: "FK_UsersNotes_UserNotes_NoteId",
                        column: x => x.NoteId,
                        principalTable: "UserNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersNotes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsersTasks",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TaskId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersTasks", x => new { x.UserId, x.TaskId });
                    table.ForeignKey(
                        name: "FK_UsersTasks_UserTasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "UserTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersTasks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsersApiaries_ApiaryId",
                table: "UsersApiaries",
                column: "ApiaryId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersNotes_NoteId",
                table: "UsersNotes",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersTasks_TaskId",
                table: "UsersTasks",
                column: "TaskId");
        }
    }
}
