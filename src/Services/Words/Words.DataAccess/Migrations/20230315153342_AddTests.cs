using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Words.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddTests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WordCollectionTestPassInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    WordCollectionId = table.Column<int>(type: "int", nullable: false),
                    TotalQuestions = table.Column<int>(type: "int", nullable: false),
                    CorrectAnswersAmount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordCollectionTestPassInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordCollectionTestPassInformation_Collections_WordCollectionId",
                        column: x => x.WordCollectionId,
                        principalTable: "Collections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WordCollectionTestPassInformation_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WordCollectionTestQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WordCollectionTestPassInformationId = table.Column<int>(type: "int", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordCollectionTestQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordCollectionTestQuestions_WordCollectionTestPassInformation_WordCollectionTestPassInformationId",
                        column: x => x.WordCollectionTestPassInformationId,
                        principalTable: "WordCollectionTestPassInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WordCollectionTestPassInformation_UserId",
                table: "WordCollectionTestPassInformation",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WordCollectionTestPassInformation_WordCollectionId",
                table: "WordCollectionTestPassInformation",
                column: "WordCollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_WordCollectionTestQuestions_WordCollectionTestPassInformationId",
                table: "WordCollectionTestQuestions",
                column: "WordCollectionTestPassInformationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WordCollectionTestQuestions");

            migrationBuilder.DropTable(
                name: "WordCollectionTestPassInformation");
        }
    }
}
