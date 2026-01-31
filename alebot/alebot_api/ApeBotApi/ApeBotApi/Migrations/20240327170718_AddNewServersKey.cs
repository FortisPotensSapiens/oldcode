using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AleBotApi.Migrations
{
    /// <inheritdoc />
    public partial class AddNewServersKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AbUserServers_UserId_ServerId_Address_Login",
                table: "AbUserServers");

            migrationBuilder.CreateIndex(
                name: "IX_AbUserServers_UserId_Password_Address_Login",
                table: "AbUserServers",
                columns: new[] { "UserId", "Password", "Address", "Login" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AbUserServers_UserId_Password_Address_Login",
                table: "AbUserServers");

            migrationBuilder.CreateIndex(
                name: "IX_AbUserServers_UserId_ServerId_Address_Login",
                table: "AbUserServers",
                columns: new[] { "UserId", "ServerId", "Address", "Login" },
                unique: true);
        }
    }
}
