using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AleBotApi.Migrations
{
    /// <inheritdoc />
    public partial class AddMerches : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "AbAccountTransactions",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AbMerches",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ShortDescription = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    FullDescription = table.Column<string>(type: "text", nullable: false),
                    Photo = table.Column<byte[]>(type: "bytea", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(38,18)", precision: 38, scale: 18, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbMerches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbMerches_AbCurrencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "AbCurrencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uuid", nullable: false),
                    PaymentNetworkId = table.Column<Guid>(type: "uuid", nullable: false),
                    TradingAccount = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    PurchasedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    State = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(38,18)", precision: 38, scale: 18, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbOrders_AbCurrencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "AbCurrencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbOrders_AbPaymentNetworks_PaymentNetworkId",
                        column: x => x.PaymentNetworkId,
                        principalTable: "AbPaymentNetworks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbOrders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Hash = table.Column<byte[]>(type: "bytea", maxLength: 255, nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    ContentType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Extention = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AbMerchCourses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MerchId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbMerchCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbMerchCourses_AbCourses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "AbCourses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbMerchCourses_AbMerches_MerchId",
                        column: x => x.MerchId,
                        principalTable: "AbMerches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbMerchLicenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MerchId = table.Column<Guid>(type: "uuid", nullable: false),
                    LicenseId = table.Column<Guid>(type: "uuid", nullable: false),
                    Qty = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbMerchLicenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbMerchLicenses_AbLicenses_LicenseId",
                        column: x => x.LicenseId,
                        principalTable: "AbLicenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbMerchLicenses_AbMerches_MerchId",
                        column: x => x.MerchId,
                        principalTable: "AbMerches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbMerchServers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MerchId = table.Column<Guid>(type: "uuid", nullable: false),
                    ServerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Qty = table.Column<long>(type: "bigint", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbMerchServers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbMerchServers_AbMerches_MerchId",
                        column: x => x.MerchId,
                        principalTable: "AbMerches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbMerchServers_AbServers_ServerId",
                        column: x => x.ServerId,
                        principalTable: "AbServers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AbOrderLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    MerchId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(38,18)", precision: 38, scale: 18, nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConcurrencyToken = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbOrderLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AbOrderLines_AbCurrencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "AbCurrencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbOrderLines_AbMerches_MerchId",
                        column: x => x.MerchId,
                        principalTable: "AbMerches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AbOrderLines_AbOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "AbOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbMerchCourses_CourseId",
                table: "AbMerchCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_AbMerchCourses_MerchId_CourseId",
                table: "AbMerchCourses",
                columns: new[] { "MerchId", "CourseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbMerchLicenses_LicenseId",
                table: "AbMerchLicenses",
                column: "LicenseId");

            migrationBuilder.CreateIndex(
                name: "IX_AbMerchLicenses_MerchId_LicenseId",
                table: "AbMerchLicenses",
                columns: new[] { "MerchId", "LicenseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbMerchServers_MerchId_ServerId",
                table: "AbMerchServers",
                columns: new[] { "MerchId", "ServerId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbMerchServers_ServerId",
                table: "AbMerchServers",
                column: "ServerId");

            migrationBuilder.CreateIndex(
                name: "IX_AbMerches_CurrencyId",
                table: "AbMerches",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_AbOrderLines_CurrencyId",
                table: "AbOrderLines",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_AbOrderLines_MerchId",
                table: "AbOrderLines",
                column: "MerchId");

            migrationBuilder.CreateIndex(
                name: "IX_AbOrderLines_OrderId_MerchId",
                table: "AbOrderLines",
                columns: new[] { "OrderId", "MerchId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbOrders_CurrencyId",
                table: "AbOrders",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_AbOrders_PaymentNetworkId",
                table: "AbOrders",
                column: "PaymentNetworkId");

            migrationBuilder.CreateIndex(
                name: "IX_AbOrders_UserId",
                table: "AbOrders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_Hash",
                table: "Files",
                column: "Hash",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbMerchCourses");

            migrationBuilder.DropTable(
                name: "AbMerchLicenses");

            migrationBuilder.DropTable(
                name: "AbMerchServers");

            migrationBuilder.DropTable(
                name: "AbOrderLines");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "AbMerches");

            migrationBuilder.DropTable(
                name: "AbOrders");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "AbAccountTransactions");
        }
    }
}
