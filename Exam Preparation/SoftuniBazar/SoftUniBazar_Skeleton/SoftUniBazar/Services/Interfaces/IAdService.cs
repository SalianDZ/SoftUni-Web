using SoftUniBazar.Models.Ad;

namespace SoftUniBazar.Services.Interfaces
{
	public interface IAdService
	{
		public Task<IEnumerable<AllAdsViewModel>> AllAds();
	}
}
