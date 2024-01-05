using Microsoft.EntityFrameworkCore;
using SoftUniBazar.Data;
using SoftUniBazar.Data.Models;
using SoftUniBazar.Models.Ad;
using SoftUniBazar.Models.Category;
using SoftUniBazar.Services.Interfaces;

namespace SoftUniBazar.Services
{
	public class AdService : IAdService
	{
		private readonly BazarDbContext context;
        public AdService(BazarDbContext context)
        {
            this.context = context;
        }

		public async Task<AddAdViewModel> AddOnHttpGet()
		{
			var categories = await context.Categories
				.Select(c => new CategoryViewModel 
				{
					Id = c.Id,
					Name = c.Name,
				}).ToListAsync();

			AddAdViewModel model = new()
			{
				Categories = categories
			};

			return model;
		}

		public async Task AddOnHttpPost(AddAdViewModel model, string userId)
		{
			Ad ad = new()
			{ 
				Name = model.Name,
				Description = model.Description,
				ImageUrl = model.ImageUrl,
				Price = model.Price,
				CategoryId = model.CategoryId,
				CreatedOn = DateTime.UtcNow,
				OwnerId = userId
			};

			await context.Ads.AddAsync(ad);
			await context.SaveChangesAsync();
		}

		public async Task<IEnumerable<AllAdsViewModel>> AllAds()
		{
			return await context.Ads
				.Select(ad => new AllAdsViewModel
				{
					Id = ad.Id,
					Name = ad.Name,
					Description = ad.Description,
					ImageUrl = ad.ImageUrl,
					Category = ad.Category.Name,
					Price = ad.Price,
					Owner = ad.Owner.UserName,
					CreatedOn = ad.CreatedOn.ToString("yyyy-MM-dd H:mm")
				}).ToListAsync();
		}

		public async Task<EditAdViewModel?> GetModelForEdit(int adId)
		{
			if (!context.Ads.Any(a => a.Id == adId))
			{
				return null;
			}

			EditAdViewModel model = await context.Ads
				.Select(m => new EditAdViewModel
				{
					Name= m.Name,
					Description = m.Description,
					ImageUrl = m.ImageUrl,
					CategoryId = m.CategoryId,
					Price = m.Price,
					Owner = m.Owner.UserName,
					OwnerId = m.OwnerId,
					Categories = context.Categories
						.Select(c => new CategoryViewModel 
						{
							Id= c.Id,
							Name= c.Name,
						}).ToList()
				})
				.FirstAsync();

			return model;
		}

		public bool DoesAdExist(int id)
		{
			return context.Ads.Any(ad => ad.Id == id);
		}

		public bool DoesCategoryExist(int categoryId)
		{
			return context.Categories.Any(c => c.Id == categoryId);
		}

		public async Task<IEnumerable<AllAdsViewModel>> MineAds(string userId)
		{
			return await context.AdsBuyers
				.Where(a => a.BuyerId == userId)
				.Select(ad => new AllAdsViewModel
				{
					Id = ad.Ad.Id,
					Name = ad.Ad.Name,
					Description = ad.Ad.Description,
					ImageUrl = ad.Ad.ImageUrl,
					Category = ad.Ad.Category.Name,
					Price = ad.Ad.Price,
					Owner = ad.Ad.Owner.UserName,
					CreatedOn = ad.Ad.CreatedOn.ToString("yyyy-MM-dd H:mm")
				})
				.ToListAsync();
		}

		public async Task EditModel(EditAdViewModel model, int adId)
		{
			Ad ad = await context.Ads.FirstAsync(a => a.Id == adId);

			ad.Name = model.Name;
			ad.Description = model.Description;
			ad.ImageUrl = model.ImageUrl;
			ad.Price = model.Price;
			ad.CategoryId = model.CategoryId;

			await context.SaveChangesAsync();
		}

		public async Task AddAdToCollection(int adId, string userId)
		{
			AdBuyer adBuyer = new AdBuyer()
			{ 
				AdId = adId,
				BuyerId = userId
			};

			await context.AdsBuyers.AddAsync(adBuyer);
			await context.SaveChangesAsync();
		}

		public bool AdAlreadyExist(int adId, string userId)
		{
			return context.AdsBuyers.Any(ab => ab.AdId == adId && ab.BuyerId == userId);
		}
	}
}
