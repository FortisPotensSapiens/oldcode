using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AleBotApi.Migrations
{
    /// <inheritdoc />
    public partial class LessonsLearnedAsGuids1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LessonsLearned",
                table: "AbUserCourses");

            migrationBuilder.AddColumn<Guid[]>(
                name: "LessonsLearnedTmp",
                table: "AbUserCourses",
                type: "uuid[]",
                nullable: false,
                defaultValue: new Guid[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LessonsLearnedTmp",
                table: "AbUserCourses");

            migrationBuilder.AddColumn<long>(
                name: "LessonsLearned",
                table: "AbUserCourses",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
