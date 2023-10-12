using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HouseRentingSystem.Data;
using HouseRentingSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using HouseRentingSystem.Data.Models;

namespace HouseRentingSystem.Services
{
    public class UserService : IUSerService
    {
        private readonly HouseRentingDbContext dbContext;

        public UserService(HouseRentingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> UserHasRentsAsync(string userId)
        {
            ApplicationUser user = await this.dbContext
                .Users
                .FirstAsync(u => u.Id.ToString() == userId);

            return user.RentedHouses.Any();
        }
    }
}
