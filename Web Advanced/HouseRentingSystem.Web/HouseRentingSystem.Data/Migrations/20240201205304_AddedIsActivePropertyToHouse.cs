﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseRentingSystem.Data.Migrations
{
    public partial class AddedIsActivePropertyToHouse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("a2e771b6-7839-4259-9016-20bc4c4e5be4"));

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("d4686d17-f77e-49b7-9717-9cc3bca3e0e1"));

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("e8ac2e09-376b-4257-906c-3a23cdee7600"));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Houses",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "AgentId", "CategoryId", "Description", "ImageUrl", "PricePerMonth", "RenterId", "Title" },
                values: new object[] { new Guid("39b5dbd8-b6b0-4486-8ccd-b6adcc72df65"), "North London, UK (near the border)", new Guid("eb18fbc8-967b-43ea-8c1f-5de49b9b0944"), 3, "A big house for your whole family. Don't miss to buy a house with three bedrooms.", "https://www.google.com/search?sca_esv=602324688&sxsrf=ACQVn091xG1DheAEe-Noujkj90AaTiohiQ:1706529204310&q=luxurious+penthouse+image+link&tbm=isch&source=lnms&sa=X&ved=2ahUKEwj2u5XYxIKEAxUfVfEDHcU_B-0Q0pQJegQIDBAB&biw=1536&bih=730&dpr=1.25#imgrc=mHLJU8LFXpjMfM", 2100.00m, new Guid("b32a0bae-6466-49a4-add5-08dc20be2566"), "Big House Marina" });

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "AgentId", "CategoryId", "Description", "ImageUrl", "PricePerMonth", "RenterId", "Title" },
                values: new object[] { new Guid("8ba37289-5d2e-4808-ad50-2d3618008477"), "Near the Sea Garden in Burgas, Bulgaria", new Guid("eb18fbc8-967b-43ea-8c1f-5de49b9b0944"), 2, "It has the best comfort you will ever ask for. With two bedrooms, it is great for your family.", "https://cf.bstatic.com/xdata/images/hotel/max1024x768/179489660.jpg?k=2029f6d9589b49c95dcc9503a265e292c2cdfcb5277487a0050397c3f8dd545a&o=&hp=1", 1200.00m, null, "Family House Comfort" });

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "AgentId", "CategoryId", "Description", "ImageUrl", "PricePerMonth", "RenterId", "Title" },
                values: new object[] { new Guid("cf63849b-f4b2-4231-a4bb-4b0c9c1a7719"), "Boyana Neighbourhood, Sofia, Bulgaria", new Guid("eb18fbc8-967b-43ea-8c1f-5de49b9b0944"), 2, "This luxurious house is everything you will need. It is just excellent.", "https://i.pinimg.com/originals/a6/f5/85/a6f5850a77633c56e4e4ac4f867e3c00.jpg", 2000.00m, null, "Grand House" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("39b5dbd8-b6b0-4486-8ccd-b6adcc72df65"));

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("8ba37289-5d2e-4808-ad50-2d3618008477"));

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("cf63849b-f4b2-4231-a4bb-4b0c9c1a7719"));

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Houses");

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "AgentId", "CategoryId", "CreatedOn", "Description", "ImageUrl", "PricePerMonth", "RenterId", "Title" },
                values: new object[] { new Guid("a2e771b6-7839-4259-9016-20bc4c4e5be4"), "Boyana Neighbourhood, Sofia, Bulgaria", new Guid("eb18fbc8-967b-43ea-8c1f-5de49b9b0944"), 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "This luxurious house is everything you will need. It is just excellent.", "https://i.pinimg.com/originals/a6/f5/85/a6f5850a77633c56e4e4ac4f867e3c00.jpg", 2000.00m, null, "Grand House" });

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "AgentId", "CategoryId", "CreatedOn", "Description", "ImageUrl", "PricePerMonth", "RenterId", "Title" },
                values: new object[] { new Guid("d4686d17-f77e-49b7-9717-9cc3bca3e0e1"), "North London, UK (near the border)", new Guid("eb18fbc8-967b-43ea-8c1f-5de49b9b0944"), 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "A big house for your whole family. Don't miss to buy a house with three bedrooms.", "https://www.google.com/search?sca_esv=602324688&sxsrf=ACQVn091xG1DheAEe-Noujkj90AaTiohiQ:1706529204310&q=luxurious+penthouse+image+link&tbm=isch&source=lnms&sa=X&ved=2ahUKEwj2u5XYxIKEAxUfVfEDHcU_B-0Q0pQJegQIDBAB&biw=1536&bih=730&dpr=1.25#imgrc=mHLJU8LFXpjMfM", 2100.00m, new Guid("b32a0bae-6466-49a4-add5-08dc20be2566"), "Big House Marina" });

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "AgentId", "CategoryId", "CreatedOn", "Description", "ImageUrl", "PricePerMonth", "RenterId", "Title" },
                values: new object[] { new Guid("e8ac2e09-376b-4257-906c-3a23cdee7600"), "Near the Sea Garden in Burgas, Bulgaria", new Guid("eb18fbc8-967b-43ea-8c1f-5de49b9b0944"), 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "It has the best comfort you will ever ask for. With two bedrooms, it is great for your family.", "https://cf.bstatic.com/xdata/images/hotel/max1024x768/179489660.jpg?k=2029f6d9589b49c95dcc9503a265e292c2cdfcb5277487a0050397c3f8dd545a&o=&hp=1", 1200.00m, null, "Family House Comfort" });
        }
    }
}
