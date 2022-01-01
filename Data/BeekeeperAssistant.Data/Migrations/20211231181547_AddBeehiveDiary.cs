using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeekeeperAssistant.Data.Migrations
{
    public partial class AddBeehiveDiary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApiaryDiaries_Apiaries_ApiaryId",
                table: "ApiaryDiaries");

            migrationBuilder.DropForeignKey(
                name: "FK_ApiaryDiaries_AspNetUsers_ModifiendById",
                table: "ApiaryDiaries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApiaryDiaries",
                table: "ApiaryDiaries");

            migrationBuilder.RenameTable(
                name: "ApiaryDiaries",
                newName: "ApiariesDiaries");

            migrationBuilder.RenameIndex(
                name: "IX_ApiaryDiaries_ModifiendById",
                table: "ApiariesDiaries",
                newName: "IX_ApiariesDiaries_ModifiendById");

            migrationBuilder.RenameIndex(
                name: "IX_ApiaryDiaries_IsDeleted",
                table: "ApiariesDiaries",
                newName: "IX_ApiariesDiaries_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_ApiaryDiaries_ApiaryId",
                table: "ApiariesDiaries",
                newName: "IX_ApiariesDiaries_ApiaryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApiariesDiaries",
                table: "ApiariesDiaries",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "BeehivesDiaries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BeehiveId = table.Column<int>(type: "int", nullable: false),
                    ModifiendById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeehivesDiaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BeehivesDiaries_AspNetUsers_ModifiendById",
                        column: x => x.ModifiendById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BeehivesDiaries_Beehives_BeehiveId",
                        column: x => x.BeehiveId,
                        principalTable: "Beehives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BeehivesDiaries_BeehiveId",
                table: "BeehivesDiaries",
                column: "BeehiveId");

            migrationBuilder.CreateIndex(
                name: "IX_BeehivesDiaries_IsDeleted",
                table: "BeehivesDiaries",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_BeehivesDiaries_ModifiendById",
                table: "BeehivesDiaries",
                column: "ModifiendById");

            migrationBuilder.AddForeignKey(
                name: "FK_ApiariesDiaries_Apiaries_ApiaryId",
                table: "ApiariesDiaries",
                column: "ApiaryId",
                principalTable: "Apiaries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApiariesDiaries_AspNetUsers_ModifiendById",
                table: "ApiariesDiaries",
                column: "ModifiendById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApiariesDiaries_Apiaries_ApiaryId",
                table: "ApiariesDiaries");

            migrationBuilder.DropForeignKey(
                name: "FK_ApiariesDiaries_AspNetUsers_ModifiendById",
                table: "ApiariesDiaries");

            migrationBuilder.DropTable(
                name: "BeehivesDiaries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApiariesDiaries",
                table: "ApiariesDiaries");

            migrationBuilder.RenameTable(
                name: "ApiariesDiaries",
                newName: "ApiaryDiaries");

            migrationBuilder.RenameIndex(
                name: "IX_ApiariesDiaries_ModifiendById",
                table: "ApiaryDiaries",
                newName: "IX_ApiaryDiaries_ModifiendById");

            migrationBuilder.RenameIndex(
                name: "IX_ApiariesDiaries_IsDeleted",
                table: "ApiaryDiaries",
                newName: "IX_ApiaryDiaries_IsDeleted");

            migrationBuilder.RenameIndex(
                name: "IX_ApiariesDiaries_ApiaryId",
                table: "ApiaryDiaries",
                newName: "IX_ApiaryDiaries_ApiaryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApiaryDiaries",
                table: "ApiaryDiaries",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApiaryDiaries_Apiaries_ApiaryId",
                table: "ApiaryDiaries",
                column: "ApiaryId",
                principalTable: "Apiaries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ApiaryDiaries_AspNetUsers_ModifiendById",
                table: "ApiaryDiaries",
                column: "ModifiendById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
