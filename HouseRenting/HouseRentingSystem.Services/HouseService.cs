using HouseRentingSystem.Data;
using Microsoft.EntityFrameworkCore;

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
    }
}
