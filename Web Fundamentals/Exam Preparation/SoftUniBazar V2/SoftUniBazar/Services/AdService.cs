using Microsoft.EntityFrameworkCore;
using SoftUniBazar.Data;
using SoftUniBazar.Data.Models;
using SoftUniBazar.Models.Ad;
using SoftUniBazar.Services.Interfaces;
using static SoftUniBazar.Data.ValidationConstants.Ad;

namespace SoftUniBazar.Services
{
    public class AdService : IAdService
    {
        private readonly BazarDbContext dbContext;

        public AdService(BazarDbContext dbContext)
        {
            this.dbContext = dbContext;    
        }

        public async Task<bool> AdAlreadyAddedByIdAsync(int id, string userId)
        {
            bool result = await dbContext.AdBuyers.AnyAsync(ab => ab.AdId == id && ab.BuyerId == userId);
            return result;
        }

        public async Task AddAdToUserCollectionAsync(int id, string userId)
        {
            AdBuyer adBuyer = new()
            {
                AdId = id,
                BuyerId = userId
            };

            await dbContext.AdBuyers.AddAsync(adBuyer);
            await dbContext.SaveChangesAsync();
        }

        public async Task CreateAdAsync(AdFormViewModel model, string ownerId)
        {
            Ad ad = new Ad() 
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                ImageUrl = model.ImageUrl,
                CategoryId = model.CategoryId,
                OwnerId = ownerId,
                CreatedOn = DateTime.UtcNow
            };

            await dbContext.Ads.AddAsync(ad);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> DoesAdExistByIdAsync(int id)
        {
            bool result = await dbContext.Ads.AnyAsync(a => a.Id == id);
            return result;
        }

        public async Task EditAdByIdAsync(AdFormViewModel model, int id)
        {
            Ad ad = await dbContext.Ads.FirstAsync(a => a.Id == id);

            ad.Name = model.Name;
            ad.Description = model.Description;
            ad.ImageUrl = model.ImageUrl;
            ad.Price = model.Price;
            ad.CategoryId = model.CategoryId;

            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<AdViewModel>> GetAdsForCartByIdAsync(string userId)
        {
            IEnumerable<AdViewModel> userAds =
                await dbContext.AdBuyers
                .Include(ab => ab.Ad.Category)
                .Where(ab => ab.BuyerId == userId)
                .Select(ab => new AdViewModel()
                {
                    Id = ab.AdId,
                    Name = ab.Ad.Name,
                    ImageUrl = ab.Ad.ImageUrl,
                    CreatedOn = ab.Ad.CreatedOn.ToString(DateFormat),
                    Category = ab.Ad.Category.Name,
                    Description = ab.Ad.Description,
                    Price = ab.Ad.Price.ToString(),
                    Owner = ab.Ad.Owner.UserName,
                }).ToArrayAsync();

            return userAds;
        }

        public async Task<IEnumerable<AdViewModel>> GetAllAdsAsync()
        {
            IEnumerable<AdViewModel> allAds = await dbContext.Ads
                 .Select(ad => new AdViewModel
                 {
                     Id = ad.Id,
                     Name = ad.Name,
                     Description = ad.Description,
                     ImageUrl = ad.ImageUrl,
                     Category = ad.Category.Name,
                     CreatedOn = ad.CreatedOn.ToString(DateFormat),
                     Price = ad.Price.ToString(),
                     Owner = ad.Owner.UserName
                 }).ToArrayAsync();

            return allAds;
        }

        public async Task<AdFormViewModel?> GetModelForEditAsync(int id)
        {
            Ad? dbModel =
                await dbContext.Ads.FirstOrDefaultAsync(a => a.Id == id)!;

            if (dbModel == null)
            {
                return null;
            }

            AdFormViewModel model = new()
            { 
                Name = dbModel.Name,
                Description = dbModel.Description,
                ImageUrl = dbModel.ImageUrl,
                Price = dbModel.Price,
                CategoryId = dbModel.CategoryId
            };

            return model;
        }

        public async Task<bool> IsUserByIdOwnerOfTheAd(string userId, int adId)
        {
            Ad ad = await dbContext.Ads.FirstAsync(a => a.Id == adId);
            bool result = ad.OwnerId == userId;
            return result;
        }

        public async Task RemoveAdFromUserCollectionAsync(int id, string userId)
        {
            AdBuyer adBuyer =
                await dbContext.AdBuyers
                .FirstAsync(ab => ab.AdId == id && ab.BuyerId == userId);

            dbContext.AdBuyers.Remove(adBuyer);
            await dbContext.SaveChangesAsync();
        }
    }
}
