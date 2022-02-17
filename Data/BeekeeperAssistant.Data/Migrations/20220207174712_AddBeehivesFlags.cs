namespace BeekeeperAssistant.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddBeehivesFlags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BeehiveMarkFlag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlagType = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeehiveMarkFlag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BeehivesFlags",
                columns: table => new
                {
                    BeehiveId = table.Column<int>(type: "int", nullable: false),
                    BeehiveMarkFlagId = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeehivesFlags", x => new { x.BeehiveId, x.BeehiveMarkFlagId });
                    table.ForeignKey(
                        name: "FK_BeehivesFlags_BeehiveMarkFlag_BeehiveMarkFlagId",
                        column: x => x.BeehiveMarkFlagId,
                        principalTable: "BeehiveMarkFlag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BeehivesFlags_Beehives_BeehiveId",
                        column: x => x.BeehiveId,
                        principalTable: "Beehives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BeehiveMarkFlag_IsDeleted",
                table: "BeehiveMarkFlag",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_BeehivesFlags_BeehiveMarkFlagId",
                table: "BeehivesFlags",
                column: "BeehiveMarkFlagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BeehivesFlags");

            migrationBuilder.DropTable(
                name: "BeehiveMarkFlag");
        }
    }
}
