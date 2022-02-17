namespace BeekeeperAssistant.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddHelpers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanRead",
                table: "ApiaryHelpers");

            migrationBuilder.DropColumn(
                name: "CanWrite",
                table: "ApiaryHelpers");

            migrationBuilder.AddColumn<int>(
                name: "Access",
                table: "ApiaryHelpers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Access",
                table: "ApiaryHelpers");

            migrationBuilder.AddColumn<bool>(
                name: "CanRead",
                table: "ApiaryHelpers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CanWrite",
                table: "ApiaryHelpers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
