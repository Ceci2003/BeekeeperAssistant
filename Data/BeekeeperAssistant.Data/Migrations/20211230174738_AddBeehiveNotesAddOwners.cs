namespace BeekeeperAssistant.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddBeehiveNotesAddOwners : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Queens_AspNetUsers_UserId",
                table: "Queens");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Queens",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Queens_UserId",
                table: "Queens",
                newName: "IX_Queens_OwnerId");

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Treatments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Queens",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Inspections",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Harvests",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Beehives",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BeehiveNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    BeehiveId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeehiveNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BeehiveNotes_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BeehiveNotes_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BeehiveNotes_Beehives_BeehiveId",
                        column: x => x.BeehiveId,
                        principalTable: "Beehives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_OwnerId",
                table: "Treatments",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Queens_CreatorId",
                table: "Queens",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Inspections_OwnerId",
                table: "Inspections",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Harvests_OwnerId",
                table: "Harvests",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Beehives_OwnerId",
                table: "Beehives",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_BeehiveNotes_BeehiveId",
                table: "BeehiveNotes",
                column: "BeehiveId");

            migrationBuilder.CreateIndex(
                name: "IX_BeehiveNotes_CreatorId",
                table: "BeehiveNotes",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_BeehiveNotes_IsDeleted",
                table: "BeehiveNotes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_BeehiveNotes_OwnerId",
                table: "BeehiveNotes",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Beehives_AspNetUsers_OwnerId",
                table: "Beehives",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Harvests_AspNetUsers_OwnerId",
                table: "Harvests",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Inspections_AspNetUsers_OwnerId",
                table: "Inspections",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Queens_AspNetUsers_CreatorId",
                table: "Queens",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Queens_AspNetUsers_OwnerId",
                table: "Queens",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Treatments_AspNetUsers_OwnerId",
                table: "Treatments",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Beehives_AspNetUsers_OwnerId",
                table: "Beehives");

            migrationBuilder.DropForeignKey(
                name: "FK_Harvests_AspNetUsers_OwnerId",
                table: "Harvests");

            migrationBuilder.DropForeignKey(
                name: "FK_Inspections_AspNetUsers_OwnerId",
                table: "Inspections");

            migrationBuilder.DropForeignKey(
                name: "FK_Queens_AspNetUsers_CreatorId",
                table: "Queens");

            migrationBuilder.DropForeignKey(
                name: "FK_Queens_AspNetUsers_OwnerId",
                table: "Queens");

            migrationBuilder.DropForeignKey(
                name: "FK_Treatments_AspNetUsers_OwnerId",
                table: "Treatments");

            migrationBuilder.DropTable(
                name: "BeehiveNotes");

            migrationBuilder.DropIndex(
                name: "IX_Treatments_OwnerId",
                table: "Treatments");

            migrationBuilder.DropIndex(
                name: "IX_Queens_CreatorId",
                table: "Queens");

            migrationBuilder.DropIndex(
                name: "IX_Inspections_OwnerId",
                table: "Inspections");

            migrationBuilder.DropIndex(
                name: "IX_Harvests_OwnerId",
                table: "Harvests");

            migrationBuilder.DropIndex(
                name: "IX_Beehives_OwnerId",
                table: "Beehives");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Treatments");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Queens");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Inspections");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Harvests");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Beehives");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Queens",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Queens_OwnerId",
                table: "Queens",
                newName: "IX_Queens_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Queens_AspNetUsers_UserId",
                table: "Queens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
