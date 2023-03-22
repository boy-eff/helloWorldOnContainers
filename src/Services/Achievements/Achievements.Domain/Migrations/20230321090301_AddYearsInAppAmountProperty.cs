﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Achievements.Domain.Migrations
{
    public partial class AddYearsInAppAmountProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "YearsInAppAmount",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YearsInAppAmount",
                table: "Users");
        }
    }
}
