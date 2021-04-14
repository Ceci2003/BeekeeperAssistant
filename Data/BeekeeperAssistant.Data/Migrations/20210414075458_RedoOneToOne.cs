using Microsoft.EntityFrameworkCore.Migrations;

namespace BeekeeperAssistant.Data.Migrations
{
    public partial class RedoOneToOne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Queens_Beehives_BeehiveId",
                table: "Queens");

            migrationBuilder.DropIndex(
                name: "IX_Queens_BeehiveId",
                table: "Queens");

            migrationBuilder.DropIndex(
                name: "IX_Beehives_QueenId",
                table: "Beehives");

            migrationBuilder.CreateIndex(
                name: "IX_Beehives_QueenId",
                table: "Beehives",
                column: "QueenId",
                unique: true,
                filter: "[QueenId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Beehives_QueenId",
                table: "Beehives");

            migrationBuilder.CreateIndex(
                name: "IX_Queens_BeehiveId",
                table: "Queens",
                column: "BeehiveId");

            migrationBuilder.CreateIndex(
                name: "IX_Beehives_QueenId",
                table: "Beehives",
                column: "QueenId");

            migrationBuilder.AddForeignKey(
                name: "FK_Queens_Beehives_BeehiveId",
                table: "Queens",
                column: "BeehiveId",
                principalTable: "Beehives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
