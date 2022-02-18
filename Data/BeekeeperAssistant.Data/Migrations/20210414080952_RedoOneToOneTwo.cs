namespace BeekeeperAssistant.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class RedoOneToOneTwo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Beehives_Queens_QueenId",
                table: "Beehives");

            migrationBuilder.DropIndex(
                name: "IX_Beehives_QueenId",
                table: "Beehives");

            migrationBuilder.CreateIndex(
                name: "IX_Queens_BeehiveId",
                table: "Queens",
                column: "BeehiveId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Queens_Beehives_BeehiveId",
                table: "Queens",
                column: "BeehiveId",
                principalTable: "Beehives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Queens_Beehives_BeehiveId",
                table: "Queens");

            migrationBuilder.DropIndex(
                name: "IX_Queens_BeehiveId",
                table: "Queens");

            migrationBuilder.CreateIndex(
                name: "IX_Beehives_QueenId",
                table: "Beehives",
                column: "QueenId",
                unique: true,
                filter: "[QueenId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Beehives_Queens_QueenId",
                table: "Beehives",
                column: "QueenId",
                principalTable: "Queens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
