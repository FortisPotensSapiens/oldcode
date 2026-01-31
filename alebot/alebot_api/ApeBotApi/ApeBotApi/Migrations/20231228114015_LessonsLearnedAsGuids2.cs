using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AleBotApi.Migrations
{
    /// <inheritdoc />
    public partial class LessonsLearnedAsGuids2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LessonsLearnedTmp",
                table: "AbUserCourses",
                newName: "LessonsLearned");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LessonsLearned",
                table: "AbUserCourses",
                newName: "LessonsLearnedTmp");
        }
    }
}
