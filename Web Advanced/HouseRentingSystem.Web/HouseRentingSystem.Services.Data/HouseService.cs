using HouseRentingSystem.Data;
using HouseRentingSystem.Data.Models;
using HouseRentingSystem.Services.Data.Interfaces;
using HouseRentingSystem.Services.Data.Models.House;
using HouseRentingSystem.Services.Data.Models.Statistics;
using HouseRentingSystem.Web.ViewModels.Agent;
using HouseRentingSystem.Web.ViewModels.Home;
using HouseRentingSystem.Web.ViewModels.House;
using HouseRentingSystem.Web.ViewModels.House.Enums;
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

		public async Task<AllHousesFilteredAndPagedServiceModel> AllAsync(AllHousesQueryModel queryModel)
		{
			IQueryable<House> housesQuery = dbContext.Houses
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryModel.Category))
            {
                housesQuery = housesQuery
                    .Where(h => h.Category.Name == queryModel.Category);
            }

            if (!string.IsNullOrWhiteSpace(queryModel.SearchString))
            {
                string wildCard = $"%{queryModel.SearchString.ToLower()}%";

                housesQuery = housesQuery
                    .Where(h => EF.Functions.Like(h.Title, wildCard) ||
                                EF.Functions.Like(h.Address, wildCard) ||
                                EF.Functions.Like(h.Description, wildCard));
            }

            housesQuery = queryModel.HouseSorting switch
            {
                HouseSorting.Newest => housesQuery
                    .OrderByDescending(h => h.CreatedOn),
                HouseSorting.Oldest => housesQuery
                    .OrderBy(h => h.CreatedOn),
                HouseSorting.PriceAscending => housesQuery
                    .OrderBy(h => h.PricePerMonth),
                HouseSorting.PriceDescending => housesQuery
                    .OrderByDescending(h => h.PricePerMonth),
                    _ => housesQuery
                        .OrderBy(h => h.RenterId != null)
                        .ThenByDescending(h => h.CreatedOn)
            };

            IEnumerable<HouseAllViewModel> allHouses = await housesQuery
				.Where(h => h.IsActive)
				.Skip((queryModel.CurrentPage - 1) * queryModel.HousesPerPage)
                .Select(h => new HouseAllViewModel
                {
                    Id = h.Id.ToString(),
                    Title = h.Title,
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    PricePerMonth = h.PricePerMonth,
                    IsRented = h.RenterId.HasValue
                }).ToArrayAsync();

            int totalHouses = housesQuery.Count();

            return new AllHousesFilteredAndPagedServiceModel()
            { 
                TotalHousesCount = totalHouses,
                Houses = allHouses
            };
        }

		public async Task<IEnumerable<HouseAllViewModel>> AllByAgentIdAsync(string agentId)
		{
            IEnumerable<HouseAllViewModel> agentHouses =
                await dbContext.Houses
                .Where(h => h.AgentId.ToString() == agentId && h.IsActive)
                .Select(h => new HouseAllViewModel
                {
                    Id = h.Id.ToString(),
                    Title = h.Title,
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    PricePerMonth = h.PricePerMonth,
                    IsRented = h.RenterId.HasValue
                }).ToArrayAsync();

            return agentHouses;
		}

		public async Task<IEnumerable<HouseAllViewModel>> AllByUserIdAsync(string userId)
		{
			IEnumerable<HouseAllViewModel> userHouses =
				await dbContext.Houses
				.Where(h => h.RenterId.HasValue && h.RenterId.ToString() == userId && h.IsActive)
				.Select(h => new HouseAllViewModel
				{
					Id = h.Id.ToString(),
					Title = h.Title,
					Address = h.Address,
					ImageUrl = h.ImageUrl,
					PricePerMonth = h.PricePerMonth,
					IsRented = h.RenterId.HasValue
				}).ToArrayAsync();

            return userHouses;
		}

		public async Task<string> CreateAndReturnIdAsync(HouseFormModel formModel, string agentId)
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
            return house.Id.ToString();
		}

		public async Task DeleteHouseByIdAsync(string houseId)
		{
            House house = await dbContext.Houses
                .Where(h => h.IsActive)
                .FirstAsync(h => h.Id.ToString() == houseId);

            house.IsActive = false;
            await dbContext.SaveChangesAsync();
		}

		public async Task EditHouseAsync(string id, HouseFormModel formModel)
		{
            House house = await dbContext.Houses.Where(h => h.IsActive).FirstAsync(h => h.Id.ToString() == id);

            house.Title = formModel.Title;
            house.Address = formModel.Address;
            house.Description = formModel.Description;
            house.PricePerMonth = formModel.PricePerMonth;
            house.ImageUrl = formModel.ImageUrl;
            house.CategoryId = formModel.CategoryId;

            await dbContext.SaveChangesAsync();
		}

		public async Task<bool> ExistsByIdAsync(string houseId)
		{
            bool result = 
                await dbContext
                .Houses
                .Where(h => h.IsActive)
                .AnyAsync(h => h.Id.ToString() == houseId);
            return result;
		}

		public async Task<HouseDetailsViewModel> GetDetailsByIdAsync(string houseId)
		{
			HouseDetailsViewModel? viewModel = await dbContext.Houses.Include(h => h.Category).Include(h => h.Agent).ThenInclude(a => a.User)
                .Where(h => h.Id.ToString() == houseId)
                .Select(h => new HouseDetailsViewModel
                {
                    Id = h.Id.ToString(),
                    Title = h.Title,
                    Address = h.Address,
                    Description = h.Description,
                    PricePerMonth = h.PricePerMonth,
                    ImageUrl = h.ImageUrl,
                    IsRented = h.RenterId.HasValue,
                    Category = h.Category.Name,
                    Agent = new AgentInfoOnHouseViewModel()
                    { 
                        Email = h.Agent.User.Email,
                        PhoneNumber = h.Agent.PhoneNumber
                    }
                }).FirstAsync();

            return viewModel;
		}

		public async Task<HousePreDeleteDetailsViewModel> GetHouseForDeleteByIdAsync(string houseId)
		{
            HousePreDeleteDetailsViewModel model = await dbContext.Houses
                .Where (h => h.Id.ToString() == houseId && h.Id.ToString() == houseId)
                .Select(h => new HousePreDeleteDetailsViewModel
                {
                    Title = h.Title,
                    Address = h.Address,
                    ImageUrl = h.ImageUrl
                }).FirstAsync();

            return model;
		}
		public async Task<HouseFormModel> GetHouseForEditByIdAsync(string houseId)
		{
            House house = await dbContext.Houses.Include(h => h.Category).Where(h => h.Id.ToString() == houseId).FirstAsync();

            return new HouseFormModel
            { 
                Title = house.Title,
                Address = house.Address,
                Description = house.Description,
                ImageUrl= house.ImageUrl,
                PricePerMonth= house.PricePerMonth,
                CategoryId = house.CategoryId
            };
		}

		public async Task<StatisticsServiceModel> GetStatisticsAsync()
		{
            return new StatisticsServiceModel()
            {
                TotalHouses = await dbContext.Houses.CountAsync(),
                TotalRents = await dbContext.Houses.Where(h => h.RenterId.HasValue).CountAsync()
            };
		}

		public async Task<bool> IsAgentWithIdOwnerOfHouseWithIdAsync(string houseId, string agentId)
		{
            House house = await dbContext.Houses.Where(h => h.IsActive).FirstAsync(h => h.Id.ToString() == houseId);
            return house.AgentId.ToString() == agentId;
		}

		public async Task<bool> IsRentedByIdAsync(string houseId)
		{
            House house = await dbContext.Houses
                .FirstAsync(h => h.Id.ToString() == houseId);

            return house.RenterId.HasValue;
		}

		public async Task<bool> IsRentedByUserWithIdAsync(string houseId, string userId)
		{
            House house = await dbContext.Houses
                .FirstAsync(h => h.Id.ToString() == houseId);

            return house.RenterId.HasValue && house.RenterId.ToString() == userId;
		}

		public async Task<IEnumerable<IndexViewModel>> LastFreeHousesAsync()
        {
            IEnumerable<IndexViewModel> lastThreeHouses = await dbContext
                .Houses
                .Where(h => h.IsActive)
                .OrderByDescending(h => h.CreatedOn)
                .Take(3)
                .Select(h => new IndexViewModel()
                {
                    Id = h.Id.ToString(),
                    Title = h.Title,
                    ImageUrl = h.ImageUrl
                })
                .ToArrayAsync();

            return lastThreeHouses;
        }

		public async Task LeaveHouseAsync(string houseId)
		{
            House house = await dbContext.Houses.FirstAsync(h => h.Id.ToString() == houseId);

            house.RenterId = null;
            await dbContext.SaveChangesAsync();
		}

		public async Task RentHouseAsync(string houseId, string userId)
		{
            House house = await dbContext.Houses.FirstAsync(h => h.Id.ToString() == houseId);

            house.RenterId = Guid.Parse(userId);
            await dbContext.SaveChangesAsync();
		}
	}
}
