using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AleBotApi.Migrations
{
    /// <inheritdoc />
    public partial class AddNavigationProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbAccountTransactions_AbCurrencies_DebitCurrencyId1",
                table: "AbAccountTransactions");

            migrationBuilder.DropIndex(
                name: "IX_AbOrderLines_MerchId",
                table: "AbOrderLines");

            migrationBuilder.DropIndex(
                name: "IX_AbOrderLines_OrderId_MerchId",
                table: "AbOrderLines");

            migrationBuilder.DropIndex(
                name: "IX_AbLessons_CourseId_Number",
                table: "AbLessons");

            migrationBuilder.DropIndex(
                name: "IX_AbAccounts_UserId_CurrencyId",
                table: "AbAccounts");

            migrationBuilder.DropIndex(
                name: "IX_AbAccountTransactions_DebitCurrencyId1",
                table: "AbAccountTransactions");

            migrationBuilder.DropColumn(
                name: "DebitCurrencyId1",
                table: "AbAccountTransactions");

            migrationBuilder.CreateIndex(
                name: "IX_AbOrders_ExternalId",
                table: "AbOrders",
                column: "ExternalId");

            migrationBuilder.CreateIndex(
                name: "IX_AbOrderLines_MerchId_OrderId",
                table: "AbOrderLines",
                columns: new[] { "MerchId", "OrderId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbOrderLines_OrderId",
                table: "AbOrderLines",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_AbMerches_PaymentNetworkId",
                table: "AbMerches",
                column: "PaymentNetworkId");

            migrationBuilder.CreateIndex(
                name: "IX_AbLessons_CourseId",
                table: "AbLessons",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_AbLessons_Number_CourseId",
                table: "AbLessons",
                columns: new[] { "Number", "CourseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbAccounts_UserId",
                table: "AbAccounts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AbAccountTransactions_ByReferalId",
                table: "AbAccountTransactions",
                column: "ByReferalId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbAccountTransactions_AspNetUsers_ByReferalId",
                table: "AbAccountTransactions",
                column: "ByReferalId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AbAccounts_AspNetUsers_UserId",
                table: "AbAccounts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AbMerches_AbPaymentNetworks_PaymentNetworkId",
                table: "AbMerches",
                column: "PaymentNetworkId",
                principalTable: "AbPaymentNetworks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbAccountTransactions_AspNetUsers_ByReferalId",
                table: "AbAccountTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_AbAccounts_AspNetUsers_UserId",
                table: "AbAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_AbMerches_AbPaymentNetworks_PaymentNetworkId",
                table: "AbMerches");

            migrationBuilder.DropIndex(
                name: "IX_AbOrders_ExternalId",
                table: "AbOrders");

            migrationBuilder.DropIndex(
                name: "IX_AbOrderLines_MerchId_OrderId",
                table: "AbOrderLines");

            migrationBuilder.DropIndex(
                name: "IX_AbOrderLines_OrderId",
                table: "AbOrderLines");

            migrationBuilder.DropIndex(
                name: "IX_AbMerches_PaymentNetworkId",
                table: "AbMerches");

            migrationBuilder.DropIndex(
                name: "IX_AbLessons_CourseId",
                table: "AbLessons");

            migrationBuilder.DropIndex(
                name: "IX_AbLessons_Number_CourseId",
                table: "AbLessons");

            migrationBuilder.DropIndex(
                name: "IX_AbAccounts_UserId",
                table: "AbAccounts");

            migrationBuilder.DropIndex(
                name: "IX_AbAccountTransactions_ByReferalId",
                table: "AbAccountTransactions");

            migrationBuilder.AddColumn<Guid>(
                name: "DebitCurrencyId1",
                table: "AbAccountTransactions",
                type: "uuid",
                nullable: true);

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
                name: "IX_AbLessons_CourseId_Number",
                table: "AbLessons",
                columns: new[] { "CourseId", "Number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbAccounts_UserId_CurrencyId",
                table: "AbAccounts",
                columns: new[] { "UserId", "CurrencyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbAccountTransactions_DebitCurrencyId1",
                table: "AbAccountTransactions",
                column: "DebitCurrencyId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AbAccountTransactions_AbCurrencies_DebitCurrencyId1",
                table: "AbAccountTransactions",
                column: "DebitCurrencyId1",
                principalTable: "AbCurrencies",
                principalColumn: "Id");
        }
    }
}
