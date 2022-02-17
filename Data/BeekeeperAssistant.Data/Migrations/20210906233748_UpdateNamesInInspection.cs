namespace BeekeeperAssistant.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class UpdateNamesInInspection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Predation",
                table: "Inspections",
                newName: "Predators");

            migrationBuilder.RenameColumn(
                name: "PolenActivity",
                table: "Inspections",
                newName: "PollenActivity");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Predators",
                table: "Inspections",
                newName: "Predation");

            migrationBuilder.RenameColumn(
                name: "PollenActivity",
                table: "Inspections",
                newName: "PolenActivity");
        }
    }
}
