using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Words.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddWordToTestQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Word",
                table: "WordCollectionTestQuestions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Word",
                table: "WordCollectionTestQuestions");
        }
    }
}
