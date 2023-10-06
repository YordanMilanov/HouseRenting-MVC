using HouseRentingSystem.Data;
using HouseRentingSystem.Services.Interfaces;
using HouseRentingSystem.Web.ViewModels.Category;
using Microsoft.EntityFrameworkCore;

namespace HouseRentingSystem.Services
{
    public class CategoryService : ICategoryService
    {

        private readonly HouseRentingDbContext dbContext;

        public CategoryService(HouseRentingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public async Task<IEnumerable<HouseSelectCategoryFormModel>> AllCategoriesAsync()
        {
            IEnumerable<HouseSelectCategoryFormModel> allCategories = await this.dbContext
                .Categories
                .Select(c => new HouseSelectCategoryFormModel()
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .AsNoTracking()
                .ToArrayAsync();

            return allCategories;
        }

        public async Task<bool> ExistsById(int id)
        {
            bool result = await this.dbContext
                .Categories
                .AnyAsync(c => c.Id == id);

            return result;
        }
    }
}
