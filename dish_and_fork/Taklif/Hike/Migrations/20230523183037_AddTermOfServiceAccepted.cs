using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hike.Migrations
{
    /// <inheritdoc />
    public partial class AddTermOfServiceAccepted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AcceptedConsentToMailings",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AcceptedConsentToPersonalDataProcessing",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AcceptedPivacyPolicy",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptedConsentToMailings",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AcceptedConsentToPersonalDataProcessing",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AcceptedPivacyPolicy",
                table: "AspNetUsers");
        }
    }
}
