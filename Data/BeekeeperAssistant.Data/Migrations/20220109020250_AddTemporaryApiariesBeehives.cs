namespace BeekeeperAssistant.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddTemporaryApiariesBeehives : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ClosingDate",
                table: "Apiaries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsClosed",
                table: "Apiaries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "OpeningDate",
                table: "Apiaries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "TemporaryApiariesBeehives",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApiaryId = table.Column<int>(type: "int", nullable: false),
                    BeehiveId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemporaryApiariesBeehives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemporaryApiariesBeehives_Apiaries_ApiaryId",
                        column: x => x.ApiaryId,
                        principalTable: "Apiaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TemporaryApiariesBeehives_Beehives_BeehiveId",
                        column: x => x.BeehiveId,
                        principalTable: "Beehives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TemporaryApiariesBeehives_ApiaryId",
                table: "TemporaryApiariesBeehives",
                column: "ApiaryId");

            migrationBuilder.CreateIndex(
                name: "IX_TemporaryApiariesBeehives_BeehiveId",
                table: "TemporaryApiariesBeehives",
                column: "BeehiveId");

            migrationBuilder.CreateIndex(
                name: "IX_TemporaryApiariesBeehives_IsDeleted",
                table: "TemporaryApiariesBeehives",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TemporaryApiariesBeehives");

            migrationBuilder.DropColumn(
                name: "ClosingDate",
                table: "Apiaries");

            migrationBuilder.DropColumn(
                name: "IsClosed",
                table: "Apiaries");

            migrationBuilder.DropColumn(
                name: "OpeningDate",
                table: "Apiaries");
        }
    }
}
