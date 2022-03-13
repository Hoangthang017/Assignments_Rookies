using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.DataAccess.Migrations
{
    public partial class addFeaturedProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsShowOnHome",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2022, 3, 13, 16, 37, 11, 652, DateTimeKind.Local).AddTicks(3747));

            migrationBuilder.UpdateData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2022, 3, 13, 16, 37, 11, 652, DateTimeKind.Local).AddTicks(3762));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2022, 3, 13, 16, 37, 11, 635, DateTimeKind.Local).AddTicks(2755));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("16a33b7f-8765-4e91-8c8d-c8a2c979a9cd"),
                column: "ConcurrencyStamp",
                value: "4662c8db-46b7-40fa-a69c-e4857c332f8d");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("f972b64f-6780-4657-9ae2-4bb4ba262024"),
                column: "ConcurrencyStamp",
                value: "ff423e57-9f29-4f39-ad8b-df3400221513");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("644f5caa-4b11-44a0-af41-0fd7a8de18ee"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "853c74d6-da1e-4fe5-a62e-fe0c19b80e31", "AQAAAAEAACcQAAAAENwLNIoP3WtGdV42UVN0fx283y31H0AQaMkqmhwtRWn8cRSqWtHeiriZQprxrgj2ng==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("dd9efc6a-1ca0-4e0b-9362-5fb185558a33"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "d46f5475-92a7-423b-b61c-a9f591913841", "AQAAAAEAACcQAAAAEDF3BOrI+dp0rZyTHGHquHB7c3ZBAAK00SiGRNI3o06pgK0zECyqf99bbGTAlfnZ7w==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsShowOnHome",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2022, 3, 13, 12, 52, 27, 326, DateTimeKind.Local).AddTicks(3294));

            migrationBuilder.UpdateData(
                table: "Images",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2022, 3, 13, 12, 52, 27, 326, DateTimeKind.Local).AddTicks(3305));

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2022, 3, 13, 12, 52, 27, 310, DateTimeKind.Local).AddTicks(9351));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("16a33b7f-8765-4e91-8c8d-c8a2c979a9cd"),
                column: "ConcurrencyStamp",
                value: "3bc3ea66-e325-46ca-b9e7-a951f662a5b0");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("f972b64f-6780-4657-9ae2-4bb4ba262024"),
                column: "ConcurrencyStamp",
                value: "eb7af200-887c-49b0-9b22-29bc33c2a829");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("644f5caa-4b11-44a0-af41-0fd7a8de18ee"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "758961a7-f282-4bcc-b72f-1911f8c8bf89", "AQAAAAEAACcQAAAAENqwxma3zXLr7dgJihO+s9hfjas7rntKYewrhUyYszOfLgLXqQS4av+D6WrgRthWGA==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("dd9efc6a-1ca0-4e0b-9362-5fb185558a33"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "bce580a6-2c5f-4549-9119-d57522bc62a2", "AQAAAAEAACcQAAAAELWUY0yFOmOb4GvcjAwwvZcJ+0b35bhalXmYw5BsJnNQ+azYxDtXw2ct4fN9x0p9jw==" });
        }
    }
}
