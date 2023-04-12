using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Words.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddPhotoToUserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoPublicId",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoPublicId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "Users");
        }
    }
}
