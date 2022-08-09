using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeekeeperAssistant.Data.Migrations
{
    public partial class AddedUserTasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserDuty");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Duties",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTasks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Duties_ApplicationUserId",
                table: "Duties",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTasks_IsDeleted",
                table: "UserTasks",
                column: "IsDeleted");

            migrationBuilder.AddForeignKey(
                name: "FK_Duties_AspNetUsers_ApplicationUserId",
                table: "Duties",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Duties_AspNetUsers_ApplicationUserId",
                table: "Duties");

            migrationBuilder.DropTable(
                name: "UserTasks");

            migrationBuilder.DropIndex(
                name: "IX_Duties_ApplicationUserId",
                table: "Duties");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Duties");

            migrationBuilder.CreateTable(
                name: "ApplicationUserDuty",
                columns: table => new
                {
                    TasksId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserDuty", x => new { x.TasksId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserDuty_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationUserDuty_Duties_TasksId",
                        column: x => x.TasksId,
                        principalTable: "Duties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserDuty_UsersId",
                table: "ApplicationUserDuty",
                column: "UsersId");
        }
    }
}
