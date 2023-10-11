using HouseRentingSystem.Common;
using HouseRentingSystem.Data;
using HouseRentingSystem.Web.ViewModels.House;
using Microsoft.EntityFrameworkCore;
using HouseRentingSystem.Data.Models;
using HouseRentingSystem.Services.Data.Models.House;
using HouseRentingSystem.Web.ViewModels.House.Enums;

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

        public async Task<AllHousesFilteredAndPagedServiceModel> AllAsync(AllHousesQueryModel queryModel)
        {
            IQueryable<House> housesQuery = this
                .houseRentingDbContext
                .Houses
                .AsQueryable();

            //check if the input category of the user in the queryModel is not empty
            if (!string.IsNullOrWhiteSpace(queryModel.Category))
            {
                //search by category
                housesQuery = housesQuery
                    .Where(h => h.Category.Name == queryModel.Category);
            }
            //check if the input search string of the user in the queryModel is not empty
            if (!string.IsNullOrWhiteSpace(queryModel.SearchString))
            {
                string wildCard = $"%{queryModel.SearchString.ToLower()}%";
                housesQuery = housesQuery
                    .Where(h => EF.Functions.Like(h.Title, wildCard) ||
                                EF.Functions.Like(h.Address, wildCard) ||
                                EF.Functions.Like(h.Description, wildCard));
            }

            housesQuery = queryModel.HouseSorting switch
            {
                HouseSorting.Newest => housesQuery
                    .OrderBy(h => h.CreatedOn),
                HouseSorting.Oldest => housesQuery
                    .OrderByDescending(h => h.CreatedOn),
                HouseSorting.PriceAscending => housesQuery
                    .OrderBy(h => h.PricePerMonth),
                HouseSorting.PriceDescending => housesQuery
                    .OrderByDescending(h => h.PricePerMonth),
                _ => housesQuery
                    .OrderBy(h => h.RenterId != null)
                    .ThenByDescending(h => h.CreatedOn)
            };

            //here we materialize the IQueryable before all the operations are done in the db
            IEnumerable<HouseAllViewModel> allHouses = await housesQuery
                .Skip((queryModel.CurrentPage - 1) * queryModel.HousesPerPage)
                .Take(queryModel.HousesPerPage)
                .Select(h => new HouseAllViewModel
                {
                    Id = h.Id.ToString(),
                    Title = h.Title,
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    PricePerMonth = h.PricePerMonth,
                    IsRented = h.RenterId.HasValue
                })
                .ToArrayAsync();

            int totalHouses = housesQuery.Count();

            return new AllHousesFilteredAndPagedServiceModel
            {
                TotalHousesCount = totalHouses,
                Houses = allHouses,
            };
        }
    }
}
