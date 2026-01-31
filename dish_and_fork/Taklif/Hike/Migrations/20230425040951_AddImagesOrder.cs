using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hike.Migrations
{
    /// <inheritdoc />
    public partial class AddImagesOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMain",
                table: "MerchandiseImages");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "MerchandiseImages",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "MerchandiseImages");

            migrationBuilder.AddColumn<bool>(
                name: "IsMain",
                table: "MerchandiseImages",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
