using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hike.Migrations
{
    /// <inheritdoc />
    public partial class AddExternalIdInDeliverySystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalIdInDeliverySystem",
                table: "Orders",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExternalIdInDeliverySystem",
                table: "Orders");
        }
    }
}
