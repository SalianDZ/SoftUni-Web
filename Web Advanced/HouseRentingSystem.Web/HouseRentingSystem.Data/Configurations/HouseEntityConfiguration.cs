using HouseRentingSystem.Data.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static HouseRentingSystem.Common.EntityValidationConstants;
using House = HouseRentingSystem.Data.Models.House;

namespace HouseRentingSystem.Data.Configurations
{
    public class HouseEntityConfiguration : IEntityTypeConfiguration<House>
    {
        public void Configure(EntityTypeBuilder<House> builder)
        {
            builder.Property(h => h.CreatedOn)
                .HasDefaultValue(DateTime.UtcNow);

            builder.HasOne(h => h.Category)
                .WithMany(c => c.Houses)
                .HasForeignKey(h => h.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(h => h.Agent)
                .WithMany(a => a.OwnedHouses)
                .HasForeignKey(h => h.AgentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(GenerateHouse());

            //builder.HasOne(h => h.Renter)
            //    .WithMany(r => r.RentedHouse)
            //    .HasForeignKey(h => h.RenterId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }

        private House[] GenerateHouse()
        { 
            ICollection<House> houses = new HashSet<House>();

            House house;

            house = new House()
            {
                Title = "Big House Marina",
                Address = "North London, UK (near the border)",
                Description = "A big house for your whole family. Don't miss to buy a house with three bedrooms.",
                ImageUrl = "https://www.google.com/search?sca_esv=602324688&sxsrf=ACQVn091xG1DheAEe-Noujkj90AaTiohiQ:1706529204310&q=luxurious+penthouse+image+link&tbm=isch&source=lnms&sa=X&ved=2ahUKEwj2u5XYxIKEAxUfVfEDHcU_B-0Q0pQJegQIDBAB&biw=1536&bih=730&dpr=1.25#imgrc=mHLJU8LFXpjMfM",
                PricePerMonth = 2100.00M,
                CategoryId = 3,
                AgentId = Guid.Parse("EB18FBC8-967B-43EA-8C1F-5DE49B9B0944"),
                RenterId = Guid.Parse("B32A0BAE-6466-49A4-ADD5-08DC20BE2566")
            };

            houses.Add(house);

            house = new House()
            {
                Title = "Family House Comfort",
                Address = "Near the Sea Garden in Burgas, Bulgaria",
                Description = "It has the best comfort you will ever ask for. With two bedrooms, it is great for your family.",
                ImageUrl = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/179489660.jpg?k=2029f6d9589b49c95dcc9503a265e292c2cdfcb5277487a0050397c3f8dd545a&o=&hp=1",
                PricePerMonth = 1200.00M,
                CategoryId = 2,
                AgentId = Guid.Parse("EB18FBC8-967B-43EA-8C1F-5DE49B9B0944")
            };

            houses.Add(house);

            house = new House()
            {
                Title = "Grand House",
                Address = "Boyana Neighbourhood, Sofia, Bulgaria",
                Description = "This luxurious house is everything you will need. It is just excellent.",
                ImageUrl = "https://i.pinimg.com/originals/a6/f5/85/a6f5850a77633c56e4e4ac4f867e3c00.jpg",
                PricePerMonth = 2000.00M,
                CategoryId = 2,
                AgentId = Guid.Parse("EB18FBC8-967B-43EA-8C1F-5DE49B9B0944")
            };

            houses.Add(house);

            return houses.ToArray();
        }
    }
}
