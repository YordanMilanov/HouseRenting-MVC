using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HouseRentingSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixCreatedOnValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("4d6e626c-0b2c-48a4-b6af-b34681cbdfe5"));

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("5ee065b2-6d8d-44c0-a537-bd046f9b411d"));

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("f8c911d5-c4aa-40df-881e-c8a5ee3be014"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Houses",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 10, 6, 10, 32, 57, 271, DateTimeKind.Utc).AddTicks(5281));

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "AgentId", "CategoryId", "Description", "ImageUrl", "PricePerMonth", "RenterId", "Title" },
                values: new object[,]
                {
                    { new Guid("3dc27563-504c-44bb-b355-15701a2e031c"), "North London, UK (near the border)", new Guid("8f2ce198-f478-4b6f-8ea0-b72856556540"), 3, "A big house for your whole family. Don't miss to buy a house with three bedrooms.", "https://www.luxury-architecture.net/wpcontent/uploads/2017/12/1513217889-7597-FAIRWAYS-010.jpg", 2100.00m, new Guid("3445ac97-a746-4374-f235-08dbc64ed13d"), "Big House Marina" },
                    { new Guid("6e997907-b25f-4d57-b2ac-c70d7b2fe748"), "Near the Sea Garden in Burgas, Bulgaria", new Guid("8f2ce198-f478-4b6f-8ea0-b72856556540"), 2, "It has the best comfort you will ever ask for. With two bedrooms,it is great for your family.", "https://www.google.com/imgres?imgurl=https%3A%2F%2Fimg.freepik.com%2Fpremium-photo%2Fluxury-beautiful-house_967158-73.jpg&tbnid=_Wk0V3qZo5ARNM&vet=12ahUKEwjD9Z7-leGBAxUVwwIHHQq6C-0QMygBegQIARB2..i&imgrefurl=https%3A%2F%2Fwww.freepik.com%2Ffree-photos-vectors%2Fbeautiful-house&docid=0MLiyCk8BDweCM&w=626&h=626&q=beautiful%20house&ved=2ahUKEwjD9Z7-leGBAxUVwwIHHQq6C-0QMygBegQIARB2", 1200.00m, null, "Family House Comfort" },
                    { new Guid("96b3abe7-8451-4d9e-8636-419ab325d91f"), "Boyana Neighbourhood, Sofia, Bulgaria", new Guid("8f2ce198-f478-4b6f-8ea0-b72856556540"), 2, "This luxurious house is everything you will need. It is just excellent.", "https://i.pinimg.com/originals/a6/f5/85/a6f5850a77633c56e4e4ac4f867e3c00.jpg", 2000.00m, null, "Grand House" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("3dc27563-504c-44bb-b355-15701a2e031c"));

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("6e997907-b25f-4d57-b2ac-c70d7b2fe748"));

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("96b3abe7-8451-4d9e-8636-419ab325d91f"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "Houses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 10, 6, 10, 32, 57, 271, DateTimeKind.Utc).AddTicks(5281),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "AgentId", "CategoryId", "Description", "ImageUrl", "PricePerMonth", "RenterId", "Title" },
                values: new object[,]
                {
                    { new Guid("4d6e626c-0b2c-48a4-b6af-b34681cbdfe5"), "North London, UK (near the border)", new Guid("8f2ce198-f478-4b6f-8ea0-b72856556540"), 3, "A big house for your whole family. Don't miss to buy a house with three bedrooms.", "https://www.luxury-architecture.net/wpcontent/uploads/2017/12/1513217889-7597-FAIRWAYS-010.jpg", 2100.00m, new Guid("3445ac97-a746-4374-f235-08dbc64ed13d"), "Big House Marina" },
                    { new Guid("5ee065b2-6d8d-44c0-a537-bd046f9b411d"), "Boyana Neighbourhood, Sofia, Bulgaria", new Guid("8f2ce198-f478-4b6f-8ea0-b72856556540"), 2, "This luxurious house is everything you will need. It is just excellent.", "https://i.pinimg.com/originals/a6/f5/85/a6f5850a77633c56e4e4ac4f867e3c00.jpg", 2000.00m, null, "Grand House" },
                    { new Guid("f8c911d5-c4aa-40df-881e-c8a5ee3be014"), "Near the Sea Garden in Burgas, Bulgaria", new Guid("8f2ce198-f478-4b6f-8ea0-b72856556540"), 2, "It has the best comfort you will ever ask for. With two bedrooms,it is great for your family.", "https://www.google.com/imgres?imgurl=https%3A%2F%2Fimg.freepik.com%2Fpremium-photo%2Fluxury-beautiful-house_967158-73.jpg&tbnid=_Wk0V3qZo5ARNM&vet=12ahUKEwjD9Z7-leGBAxUVwwIHHQq6C-0QMygBegQIARB2..i&imgrefurl=https%3A%2F%2Fwww.freepik.com%2Ffree-photos-vectors%2Fbeautiful-house&docid=0MLiyCk8BDweCM&w=626&h=626&q=beautiful%20house&ved=2ahUKEwjD9Z7-leGBAxUVwwIHHQq6C-0QMygBegQIARB2", 1200.00m, null, "Family House Comfort" }
                });
        }
    }
}
