using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hike.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDeviceIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Devices_FcmPushToken",
                table: "Devices");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_FcmPushToken_UserId",
                table: "Devices",
                columns: new[] { "FcmPushToken", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Devices_FcmPushToken_UserId",
                table: "Devices");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_FcmPushToken",
                table: "Devices",
                column: "FcmPushToken",
                unique: true);
        }
    }
}
