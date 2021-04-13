using Microsoft.EntityFrameworkCore.Migrations;

namespace BeekeeperAssistant.Data.Migrations
{
    public partial class MakeBeesHivesandQueensOneToOne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QueenId",
                table: "Beehives",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Beehives_QueenId",
                table: "Beehives",
                column: "QueenId");

            migrationBuilder.AddForeignKey(
                name: "FK_Beehives_Queens_QueenId",
                table: "Beehives",
                column: "QueenId",
                principalTable: "Queens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Beehives_Queens_QueenId",
                table: "Beehives");

            migrationBuilder.DropIndex(
                name: "IX_Beehives_QueenId",
                table: "Beehives");

            migrationBuilder.DropColumn(
                name: "QueenId",
                table: "Beehives");
        }
    }
}
