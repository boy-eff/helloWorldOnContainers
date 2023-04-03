using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Words.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddModerationStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActualModerationStatus",
                table: "Collections",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "ModerationStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModerationStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WordCollectionModerations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WordCollectionId = table.Column<int>(type: "int", nullable: false),
                    ModerationStatusId = table.Column<int>(type: "int", nullable: false),
                    ModeratorId = table.Column<int>(type: "int", nullable: false),
                    Review = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordCollectionModerations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordCollectionModerations_Collections_WordCollectionId",
                        column: x => x.WordCollectionId,
                        principalTable: "Collections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WordCollectionModerations_ModerationStatuses_ModerationStatusId",
                        column: x => x.ModerationStatusId,
                        principalTable: "ModerationStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WordCollectionModerations_Users_ModeratorId",
                        column: x => x.ModeratorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "ModerationStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Pending" },
                    { 2, "Accepted" },
                    { 3, "Rejected" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_WordCollectionModerations_ModerationStatusId",
                table: "WordCollectionModerations",
                column: "ModerationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_WordCollectionModerations_ModeratorId",
                table: "WordCollectionModerations",
                column: "ModeratorId");

            migrationBuilder.CreateIndex(
                name: "IX_WordCollectionModerations_WordCollectionId",
                table: "WordCollectionModerations",
                column: "WordCollectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WordCollectionModerations");

            migrationBuilder.DropTable(
                name: "ModerationStatuses");

            migrationBuilder.DropColumn(
                name: "ActualModerationStatus",
                table: "Collections");
        }
    }
}
