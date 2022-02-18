namespace BeekeeperAssistant.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddHelpersAndChangeTableNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BeehivesHelpers",
                columns: table => new
                {
                    BeehiveId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Access = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeehivesHelpers", x => new { x.BeehiveId, x.UserId });
                    table.ForeignKey(
                        name: "FK_BeehivesHelpers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BeehivesHelpers_Beehives_BeehiveId",
                        column: x => x.BeehiveId,
                        principalTable: "Beehives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QueensHelpers",
                columns: table => new
                {
                    QueenId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Access = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueensHelpers", x => new { x.QueenId, x.UserId });
                    table.ForeignKey(
                        name: "FK_QueensHelpers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QueensHelpers_Queens_QueenId",
                        column: x => x.QueenId,
                        principalTable: "Queens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BeehivesHelpers_UserId",
                table: "BeehivesHelpers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_QueensHelpers_UserId",
                table: "QueensHelpers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BeehivesHelpers");

            migrationBuilder.DropTable(
                name: "QueensHelpers");
        }
    }
}
