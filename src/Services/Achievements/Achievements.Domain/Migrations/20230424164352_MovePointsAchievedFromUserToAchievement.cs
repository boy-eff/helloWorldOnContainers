using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Achievements.Domain.Migrations
{
    public partial class MovePointsAchievedFromUserToAchievement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CollectionTestsPassedAmount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CollectionsCreatedAmount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "WordsInDictionaryAmount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "YearsInAppAmount",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "CurrentLevel",
                table: "UsersAchievements",
                newName: "PointsAchieved");

            migrationBuilder.AddColumn<int>(
                name: "NextLevelId",
                table: "UsersAchievements",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UsersAchievements_AchievementId_NextLevelId",
                table: "UsersAchievements",
                columns: new[] { "AchievementId", "NextLevelId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UsersAchievements_AchievementLevels_AchievementId_NextLevel~",
                table: "UsersAchievements",
                columns: new[] { "AchievementId", "NextLevelId" },
                principalTable: "AchievementLevels",
                principalColumns: new[] { "AchievementId", "Level" },
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersAchievements_AchievementLevels_AchievementId_NextLevel~",
                table: "UsersAchievements");

            migrationBuilder.DropIndex(
                name: "IX_UsersAchievements_AchievementId_NextLevelId",
                table: "UsersAchievements");

            migrationBuilder.DropColumn(
                name: "NextLevelId",
                table: "UsersAchievements");

            migrationBuilder.RenameColumn(
                name: "PointsAchieved",
                table: "UsersAchievements",
                newName: "CurrentLevel");

            migrationBuilder.AddColumn<int>(
                name: "CollectionTestsPassedAmount",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CollectionsCreatedAmount",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WordsInDictionaryAmount",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "YearsInAppAmount",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
