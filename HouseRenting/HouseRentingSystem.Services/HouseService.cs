using HouseRentingSystem.Common;
using HouseRentingSystem.Data;
using HouseRentingSystem.Web.ViewModels.House;
using Microsoft.EntityFrameworkCore;
using HouseRentingSystem.Data.Models;
using HouseRentingSystem.Services.Data.Models.House;
using HouseRentingSystem.Services.Data.Models.Statistics;
using HouseRentingSystem.Web.ViewModels.Agent;
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
                    .Where(h => h.IsActive)
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

        public async Task<string> CreateAndReturnIdAsync(HouseFormModel formModel, string agentId)
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
            //after house is persisted to db it gets its Id automatically
            //and we can directly use it here.
           return newHouse.Id.ToString();
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
                    .OrderByDescending(h => h.CreatedOn),
                HouseSorting.Oldest => housesQuery
                    .OrderBy(h => h.CreatedOn),
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
                .Where(h => h.IsActive)
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

        public async Task<IEnumerable<HouseAllViewModel>> AllByAgentIdAsync(string agentId)
        {
            IEnumerable<HouseAllViewModel> allAgentHouses = await this.houseRentingDbContext
                .Houses
                .Where(h => h.IsActive)
                .Where(h => h.AgentId.ToString() == agentId)
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

            return allAgentHouses;
        }

        public async Task<IEnumerable<HouseAllViewModel>> AllByUserIdAsync(string userId)
        {
            IEnumerable<HouseAllViewModel> allUserHouses = await this.houseRentingDbContext
                .Houses
                .Where(h => h.IsActive)
                .Where(h => h.RenterId.ToString() == userId &&
                            h.RenterId.HasValue)
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

            return allUserHouses;
        }

        public async Task<HouseDetailsViewModel> GetDetailsByIdAsync(string houseId)
        {
            House house =await this.houseRentingDbContext
                .Houses
                .Include(h => h.Category)
                .Include(h => h.Agent)
                .ThenInclude(a => a.User)
                .Where(h => h.IsActive)
                .FirstAsync(h => h.Id.ToString() == houseId);

            if (house == null)
            {
                return null;
            }

            return new HouseDetailsViewModel
            {
                Id = house.Id.ToString(),
                Title = house.Title,
                Address = house.Address,
                ImageUrl = house.ImageUrl,
                IsRented = house.RenterId.HasValue,
                PricePerMonth = house.PricePerMonth,
                Description = house.Description,
                Category = house.Category.Name,
                Agent = new AgentInfoOnHouseViewModel
                {
                    Email = house.Agent.User.Email!,
                    PhoneNumber = house.Agent.PhoneNumber
                }
            };
        }

        public async Task<bool> ExistsByIdAsync(string houseId)
        {
            bool result = await this.houseRentingDbContext
                .Houses
                .Where(h => h.IsActive)
                .AnyAsync(h => h.Id.ToString() == houseId);

            return result;
        }

        public async Task<HouseFormModel> GetHouseForEditByIdAsync(string houseId)
        {
            House house = await this.houseRentingDbContext
                .Houses
                .Include(h => h.Category)
                .Where(h => h.IsActive)
                .FirstAsync(h => h.Id.ToString() == houseId);

            return new HouseFormModel
            {
                Title = house.Title,
                Address = house.Address,
                Description = house.Description,
                ImageUrl = house.ImageUrl,
                PricePerMonth = house.PricePerMonth,
                CategoryId = house.CategoryId,
            };
        }

        public async Task<bool> IsAgentWithIdOwnerOfHouseWithIdAsync(string houseId, string agentId)
        {
            House house = await this.houseRentingDbContext
                .Houses
                .Where(h => h.IsActive)
                .FirstAsync(h => h.Id.ToString() == houseId);

            return house.AgentId.ToString() == agentId;
        }

        public async Task EditHouseByIdAndFormModel(string houseId, HouseFormModel formModel)
        {
            House house = await this.houseRentingDbContext
                .Houses
                .Where(h => h.IsActive)
                .FirstAsync(h => h.Id.ToString() == houseId);

            house.Title = formModel.Title;
            house.Address = formModel.Address;
            house.Description = formModel.Description;
            house.ImageUrl = formModel.ImageUrl;
            house.CategoryId = formModel.CategoryId;
            house.PricePerMonth = formModel.PricePerMonth;

            await this.houseRentingDbContext.SaveChangesAsync();
        }

        public async Task<HousePreDeleteDetailsViewModel> GetHouseForDeleteByIdAsync(string houseId)
        {
            House house = await this.houseRentingDbContext
                .Houses
                .Where(h => h.IsActive)
                .FirstAsync(h => h.Id.ToString() == houseId);

            return new HousePreDeleteDetailsViewModel()
            {
                Title = house.Title,
                Address = house.Address,
                ImageUrl = house.ImageUrl,
            };
        }

        public async Task DeleteHouseByIdAsync(string houseId)
        {
            House houseToDelete = await this.houseRentingDbContext
                .Houses
                .Where(h => h.IsActive)
                .FirstAsync(h => h.Id.ToString() == houseId);

            houseToDelete.IsActive = false;
            await this.houseRentingDbContext.SaveChangesAsync();
        }

        public async Task<bool> IsRented(string houseId)
        {
            House house = await this.houseRentingDbContext
                .Houses
                .FirstAsync(h => h.Id.ToString() == houseId);

            return house.RenterId.HasValue;
        }

        public async Task RentHouseAsync(string houseId, string userId)
        {
            House house = await this.houseRentingDbContext
                .Houses
                .Where(h => h.IsActive)
                .FirstAsync(h => h.Id.ToString() == houseId);

            house.RenterId = Guid.Parse(userId);

            await this.houseRentingDbContext.SaveChangesAsync();
        }

        public async Task<bool> IsRentedByUserWithIdAsync(string houseId, string userId)
        {
            House house = await this.houseRentingDbContext
                .Houses
                .Where(h => h.IsActive)
                .FirstAsync(h => h.Id.ToString() == houseId);

            return house.RenterId.HasValue && house.RenterId.ToString() == userId; 
        }

        public async Task LeaveHouseAsync(string houseId)
        {
            House house = await this.houseRentingDbContext
                .Houses
                .Where(h => h.IsActive)
                .FirstAsync(h => h.Id.ToString() == houseId);

            house.RenterId = null;
            await this.houseRentingDbContext.SaveChangesAsync();
        }

        public async Task<StatisticsServiceModel> GetStatisticsAsync()
        {
            return new StatisticsServiceModel()
            {
                TotalHouses = await this.houseRentingDbContext
                    .Houses
                    .CountAsync(),
                TotalRents = await this.houseRentingDbContext
                    .Houses
                    .Where(h => h.RenterId.HasValue)
                    .CountAsync(),
            };
        }
    }
}
