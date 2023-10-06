using HouseRentingSystem.Web.ViewModels.Home;

namespace HouseRentingSystem.Services.Interfaces
{
    public interface IHouseService
    {
        Task<IEnumerable<IndexViewModel>> LastThreeHousesAsync();
    }
}
