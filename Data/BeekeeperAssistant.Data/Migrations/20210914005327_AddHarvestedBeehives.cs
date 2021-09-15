using Microsoft.EntityFrameworkCore.Migrations;

namespace BeekeeperAssistant.Data.Migrations
{
    public partial class AddHarvestedBeehives : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Harvests_AspNetUsers_ApplicationUserId",
                table: "Harvests");

            migrationBuilder.DropForeignKey(
                name: "FK_Harvests_Beehives_BeehiveId",
                table: "Harvests");

            migrationBuilder.DropIndex(
                name: "IX_Harvests_BeehiveId",
                table: "Harvests");

            migrationBuilder.RenameColumn(
                name: "Product",
                table: "Harvests",
                newName: "Unit");

            migrationBuilder.RenameColumn(
                name: "BeehiveId",
                table: "Harvests",
                newName: "Quqntity");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Harvests",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Harvests",
                newName: "HarvestProductType");

            migrationBuilder.RenameIndex(
                name: "IX_Harvests_ApplicationUserId",
                table: "Harvests",
                newName: "IX_Harvests_CreatorId");

            migrationBuilder.CreateTable(
                name: "HarvestedBeehives",
                columns: table => new
                {
                    BeehiveId = table.Column<int>(type: "int", nullable: false),
                    HarvestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HarvestedBeehives", x => new { x.BeehiveId, x.HarvestId });
                    table.ForeignKey(
                        name: "FK_HarvestedBeehives_Beehives_BeehiveId",
                        column: x => x.BeehiveId,
                        principalTable: "Beehives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HarvestedBeehives_Harvests_HarvestId",
                        column: x => x.HarvestId,
                        principalTable: "Harvests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HarvestedBeehives_HarvestId",
                table: "HarvestedBeehives",
                column: "HarvestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Harvests_AspNetUsers_CreatorId",
                table: "Harvests",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Harvests_AspNetUsers_CreatorId",
                table: "Harvests");

            migrationBuilder.DropTable(
                name: "HarvestedBeehives");

            migrationBuilder.RenameColumn(
                name: "Unit",
                table: "Harvests",
                newName: "Product");

            migrationBuilder.RenameColumn(
                name: "Quqntity",
                table: "Harvests",
                newName: "BeehiveId");

            migrationBuilder.RenameColumn(
                name: "HarvestProductType",
                table: "Harvests",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Harvests",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Harvests_CreatorId",
                table: "Harvests",
                newName: "IX_Harvests_ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Harvests_BeehiveId",
                table: "Harvests",
                column: "BeehiveId");

            migrationBuilder.AddForeignKey(
                name: "FK_Harvests_AspNetUsers_ApplicationUserId",
                table: "Harvests",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Harvests_Beehives_BeehiveId",
                table: "Harvests",
                column: "BeehiveId",
                principalTable: "Beehives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
