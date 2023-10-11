using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HouseRentingSystem.Web.ViewModels.Category;

namespace HouseRentingSystem.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<HouseSelectCategoryFormModel>> AllCategoriesAsync();

        Task<bool> ExistsById(int id);

        Task<IEnumerable<string>> AllCategoryNamesAsync();
    }
}
