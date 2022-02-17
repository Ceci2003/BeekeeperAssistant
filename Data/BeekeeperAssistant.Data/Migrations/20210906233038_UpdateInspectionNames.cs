namespace BeekeeperAssistant.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class UpdateInspectionNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Pollen",
                table: "Inspections",
                newName: "FramesWithPollen");

            migrationBuilder.RenameColumn(
                name: "Honey",
                table: "Inspections",
                newName: "FramesWithHoney");

            migrationBuilder.RenameColumn(
                name: "Brood",
                table: "Inspections",
                newName: "FramesWithBrood");

            migrationBuilder.RenameColumn(
                name: "Bees",
                table: "Inspections",
                newName: "FramesWithBees");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FramesWithPollen",
                table: "Inspections",
                newName: "Pollen");

            migrationBuilder.RenameColumn(
                name: "FramesWithHoney",
                table: "Inspections",
                newName: "Honey");

            migrationBuilder.RenameColumn(
                name: "FramesWithBrood",
                table: "Inspections",
                newName: "Brood");

            migrationBuilder.RenameColumn(
                name: "FramesWithBees",
                table: "Inspections",
                newName: "Bees");
        }
    }
}
