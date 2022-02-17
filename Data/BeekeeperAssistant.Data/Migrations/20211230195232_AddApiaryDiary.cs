namespace BeekeeperAssistant.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddApiaryDiary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiaryDiaries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApiaryId = table.Column<int>(type: "int", nullable: false),
                    ModifiendById = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiaryDiaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApiaryDiaries_Apiaries_ApiaryId",
                        column: x => x.ApiaryId,
                        principalTable: "Apiaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ApiaryDiaries_AspNetUsers_ModifiendById",
                        column: x => x.ModifiendById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiaryDiaries_ApiaryId",
                table: "ApiaryDiaries",
                column: "ApiaryId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiaryDiaries_IsDeleted",
                table: "ApiaryDiaries",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ApiaryDiaries_ModifiendById",
                table: "ApiaryDiaries",
                column: "ModifiendById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiaryDiaries");
        }
    }
}
