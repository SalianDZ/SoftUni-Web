using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseRentingSystem.Data.Migrations
{
    public partial class SeedingDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Cottage" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Single-Family" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Duplex" });

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "AgentId", "CategoryId", "Description", "ImageUrl", "PricePerMonth", "RenterId", "Title" },
                values: new object[] { new Guid("04f71658-1b86-4a5d-9dea-ca9a79197195"), "North London, UK (near the border)", new Guid("eb18fbc8-967b-43ea-8c1f-5de49b9b0944"), 3, "A big house for your whole family. Don't miss to buy a house with three bedrooms.", "https://www.google.com/search?sca_esv=602324688&sxsrf=ACQVn091xG1DheAEe-Noujkj90AaTiohiQ:1706529204310&q=luxurious+penthouse+image+link&tbm=isch&source=lnms&sa=X&ved=2ahUKEwj2u5XYxIKEAxUfVfEDHcU_B-0Q0pQJegQIDBAB&biw=1536&bih=730&dpr=1.25#imgrc=mHLJU8LFXpjMfM", 2100.00m, new Guid("b32a0bae-6466-49a4-add5-08dc20be2566"), "Big House Marina" });

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "AgentId", "CategoryId", "Description", "ImageUrl", "PricePerMonth", "RenterId", "Title" },
                values: new object[] { new Guid("c4061ef0-7ed5-4280-975b-3d294d9da62d"), "Boyana Neighbourhood, Sofia, Bulgaria", new Guid("eb18fbc8-967b-43ea-8c1f-5de49b9b0944"), 2, "This luxurious house is everything you will need. It is just excellent.", "https://i.pinimg.com/originals/a6/f5/85/a6f5850a77633c56e4e4ac4f867e3c00.jpg", 2000.00m, null, "Grand House" });

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "AgentId", "CategoryId", "Description", "ImageUrl", "PricePerMonth", "RenterId", "Title" },
                values: new object[] { new Guid("cd2695a9-af14-46c0-9a85-c2769742f98c"), "Near the Sea Garden in Burgas, Bulgaria", new Guid("eb18fbc8-967b-43ea-8c1f-5de49b9b0944"), 2, "It has the best comfort you will ever ask for. With two bedrooms, it is great for your family.", "https://cf.bstatic.com/xdata/images/hotel/max1024x768/179489660.jpg?k=2029f6d9589b49c95dcc9503a265e292c2cdfcb5277487a0050397c3f8dd545a&o=&hp=1", 1200.00m, null, "Family House Comfort" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("04f71658-1b86-4a5d-9dea-ca9a79197195"));

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("c4061ef0-7ed5-4280-975b-3d294d9da62d"));

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("cd2695a9-af14-46c0-9a85-c2769742f98c"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
