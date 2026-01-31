using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AleBotApi.Migrations
{
    /// <inheritdoc />
    public partial class CoursesConstr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AbUserCourses_UserId",
                table: "AbUserCourses");

            migrationBuilder.CreateIndex(
                name: "IX_AbUserCourses_UserId_CourseId",
                table: "AbUserCourses",
                columns: new[] { "UserId", "CourseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbLessons_Number",
                table: "AbLessons",
                column: "Number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AbUserCourses_UserId_CourseId",
                table: "AbUserCourses");

            migrationBuilder.DropIndex(
                name: "IX_AbLessons_Number",
                table: "AbLessons");

            migrationBuilder.CreateIndex(
                name: "IX_AbUserCourses_UserId",
                table: "AbUserCourses",
                column: "UserId");
        }
    }
}
