using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Achievements.Domain.Migrations
{
    public partial class RenameUserField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CollectionsPassedAmount",
                table: "Users",
                newName: "CollectionTestsPassedAmount");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CollectionTestsPassedAmount",
                table: "Users",
                newName: "CollectionsPassedAmount");
        }
    }
}
