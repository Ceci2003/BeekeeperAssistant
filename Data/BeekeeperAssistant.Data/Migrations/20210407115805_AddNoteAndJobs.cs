namespace BeekeeperAssistant.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddNoteAndJobs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Apiaries_Apiaries_ApiaryId",
                table: "Apiaries");

            migrationBuilder.DropTable(
                name: "UserNotes");

            migrationBuilder.DropTable(
                name: "UserTasks");

            migrationBuilder.DropIndex(
                name: "IX_Apiaries_ApiaryId",
                table: "Apiaries");

            migrationBuilder.DropColumn(
                name: "ApiaryId",
                table: "Apiaries");

            migrationBuilder.AlterColumn<bool>(
                name: "HasPropolisCatcher",
                table: "Beehives",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "HasPolenCatcher",
                table: "Beehives",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "HasDevice",
                table: "Beehives",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserNote",
                columns: table => new
                {
                    NotesId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserNote", x => new { x.NotesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserNote_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationUserNote_Notes_NotesId",
                        column: x => x.NotesId,
                        principalTable: "Notes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserJobs",
                columns: table => new
                {
                    TasksId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserJobs", x => new { x.TasksId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserJobs_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApplicationUserJobs_Tasks_TasksId",
                        column: x => x.TasksId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserJobs_UsersId",
                table: "ApplicationUserJobs",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserNote_UsersId",
                table: "ApplicationUserNote",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_IsDeleted",
                table: "Notes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_IsDeleted",
                table: "Tasks",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserJobs");

            migrationBuilder.DropTable(
                name: "ApplicationUserNote");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.AlterColumn<bool>(
                name: "HasPropolisCatcher",
                table: "Beehives",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "HasPolenCatcher",
                table: "Beehives",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "HasDevice",
                table: "Beehives",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<int>(
                name: "ApiaryId",
                table: "Apiaries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNotes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTasks", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Apiaries_ApiaryId",
                table: "Apiaries",
                column: "ApiaryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotes_IsDeleted",
                table: "UserNotes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_UserTasks_IsDeleted",
                table: "UserTasks",
                column: "IsDeleted");

            migrationBuilder.AddForeignKey(
                name: "FK_Apiaries_Apiaries_ApiaryId",
                table: "Apiaries",
                column: "ApiaryId",
                principalTable: "Apiaries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
