using SoftUniBazar.Models.Ad;

namespace SoftUniBazar.Services.Interfaces
{
    public interface IAdService
    {
        Task<IEnumerable<AdViewModel>> GetAllAdsAsync();

        Task CreateAdAsync(AdFormViewModel model, string ownerId);

        Task<IEnumerable<AdViewModel>> GetAdsForCartByIdAsync(string userId);

        Task<AdFormViewModel?> GetModelForEditAsync(int id);

        Task<bool> IsUserByIdOwnerOfTheAd(string userId, int adId);

        Task EditAdByIdAsync(AdFormViewModel model, int id);
    }
}
