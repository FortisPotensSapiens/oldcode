using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AleBotApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangeReferrer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_RefereId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RefereId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RefereId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "AbCourses");

            migrationBuilder.AddColumn<byte[]>(
                name: "Photo",
                table: "AbCourses",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RefererId",
                table: "AspNetUsers",
                column: "RefererId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_RefererId",
                table: "AspNetUsers",
                column: "RefererId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_RefererId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RefererId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "AbCourses");

            migrationBuilder.AddColumn<Guid>(
                name: "RefereId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "AbCourses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RefereId",
                table: "AspNetUsers",
                column: "RefereId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_RefereId",
                table: "AspNetUsers",
                column: "RefereId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
