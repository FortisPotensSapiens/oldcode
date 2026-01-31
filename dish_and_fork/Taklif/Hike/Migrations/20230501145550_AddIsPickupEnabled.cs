using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hike.Migrations
{
    /// <inheritdoc />
    public partial class AddIsPickupEnabled : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPickupEnabled",
                table: "Partners",
                type: "boolean",
                nullable: false,
                defaultValue: false);
            migrationBuilder.Sql("update  public.\"Partners\" set \"IsPickupEnabled\" = true;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPickupEnabled",
                table: "Partners");
        }
    }
}
