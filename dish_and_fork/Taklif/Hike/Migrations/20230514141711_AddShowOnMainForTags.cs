using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hike.Migrations
{
    /// <inheritdoc />
    public partial class AddShowOnMainForTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowOnMainPage",
                table: "Categories",
                type: "boolean",
                nullable: false,
                defaultValue: false);
            migrationBuilder.Sql("update \"Categories\" set \"ShowOnMainPage\" = true;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowOnMainPage",
                table: "Categories");
        }
    }
}
