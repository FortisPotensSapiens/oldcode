using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AleBotApi.Migrations
{
    /// <inheritdoc />
    public partial class AddSeverExtentions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "AbUserServers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrderLineId",
                table: "AbUserServers",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrderLineId",
                table: "AbUserLicenses",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrderLineId",
                table: "AbUserCourses",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ServerDurationInMonth",
                table: "AbServers",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AbServersExtention",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ServerDurationInMonth = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbServersExtention", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbMerchServerExtentions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MerchId = table.Column<Guid>(type: "uuid", nullable: false),
                    ServerExtentionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Qty = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbMerchServerExtentions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbMerchServerExtentions_AbMerches_MerchId",
                        column: x => x.MerchId,
                        principalTable: "AbMerches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbMerchServerExtentions_AbServersExtention_ServerExtentionId",
                        column: x => x.ServerExtentionId,
                        principalTable: "AbServersExtention",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbUserServers_OrderLineId",
                table: "AbUserServers",
                column: "OrderLineId");

            migrationBuilder.CreateIndex(
                name: "IX_AbUserLicenses_OrderLineId",
                table: "AbUserLicenses",
                column: "OrderLineId");

            migrationBuilder.CreateIndex(
                name: "IX_AbUserCourses_OrderLineId",
                table: "AbUserCourses",
                column: "OrderLineId");

            migrationBuilder.CreateIndex(
                name: "IX_AbMerchServerExtentions_MerchId_ServerExtentionId",
                table: "AbMerchServerExtentions",
                columns: new[] { "MerchId", "ServerExtentionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbMerchServerExtentions_ServerExtentionId",
                table: "AbMerchServerExtentions",
                column: "ServerExtentionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbUserCourses_AbOrderLines_OrderLineId",
                table: "AbUserCourses",
                column: "OrderLineId",
                principalTable: "AbOrderLines",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AbUserLicenses_AbOrderLines_OrderLineId",
                table: "AbUserLicenses",
                column: "OrderLineId",
                principalTable: "AbOrderLines",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AbUserServers_AbOrderLines_OrderLineId",
                table: "AbUserServers",
                column: "OrderLineId",
                principalTable: "AbOrderLines",
                principalColumn: "Id");

            //Seed Data -----------------------------------------------------------------
            migrationBuilder.UpdateData(
                table: "AbUserServers",
                column: "ExpirationDate",
                value: new DateTime(new DateOnly(2025, 2, 15), new TimeOnly(6, 6), DateTimeKind.Utc),
                keyColumn: "ExpirationDate",
                keyValue: null
                );
            migrationBuilder.UpdateData(
              table: "AbServers",
              column: "ServerDurationInMonth",
              value: (uint)1,
              keyColumn: "Name",
              keyValue: "Trial 1 месяц"
              );
            migrationBuilder.UpdateData(
              table: "AbServers",
              column: "ServerDurationInMonth",
              value: (uint)3,
              keyColumn: "Name",
              keyValue: "Invest 3 месяца"
              );
            migrationBuilder.UpdateData(
              table: "AbServers",
              column: "ServerDurationInMonth",
              value: (uint)6,
              keyColumn: "Name",
              keyValue: "Worker 6 месяцев"
              );
            migrationBuilder.UpdateData(
              table: "AbServers",
              column: "ServerDurationInMonth",
              value: (uint)12,
              keyColumn: "Name",
              keyValue: "VIP 12 месяцев"
              );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbUserCourses_AbOrderLines_OrderLineId",
                table: "AbUserCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_AbUserLicenses_AbOrderLines_OrderLineId",
                table: "AbUserLicenses");

            migrationBuilder.DropForeignKey(
                name: "FK_AbUserServers_AbOrderLines_OrderLineId",
                table: "AbUserServers");

            migrationBuilder.DropTable(
                name: "AbMerchServerExtentions");

            migrationBuilder.DropTable(
                name: "AbServersExtention");

            migrationBuilder.DropIndex(
                name: "IX_AbUserServers_OrderLineId",
                table: "AbUserServers");

            migrationBuilder.DropIndex(
                name: "IX_AbUserLicenses_OrderLineId",
                table: "AbUserLicenses");

            migrationBuilder.DropIndex(
                name: "IX_AbUserCourses_OrderLineId",
                table: "AbUserCourses");

            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "AbUserServers");

            migrationBuilder.DropColumn(
                name: "OrderLineId",
                table: "AbUserServers");

            migrationBuilder.DropColumn(
                name: "OrderLineId",
                table: "AbUserLicenses");

            migrationBuilder.DropColumn(
                name: "OrderLineId",
                table: "AbUserCourses");

            migrationBuilder.DropColumn(
                name: "ServerDurationInMonth",
                table: "AbServers");
        }
    }
}
