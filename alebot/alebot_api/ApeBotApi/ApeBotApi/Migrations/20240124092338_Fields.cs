using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AleBotApi.Migrations
{
    /// <inheritdoc />
    public partial class Fields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "AbAccountTransactions");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PaymentNetworkId",
                table: "AbMerches",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PaymentNetworkId",
                table: "AbMerches");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "AbAccountTransactions",
                type: "uuid",
                nullable: true);
        }
    }
}
