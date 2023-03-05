using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEnglishLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EnglishLevel",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "EnglishLevel", "PasswordHash", "SecurityStamp" },
                values: new object[] { "427df4b5-1ef8-46ca-8ca3-9f545792a47d", 0, "AQAAAAIAAYagAAAAEDkQKVZlBlST4w0Vbh570tGQTDjcZTqU5v7SGA86c5mmAOioUBOsYHyPaUSMtBb+uQ==", "e3c3ed39-2958-4813-b035-9327d9096db6" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnglishLevel",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d52dd528-04f3-4222-8149-aee076e9ef22", "AQAAAAIAAYagAAAAEGlsdsVM4MyHGTn8ywneGngD4ulMEPVjwiO2p/6RlGT5kEV6xXzsfjN5vYaR8H/jRQ==", "d8a5e059-7b34-461b-bfc3-2bde84283fd3" });
        }
    }
}
