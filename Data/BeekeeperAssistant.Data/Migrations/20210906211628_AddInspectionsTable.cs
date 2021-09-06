using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BeekeeperAssistant.Data.Migrations
{
    public partial class AddInspectionsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inspections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BeehiveId = table.Column<int>(type: "int", nullable: false),
                    DateOfInspection = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InspectionType = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Swarmed = table.Column<bool>(type: "bit", nullable: false),
                    BeehivePower = table.Column<int>(type: "int", nullable: false),
                    BeehiveTemperament = table.Column<int>(type: "int", nullable: false),
                    BeehiveActions = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    HiveTemperature = table.Column<double>(type: "float", nullable: false),
                    HiveHumidity = table.Column<double>(type: "float", nullable: false),
                    IncludeQueenSection = table.Column<bool>(type: "bit", nullable: false),
                    QueenSeen = table.Column<bool>(type: "bit", nullable: false),
                    QueenCells = table.Column<int>(type: "int", nullable: false),
                    QueenWorkingStatus = table.Column<int>(type: "int", nullable: false),
                    QueenPowerStatus = table.Column<int>(type: "int", nullable: false),
                    IncludeBrood = table.Column<bool>(type: "bit", nullable: false),
                    Eggs = table.Column<bool>(type: "bit", nullable: false),
                    ClappedBrood = table.Column<bool>(type: "bit", nullable: false),
                    UnclappedBrood = table.Column<bool>(type: "bit", nullable: false),
                    IncludeFramesWith = table.Column<bool>(type: "bit", nullable: false),
                    Bees = table.Column<int>(type: "int", nullable: false),
                    Brood = table.Column<int>(type: "int", nullable: false),
                    Honey = table.Column<int>(type: "int", nullable: false),
                    Pollen = table.Column<int>(type: "int", nullable: false),
                    IncludeActivity = table.Column<bool>(type: "bit", nullable: false),
                    BeeActivity = table.Column<int>(type: "int", nullable: false),
                    OrientationActivity = table.Column<int>(type: "int", nullable: false),
                    PolenActivity = table.Column<int>(type: "int", nullable: false),
                    ForragingActivity = table.Column<int>(type: "int", nullable: false),
                    BeesPerMinute = table.Column<int>(type: "int", nullable: false),
                    StoredHoney = table.Column<int>(type: "int", nullable: false),
                    StoredPollen = table.Column<int>(type: "int", nullable: false),
                    IncludeSpottedProblem = table.Column<bool>(type: "bit", nullable: false),
                    Disease = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Treatment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pests = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Predation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IncludeWeatherInfo = table.Column<bool>(type: "bit", nullable: false),
                    Conditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WeatherTemperature = table.Column<double>(type: "float", nullable: false),
                    WeatherHumidity = table.Column<double>(type: "float", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inspections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inspections_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inspections_Beehives_BeehiveId",
                        column: x => x.BeehiveId,
                        principalTable: "Beehives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_BeehiveId",
                table: "Inspections",
                column: "BeehiveId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_CreatorId",
                table: "Inspections",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_IsDeleted",
                table: "Inspections",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inspections");
        }
    }
}
