using Microsoft.AspNetCore.Mvc;
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
	}
}
