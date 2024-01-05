using SoftUniBazar.Models.Ad;

namespace SoftUniBazar.Services.Interfaces
{
	public interface IAdService
	{
		Task<IEnumerable<AllAdsViewModel>> AllAds();
		Task<IEnumerable<AllAdsViewModel>> MineAds(string userId);

		Task<AddAdViewModel> AddOnHttpGet();
		Task AddOnHttpPost(AddAdViewModel model, string userId);

		bool DoesCategoryExist(int categoryId);

		Task<EditAdViewModel?> GetModelForEdit(int adId);

		Task EditModel(EditAdViewModel model, int adId);
	}
}
