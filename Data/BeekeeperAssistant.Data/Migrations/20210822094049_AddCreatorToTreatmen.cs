namespace BeekeeperAssistant.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddCreatorToTreatmen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Treatments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_CreatorId",
                table: "Treatments",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Treatments_AspNetUsers_CreatorId",
                table: "Treatments",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Treatments_AspNetUsers_CreatorId",
                table: "Treatments");

            migrationBuilder.DropIndex(
                name: "IX_Treatments_CreatorId",
                table: "Treatments");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Treatments");
        }
    }
}
