using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEnglishLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnglishLevel",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "76d42689-fbb9-4f56-8439-5b33b4f5c0c7", "AQAAAAIAAYagAAAAEDcURaxnY1SCg/aS8c0XKgtSnpxvb5zXQ/ocVxLK1+1JQ4mw/MpKEHmmm5VvBneIuw==", "4da2fd4a-c88e-4462-a46a-cf64a1488fba" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                values: new object[] { "46c24903-348b-486d-a0ef-3b4fb570de39", 0, "AQAAAAIAAYagAAAAEMkyQZ2mSpHNU0igWYFsX5ZO6yiVmGt5GQ7Zw/YS9QxRLMpeTopsQDWVKUN3kEMDIw==", "9913a21d-4467-444a-998e-55335bdbb836" });
        }
    }
}
