using HouseRentingSystem.Web.ViewModels.Agent;

namespace HouseRentingSystem.Services.Interfaces
{
    public interface IAgentService
    {
        Task<bool> AgentExistsByUserIdAsync(string userId);

        Task<bool> AgentExistsByPhoneNumberAsync(string phoneNumber);

        Task<bool> UserHasRentsByUserIdAsync(string userId);

        Task Create(string userId, BecomeAgentFormModel model);

    }
}
