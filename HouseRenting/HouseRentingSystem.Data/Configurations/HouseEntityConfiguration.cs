using HouseRentingSystem.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HouseRentingSystem.Data.Configurations
{
    public class HouseEntityConfiguration : IEntityTypeConfiguration<House>
    {
        public void Configure(EntityTypeBuilder<House> builder)
        {
            //automatically set the date to the date of the moment of adding
            builder
                .Property(h => h.CreatedOn)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(h => h.IsActive)
                .HasDefaultValue(true);

            builder
                .HasOne(h => h.Category)
                .WithMany(c => c.Houses)
                .HasForeignKey(h => h.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(h => h.Agent)
                .WithMany(a => a.OwnedHouses)
                .HasForeignKey(h => h.AgentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasData(GenerateHouses());
        }

        private House[] GenerateHouses()
        {
            ICollection<House> houses = new List<House>();
            House house;
            house = new House()
            {
                Title = "Big House Marina",
                Address = "North London, UK (near the border)",
                Description = "A big house for your whole family. Don't miss to buy a house with three bedrooms.",
                ImageUrl = "https://www.luxury-architecture.net/wpcontent/uploads/2017/12/1513217889-7597-FAIRWAYS-010.jpg",
                PricePerMonth = 2100.00M,
                CategoryId = 3,
                AgentId = Guid.Parse("8F2CE198-F478-4B6F-8EA0-B72856556540"), //AgentId
                RenterId = Guid.Parse("3445AC97-A746-4374-F235-08DBC64ED13D") //UserId
            };
            houses.Add(house);
            house = new House()
            {
                Title = "Family House Comfort",
                Address = "Near the Sea Garden in Burgas, Bulgaria",
                Description = "It has the best comfort you will ever ask for. With two bedrooms,it is great for your family.",
                ImageUrl = "https://www.google.com/imgres?imgurl=https%3A%2F%2Fimg.freepik.com%2Fpremium-photo%2Fluxury-beautiful-house_967158-73.jpg&tbnid=_Wk0V3qZo5ARNM&vet=12ahUKEwjD9Z7-leGBAxUVwwIHHQq6C-0QMygBegQIARB2..i&imgrefurl=https%3A%2F%2Fwww.freepik.com%2Ffree-photos-vectors%2Fbeautiful-house&docid=0MLiyCk8BDweCM&w=626&h=626&q=beautiful%20house&ved=2ahUKEwjD9Z7-leGBAxUVwwIHHQq6C-0QMygBegQIARB2",
                PricePerMonth = 1200.00M,
                CategoryId = 2,
                AgentId = Guid.Parse("8F2CE198-F478-4B6F-8EA0-B72856556540"), //AgentId
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
                AgentId = Guid.Parse("8F2CE198-F478-4B6F-8EA0-B72856556540"), //AgentId
            };
            houses.Add(house);

            return houses.ToArray();
        }
    }
}
