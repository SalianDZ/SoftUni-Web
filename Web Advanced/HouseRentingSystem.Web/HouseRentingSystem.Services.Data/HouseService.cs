using HouseRentingSystem.Data;
using HouseRentingSystem.Data.Models;
using HouseRentingSystem.Services.Data.Interfaces;
using HouseRentingSystem.Web.ViewModels.Home;
using HouseRentingSystem.Web.ViewModels.House;
using Microsoft.EntityFrameworkCore;

namespace HouseRentingSystem.Services.Data
{
	public class HouseService : IHouseService
    {
        private readonly HouseRentingDbContext dbContext;

        public HouseService(HouseRentingDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

		public async Task CreateAsync(HouseFormModel formModel, string agentId)
		{
            House house = new House()
            { 
                Title = formModel.Title,
                Address = formModel.Address,
                Description = formModel.Description,
                PricePerMonth = formModel.PricePerMonth,
                ImageUrl = formModel.ImageUrl,
                CategoryId = formModel.CategoryId,
                AgentId = Guid.Parse(agentId)
            };

            await dbContext.Houses.AddAsync(house);
            await dbContext.SaveChangesAsync();
		}

		public async Task<IEnumerable<IndexViewModel>> LastFreeHousesAsync()
        {
            IEnumerable<IndexViewModel> lastThreeHouses = await dbContext.Houses.OrderByDescending(h => h.CreatedOn).Take(3)
                .Select(h => new IndexViewModel()
                {
                    Id = h.Id.ToString(),
                    Title = h.Title,
                    ImageUrl = h.ImageUrl
                })
                .ToArrayAsync();

            return lastThreeHouses;
        }
    }
}
