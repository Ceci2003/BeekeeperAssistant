namespace BeekeeperAssistant.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddIncludeStorageToInspection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IncludeStorage",
                table: "Inspections",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncludeStorage",
                table: "Inspections");
        }
    }
}
