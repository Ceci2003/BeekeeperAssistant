namespace BeekeeperAssistant.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class UpdateHarvestedBeehivesQuantity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quqntity",
                table: "Harvests",
                newName: "Quantity");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Harvests",
                newName: "Quqntity");
        }
    }
}
