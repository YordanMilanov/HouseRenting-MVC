using HouseRentingSystem.Common;
using HouseRentingSystem.Data;
using HouseRentingSystem.Web.ViewModels.House;
using Microsoft.EntityFrameworkCore;
using HouseRentingSystem.Data.Models;

namespace HouseRentingSystem.Services
{
    using HouseRentingSystem.Services.Interfaces;
    using HouseRentingSystem.Web.ViewModels.Home;
    public class HouseService : IHouseService
    {
        private readonly HouseRentingDbContext houseRentingDbContext;

        public HouseService(HouseRentingDbContext houseRentingDbContext)
        {
            this.houseRentingDbContext = houseRentingDbContext;
        }

        public async Task<IEnumerable<IndexViewModel>> LastThreeHousesAsync()
        {
            IEnumerable<IndexViewModel> lastThreeHouses =
               await this.houseRentingDbContext
                    .Houses
                    .OrderByDescending(h => h.CreatedOn)
                    .Take(3)
                    .Select(h => new IndexViewModel()
                    {
                        Id = h.Id.ToString(),
                        Title = h.Title,
                        ImageUrl = h.ImageUrl
                    })
                    .ToArrayAsync();
            return lastThreeHouses;
        }

        public async Task CreateAsync(HouseFormModel formModel, string agentId)
        {
            House newHouse = new House
            {
                Title = formModel.Title,
                Address = formModel.Address,
                Description = formModel.Description,
                ImageUrl = formModel.ImageUrl,
                PricePerMonth = formModel.PricePerMonth,
                CategoryId = formModel.CategoryId,
                AgentId = Guid.Parse(agentId),
            };

           await this.houseRentingDbContext.AddAsync(newHouse);
           await this.houseRentingDbContext.SaveChangesAsync();
        }
    }
}
