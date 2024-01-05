using SoftUniBazar.Models.Ad;

namespace SoftUniBazar.Services.Interfaces
{
	public interface IAdService
	{
		Task<IEnumerable<AllAdsViewModel>> AllAds();
		Task<IEnumerable<AllAdsViewModel>> MineAds(string userId);

		Task<AddAdViewModel> AddOnHttpGet();
	}
}
