using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "d52dd528-04f3-4222-8149-aee076e9ef22", null, "SUPERADMIN", "AQAAAAIAAYagAAAAEGlsdsVM4MyHGTn8ywneGngD4ulMEPVjwiO2p/6RlGT5kEV6xXzsfjN5vYaR8H/jRQ==", "d8a5e059-7b34-461b-bfc3-2bde84283fd3", "superadmin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "SecurityStamp", "UserName" },
                values: new object[] { "69a02226-48e8-4511-9251-8ebf3eb31303", "ADMIN", null, null, null, "admin" });
        }
    }
}
