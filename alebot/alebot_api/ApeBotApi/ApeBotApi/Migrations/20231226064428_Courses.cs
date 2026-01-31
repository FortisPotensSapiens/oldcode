using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AleBotApi.Migrations
{
    /// <inheritdoc />
    public partial class Courses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DebitCurrencyId",
                table: "AbAccountTransactions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DebitCurrencyId1",
                table: "AbAccountTransactions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DebitFee",
                table: "AbAccountTransactions",
                type: "numeric(38,18)",
                precision: 38,
                scale: 18,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OperationDescription",
                table: "AbAccountTransactions",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AbCourses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    PhotoUrl = table.Column<string>(type: "text", nullable: false),
                    Free = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbCourses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbLessons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Number = table.Column<long>(type: "bigint", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbLessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbLessons_AbCourses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "AbCourses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbUserCourses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    LastLessonId = table.Column<Guid>(type: "uuid", nullable: true),
                    LessonsLearned = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbUserCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbUserCourses_AbCourses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "AbCourses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbUserCourses_AbLessons_LastLessonId",
                        column: x => x.LastLessonId,
                        principalTable: "AbLessons",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AbUserCourses_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbAccountTransactions_DebitCurrencyId",
                table: "AbAccountTransactions",
                column: "DebitCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_AbAccountTransactions_DebitCurrencyId1",
                table: "AbAccountTransactions",
                column: "DebitCurrencyId1");

            migrationBuilder.CreateIndex(
                name: "IX_AbLessons_CourseId",
                table: "AbLessons",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_AbUserCourses_CourseId",
                table: "AbUserCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_AbUserCourses_LastLessonId",
                table: "AbUserCourses",
                column: "LastLessonId");

            migrationBuilder.CreateIndex(
                name: "IX_AbUserCourses_UserId",
                table: "AbUserCourses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbAccountTransactions_AbCurrencies_DebitCurrencyId",
                table: "AbAccountTransactions",
                column: "DebitCurrencyId",
                principalTable: "AbCurrencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AbAccountTransactions_AbCurrencies_DebitCurrencyId1",
                table: "AbAccountTransactions",
                column: "DebitCurrencyId1",
                principalTable: "AbCurrencies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbAccountTransactions_AbCurrencies_DebitCurrencyId",
                table: "AbAccountTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_AbAccountTransactions_AbCurrencies_DebitCurrencyId1",
                table: "AbAccountTransactions");

            migrationBuilder.DropTable(
                name: "AbUserCourses");

            migrationBuilder.DropTable(
                name: "AbLessons");

            migrationBuilder.DropTable(
                name: "AbCourses");

            migrationBuilder.DropIndex(
                name: "IX_AbAccountTransactions_DebitCurrencyId",
                table: "AbAccountTransactions");

            migrationBuilder.DropIndex(
                name: "IX_AbAccountTransactions_DebitCurrencyId1",
                table: "AbAccountTransactions");

            migrationBuilder.DropColumn(
                name: "DebitCurrencyId",
                table: "AbAccountTransactions");

            migrationBuilder.DropColumn(
                name: "DebitCurrencyId1",
                table: "AbAccountTransactions");

            migrationBuilder.DropColumn(
                name: "DebitFee",
                table: "AbAccountTransactions");

            migrationBuilder.DropColumn(
                name: "OperationDescription",
                table: "AbAccountTransactions");
        }
    }
}
