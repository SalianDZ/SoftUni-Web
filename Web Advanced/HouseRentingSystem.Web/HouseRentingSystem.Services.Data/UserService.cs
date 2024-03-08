using HouseRentingSystem.Data;
using HouseRentingSystem.Data.Models;
using HouseRentingSystem.Services.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HouseRentingSystem.Services.Data
{
    public class UserService : IUserService
    {
        private readonly HouseRentingDbContext context;

        public UserService(HouseRentingDbContext context)
        {
                this.context = context;
        }

        public async Task<string> GetFullNameByEmailAsync(string email)
        {
            ApplicationUser? user = await context.Users.FirstOrDefaultAsync(u => u.Email == email); 
            
            if (user == null)
            {
                return string.Empty;
            }

            return $"{user.FirstName} {user.LastName}";
        }
    }
}
