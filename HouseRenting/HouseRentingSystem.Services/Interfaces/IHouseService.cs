using HouseRentingSystem.Web.ViewModels.Home;
using HouseRentingSystem.Web.ViewModels.House;

namespace HouseRentingSystem.Services.Interfaces
{
    public interface IHouseService
    {
        Task<IEnumerable<IndexViewModel>> LastThreeHousesAsync();

        Task CreateAsync(HouseFormModel formModel, string agentId);
    }
}
