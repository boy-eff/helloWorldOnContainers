﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Words.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsCorrectProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCorrect",
                table: "WordCollectionTestQuestions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCorrect",
                table: "WordCollectionTestQuestions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
