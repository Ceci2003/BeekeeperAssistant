namespace BeekeeperAssistant.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class RenameBeehiveActions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BeehiveActions",
                table: "Inspections",
                newName: "BeehiveAction");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BeehiveAction",
                table: "Inspections",
                newName: "BeehiveActions");
        }
    }
}
