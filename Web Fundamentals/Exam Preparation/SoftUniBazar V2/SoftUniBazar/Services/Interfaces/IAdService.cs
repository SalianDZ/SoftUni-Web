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

        Task<bool> DoesAdExistByIdAsync(int id);

        Task<bool> AdAlreadyAddedByIdAsync(int id, string userId);

        Task AddAdToUserCollectionAsync(int id, string userId);

        Task RemoveAdFromUserCollectionAsync(int id, string userId);
    }
}
