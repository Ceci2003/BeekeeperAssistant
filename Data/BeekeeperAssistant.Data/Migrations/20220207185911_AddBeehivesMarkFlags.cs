namespace BeekeeperAssistant.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddBeehivesMarkFlags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BeehivesFlags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BeehiveMarkFlag",
                table: "BeehiveMarkFlag");

            migrationBuilder.RenameTable(
                name: "BeehiveMarkFlag",
                newName: "BeehivesMarkFlags");

            migrationBuilder.RenameIndex(
                name: "IX_BeehiveMarkFlag_IsDeleted",
                table: "BeehivesMarkFlags",
                newName: "IX_BeehivesMarkFlags_IsDeleted");

            migrationBuilder.AddColumn<int>(
                name: "BeehiveId",
                table: "BeehivesMarkFlags",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BeehivesMarkFlags",
                table: "BeehivesMarkFlags",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BeehivesMarkFlags_BeehiveId",
                table: "BeehivesMarkFlags",
                column: "BeehiveId");

            migrationBuilder.AddForeignKey(
                name: "FK_BeehivesMarkFlags_Beehives_BeehiveId",
                table: "BeehivesMarkFlags",
                column: "BeehiveId",
                principalTable: "Beehives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BeehivesMarkFlags_Beehives_BeehiveId",
                table: "BeehivesMarkFlags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BeehivesMarkFlags",
                table: "BeehivesMarkFlags");

            migrationBuilder.DropIndex(
                name: "IX_BeehivesMarkFlags_BeehiveId",
                table: "BeehivesMarkFlags");

            migrationBuilder.DropColumn(
                name: "BeehiveId",
                table: "BeehivesMarkFlags");

            migrationBuilder.RenameTable(
                name: "BeehivesMarkFlags",
                newName: "BeehiveMarkFlag");

            migrationBuilder.RenameIndex(
                name: "IX_BeehivesMarkFlags_IsDeleted",
                table: "BeehiveMarkFlag",
                newName: "IX_BeehiveMarkFlag_IsDeleted");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BeehiveMarkFlag",
                table: "BeehiveMarkFlag",
                column: "Id");

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
                name: "IX_BeehivesFlags_BeehiveMarkFlagId",
                table: "BeehivesFlags",
                column: "BeehiveMarkFlagId");
        }
    }
}
