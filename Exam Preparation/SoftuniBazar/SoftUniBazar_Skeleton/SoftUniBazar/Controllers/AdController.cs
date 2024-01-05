using Microsoft.AspNetCore.Mvc;
using SoftUniBazar.Models.Ad;
using SoftUniBazar.Services.Interfaces;

namespace SoftUniBazar.Controllers
{
	public class AdController : BaseController
	{
		private readonly IAdService adService;
        public AdController(IAdService adService)
        {
            this.adService = adService;
        }
        public async Task<IActionResult> All()
		{
			var model = await adService.AllAds();
			return View(model);
		}

		public async Task<IActionResult> Cart()
		{
			string currentUserId = GetUserId();
			var model = await adService.MineAds(currentUserId);
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Add()
		{
			AddAdViewModel model = await adService.AddOnHttpGet();
			return View(model);
		}
	}
}
