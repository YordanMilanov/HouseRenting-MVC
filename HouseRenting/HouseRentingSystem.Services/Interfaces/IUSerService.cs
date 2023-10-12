namespace HouseRentingSystem.Services.Interfaces
{
    public interface IUSerService
    {
        Task<bool> UserHasRentsAsync(string userId);
    }
}
