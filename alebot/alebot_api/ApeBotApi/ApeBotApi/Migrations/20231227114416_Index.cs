using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AleBotApi.Migrations
{
    /// <inheritdoc />
    public partial class Index : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AbLessons_CourseId",
                table: "AbLessons");

            migrationBuilder.DropIndex(
                name: "IX_AbLessons_Number",
                table: "AbLessons");

            migrationBuilder.CreateIndex(
                name: "IX_AbLessons_CourseId_Number",
                table: "AbLessons",
                columns: new[] { "CourseId", "Number" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AbLessons_CourseId_Number",
                table: "AbLessons");

            migrationBuilder.CreateIndex(
                name: "IX_AbLessons_CourseId",
                table: "AbLessons",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_AbLessons_Number",
                table: "AbLessons",
                column: "Number",
                unique: true);
        }
    }
}
