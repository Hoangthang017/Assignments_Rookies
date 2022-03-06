using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.DataAccess.Migrations
{
    public partial class fix_nameOfUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_USerTokens_Users_UserId",
                table: "USerTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_USerTokens",
                table: "USerTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.RenameTable(
                name: "USerTokens",
                newName: "UserTokens");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTokens",
                table: "UserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2022, 3, 5, 22, 18, 35, 377, DateTimeKind.Local).AddTicks(6206));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("f972b64f-6780-4657-9ae2-4bb4ba262024"),
                column: "ConcurrencyStamp",
                value: "3ed82d6f-117f-4e85-ae77-d9518b9e1e9e");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("644f5caa-4b11-44a0-af41-0fd7a8de18ee"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "7922c7d8-38b0-434f-a222-8c8283abc2de", "AQAAAAEAACcQAAAAELnXbSJpZmV3UO/yYoCiyIlJldlFVBAMvJcHqlGRtgHFpZmllEl22CDQM+B8Xva7cg==" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserTokens_Users_UserId",
                table: "UserTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTokens_Users_UserId",
                table: "UserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTokens",
                table: "UserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.RenameTable(
                name: "UserTokens",
                newName: "USerTokens");

            migrationBuilder.AddPrimaryKey(
                name: "PK_USerTokens",
                table: "USerTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2022, 2, 22, 15, 52, 35, 988, DateTimeKind.Local).AddTicks(5202));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("f972b64f-6780-4657-9ae2-4bb4ba262024"),
                column: "ConcurrencyStamp",
                value: "903bd230-f821-4c08-9e7f-9ff93509d07f");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("644f5caa-4b11-44a0-af41-0fd7a8de18ee"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "624a50ca-6f8e-4406-9e08-742117fe8f30", "AQAAAAEAACcQAAAAEFQIPjWIniEqa8f+PEQvbKVpte2fcWDg0IPPxythII8yvd0Q1UQ88nMJrFTkDeRfNQ==" });

            migrationBuilder.AddForeignKey(
                name: "FK_USerTokens_Users_UserId",
                table: "USerTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
