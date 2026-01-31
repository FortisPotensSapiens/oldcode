using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hike.Migrations
{
    /// <inheritdoc />
    public partial class AddNotNullIndexToExternalIdInPaymentSystemForPartner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Partners_ExternalId",
                table: "Partners");

            migrationBuilder.CreateIndex(
                name: "IX_Partners_ExternalId",
                table: "Partners",
                column: "ExternalId",
                unique: true,
                filter: "\"ExternalId\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Partners_ExternalId",
                table: "Partners");

            migrationBuilder.CreateIndex(
                name: "IX_Partners_ExternalId",
                table: "Partners",
                column: "ExternalId",
                unique: true);
        }
    }
}
