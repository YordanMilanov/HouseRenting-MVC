using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HouseRentingSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveColumnForSoftDeletingHouse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Houses",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "AgentId", "CategoryId", "Description", "ImageUrl", "PricePerMonth", "RenterId", "Title" },
                values: new object[,]
                {
                    { new Guid("2fd87338-5a8c-43e9-ac0c-d57a8cc11cef"), "Near the Sea Garden in Burgas, Bulgaria", new Guid("8f2ce198-f478-4b6f-8ea0-b72856556540"), 2, "It has the best comfort you will ever ask for. With two bedrooms,it is great for your family.", "https://www.google.com/imgres?imgurl=https%3A%2F%2Fimg.freepik.com%2Fpremium-photo%2Fluxury-beautiful-house_967158-73.jpg&tbnid=_Wk0V3qZo5ARNM&vet=12ahUKEwjD9Z7-leGBAxUVwwIHHQq6C-0QMygBegQIARB2..i&imgrefurl=https%3A%2F%2Fwww.freepik.com%2Ffree-photos-vectors%2Fbeautiful-house&docid=0MLiyCk8BDweCM&w=626&h=626&q=beautiful%20house&ved=2ahUKEwjD9Z7-leGBAxUVwwIHHQq6C-0QMygBegQIARB2", 1200.00m, null, "Family House Comfort" },
                    { new Guid("4358f541-e6e8-4760-98a8-8a84a3e10564"), "Boyana Neighbourhood, Sofia, Bulgaria", new Guid("8f2ce198-f478-4b6f-8ea0-b72856556540"), 2, "This luxurious house is everything you will need. It is just excellent.", "https://i.pinimg.com/originals/a6/f5/85/a6f5850a77633c56e4e4ac4f867e3c00.jpg", 2000.00m, null, "Grand House" },
                    { new Guid("eb961987-1c23-4609-9de7-f43cd1fa56cd"), "North London, UK (near the border)", new Guid("8f2ce198-f478-4b6f-8ea0-b72856556540"), 3, "A big house for your whole family. Don't miss to buy a house with three bedrooms.", "https://www.luxury-architecture.net/wpcontent/uploads/2017/12/1513217889-7597-FAIRWAYS-010.jpg", 2100.00m, new Guid("3445ac97-a746-4374-f235-08dbc64ed13d"), "Big House Marina" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("2fd87338-5a8c-43e9-ac0c-d57a8cc11cef"));

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("4358f541-e6e8-4760-98a8-8a84a3e10564"));

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("eb961987-1c23-4609-9de7-f43cd1fa56cd"));

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Houses");

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
    }
}
