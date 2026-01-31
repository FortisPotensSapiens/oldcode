using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AleBotApi.Migrations
{
    /// <inheritdoc />
    public partial class AddUserServerExternalId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "AbUserServers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ExtendedUserServerId",
                table: "AbOrderLines",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbOrderLines_ExtendedUserServerId",
                table: "AbOrderLines",
                column: "ExtendedUserServerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AbOrderLines_AbUserServers_ExtendedUserServerId",
                table: "AbOrderLines",
                column: "ExtendedUserServerId",
                principalTable: "AbUserServers",
                principalColumn: "Id");

            //Seed data ------------------------------------------------
            migrationBuilder.UpdateData(
                  table: "AbUserServers",
                  column: "ExternalId",
                  value: "15",
                  keyColumn: "Id",
                  keyValue: "36812cae-6a61-4d33-83c6-7b3a2ecf3a03"
                  );
            migrationBuilder.UpdateData(
                 table: "AbUserServers",
                 column: "ExternalId",
                 value: "13",
                 keyColumn: "Id",
                 keyValue: "2ff91fd8-2836-40f7-90cb-d507d1ae096c"
                 );
            migrationBuilder.UpdateData(
                 table: "AbUserServers",
                 column: "ExternalId",
                 value: "14",
                 keyColumn: "Id",
                 keyValue: "055d91e9-d1fd-4504-8f3f-7a0a668b5abc"
                 );
            migrationBuilder.UpdateData(
                 table: "AbUserServers",
                 column: "ExternalId",
                 value: "16",
                 keyColumn: "Id",
                 keyValue: "aaf3c9b1-d16c-416c-83bb-26a01efcc783"
                 );
            migrationBuilder.UpdateData(
     table: "AbUserServers",
     column: "ExternalId",
     value: "1",
     keyColumn: "Id",
     keyValue: "0a08a7b4-cdd5-4330-9cad-725739544485"
     );
            migrationBuilder.UpdateData(
     table: "AbUserServers",
     column: "ExternalId",
     value: "2",
     keyColumn: "Id",
     keyValue: "0a08a7b4-cdd5-4330-9cad-725739544d85"
     );
            migrationBuilder.UpdateData(
     table: "AbUserServers",
     column: "ExternalId",
     value: "4",
     keyColumn: "Id",
     keyValue: "0e7d294c-4e30-4a26-9fc0-0b74796f1ca6"
     );
            migrationBuilder.UpdateData(
     table: "AbUserServers",
     column: "ExternalId",
     value: "5",
     keyColumn: "Id",
     keyValue: "fcf28ebf-cb8c-47d1-b9b1-95d9ff70b1c9"
     );
            migrationBuilder.UpdateData(
     table: "AbUserServers",
     column: "ExternalId",
     value: "6",
     keyColumn: "Id",
     keyValue: "38c1337c-9ccb-4bfc-87c7-4257a7ba5983"
     );
            migrationBuilder.UpdateData(
     table: "AbUserServers",
     column: "ExternalId",
     value: "7",
     keyColumn: "Id",
     keyValue: "818d0c82-ac84-4ac6-9bac-b2166c1ca8fe"
     );
            migrationBuilder.UpdateData(
     table: "AbUserServers",
     column: "ExternalId",
     value: "8",
     keyColumn: "Id",
     keyValue: "b7037e68-fb23-4fd4-9725-7f949999a2ff"
     );
            migrationBuilder.UpdateData(
     table: "AbUserServers",
     column: "ExternalId",
     value: "9",
     keyColumn: "Id",
     keyValue: "04b3b637-a074-4f32-b4ec-c0ead0decb1f"
     );
            migrationBuilder.UpdateData(
table: "AbUserServers",
column: "ExternalId",
value: "10",
keyColumn: "Id",
keyValue: "b63cbf89-65c6-4c91-a921-2858f871259f"
);
            migrationBuilder.UpdateData(
table: "AbUserServers",
column: "ExternalId",
value: "11",
keyColumn: "Id",
keyValue: "a5e8f9a9-b171-4dda-aac7-ce7f36b09468"
);
            migrationBuilder.UpdateData(
table: "AbUserServers",
column: "ExternalId",
value: "12",
keyColumn: "Id",
keyValue: "5aca72bc-fa81-4d72-844e-be5bc8f5b07a"
);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AbOrderLines_AbUserServers_ExtendedUserServerId",
                table: "AbOrderLines");

            migrationBuilder.DropIndex(
                name: "IX_AbOrderLines_ExtendedUserServerId",
                table: "AbOrderLines");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "AbUserServers");

            migrationBuilder.DropColumn(
                name: "ExtendedUserServerId",
                table: "AbOrderLines");
        }
    }
}
