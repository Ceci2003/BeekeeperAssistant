using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeekeeperAssistant.Data.Migrations
{
    public partial class InitialCreateTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.CreateTable(
                name: "Apiaries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApiaryType = table.Column<int>(type: "int", nullable: false),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApiaryId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apiaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Apiaries_Apiaries_ApiaryId",
                        column: x => x.ApiaryId,
                        principalTable: "Apiaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Apiaries_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "Beehives",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApiaryId = table.Column<int>(type: "int", nullable: false),
                    CreatorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Number = table.Column<int>(type: "int", nullable: false),
                    BeehiveSystem = table.Column<int>(type: "int", nullable: false),
                    BeehiveType = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BeehivePower = table.Column<int>(type: "int", nullable: false),
                    HasDevice = table.Column<bool>(type: "bit", nullable: true),
                    HasPolenCatcher = table.Column<bool>(type: "bit", nullable: true),
                    HasPropolisCatcher = table.Column<bool>(type: "bit", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Beehives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Beehives_Apiaries_ApiaryId",
                        column: x => x.ApiaryId,
                        principalTable: "Apiaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Beehives_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Queens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BeehiveId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    FertilizationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GivingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QueenType = table.Column<int>(type: "int", nullable: false),
                    Origin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HygenicHabits = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Temperament = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<int>(type: "int", nullable: false),
                    Breed = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Queens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Queens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Queens_Beehives_BeehiveId",
                        column: x => x.BeehiveId,
                        principalTable: "Beehives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Apiaries_ApiaryId",
                table: "Apiaries",
                column: "ApiaryId");

            migrationBuilder.CreateIndex(
                name: "IX_Apiaries_CreatorId",
                table: "Apiaries",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Apiaries_IsDeleted",
                table: "Apiaries",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Beehives_ApiaryId",
                table: "Beehives",
                column: "ApiaryId");

            migrationBuilder.CreateIndex(
                name: "IX_Beehives_CreatorId",
                table: "Beehives",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Beehives_IsDeleted",
                table: "Beehives",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Queens_BeehiveId",
                table: "Queens",
                column: "BeehiveId");

            migrationBuilder.CreateIndex(
                name: "IX_Queens_IsDeleted",
                table: "Queens",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Queens_UserId",
                table: "Queens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotes_IsDeleted",
                table: "UserNotes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_UserTasks_IsDeleted",
                table: "UserTasks",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Queens");

            migrationBuilder.DropTable(
                name: "UserNotes");

            migrationBuilder.DropTable(
                name: "UserTasks");

            migrationBuilder.DropTable(
                name: "Beehives");

            migrationBuilder.DropTable(
                name: "Apiaries");

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Settings_IsDeleted",
                table: "Settings",
                column: "IsDeleted");
        }
    }
}
