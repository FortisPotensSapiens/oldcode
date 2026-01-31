using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hike.Migrations
{
    /// <inheritdoc />
    public partial class AddSelectedOrderAndSelectedOfferToApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "ApplicationOffers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationOffers_OrderId",
                table: "ApplicationOffers",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationOffers_Orders_OrderId",
                table: "ApplicationOffers",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationOffers_Orders_OrderId",
                table: "ApplicationOffers");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationOffers_OrderId",
                table: "ApplicationOffers");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "ApplicationOffers");
        }
    }
}
