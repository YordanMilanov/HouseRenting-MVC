using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HouseRentingSystem.Data;
using HouseRentingSystem.Data.Models;
using HouseRentingSystem.Services.Interfaces;
using HouseRentingSystem.Web.ViewModels.Agent;
using Microsoft.EntityFrameworkCore;

namespace HouseRentingSystem.Services
{
    public class AgentService : IAgentService
    {
        private readonly HouseRentingDbContext houseRentingDbContext;

        public AgentService(HouseRentingDbContext houseRentingDbContext)
        {
            this.houseRentingDbContext = houseRentingDbContext;
        }

        public async Task<bool> AgentExistsByUserIdAsync(string userId)
        {
          bool result = await this.houseRentingDbContext
                .Agents
                .AnyAsync(a => a.UserId.ToString() == userId);

          return result;
        }

        public async Task<bool> AgentExistsByPhoneNumberAsync(string phoneNumber)
        {
            bool result = await this.houseRentingDbContext
                .Agents
                .AnyAsync(a => a.PhoneNumber == phoneNumber);
            return result;
        }

        public async Task<bool> UserHasRentsByUserIdAsync(string userId)
        {
            ApplicationUser? user = await this.houseRentingDbContext
                .Users
                .FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null)
            {
                return false;
            }

            return user.RentedHouses.Any();
        }

        public async Task Create(string userId, BecomeAgentFormModel model)
        {
            Agent agent = new Agent()
            {
                PhoneNumber = model.PhoneNumber,
                UserId = Guid.Parse(userId)
            };

            await this.houseRentingDbContext.Agents.AddAsync(agent);
            await this.houseRentingDbContext.SaveChangesAsync();
        }

        public async Task<string> GetAgentIdByUserIdAsync(string userId)
        {
            Agent? agent = await this.houseRentingDbContext
                .Agents
                .FirstOrDefaultAsync(a => a.UserId.ToString() == userId);

            if (agent == null)
            {
                return null;
            }

            return agent.Id.ToString();
        }
    }
}
