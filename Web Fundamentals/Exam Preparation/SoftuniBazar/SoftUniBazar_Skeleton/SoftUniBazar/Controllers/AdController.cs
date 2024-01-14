using Microsoft.AspNetCore.Mvc;
using SoftUniBazar.Data.Models;
using SoftUniBazar.Models.Ad;
using SoftUniBazar.Services.Interfaces;
using System.Security.Cryptography.X509Certificates;

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

		[HttpPost]
		public async Task<IActionResult> Add(AddAdViewModel model)
		{
			if (!adService.DoesCategoryExist(model.CategoryId))
			{
				ModelState.AddModelError("Category", "The category does not exist");
			}

			if (!ModelState.IsValid || model == null)
			{
				return View(model);
			}

			await adService.AddOnHttpPost(model, GetUserId());
			return RedirectToAction("All", "Ad");
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			EditAdViewModel? model = await adService.GetModelForEdit(id);

			if (model == null)
			{
				return BadRequest();
			}

			if (model.OwnerId != GetUserId())
			{
				return Unauthorized();
			}

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(EditAdViewModel viewModel, int id)
		{
			EditAdViewModel? model = await adService.GetModelForEdit(id);

			if (model == null)
			{
				return BadRequest();
			}

			if (model.OwnerId != GetUserId())
			{
				return Unauthorized();
			}

			if (!adService.DoesCategoryExist(viewModel.CategoryId))
			{
				ModelState.AddModelError(nameof(model.CategoryId), "Category does not exist!");
				return View(model);
			}

			await adService.EditModel(viewModel, id);

			return RedirectToAction("All", "Ad");
		}

		public async Task<IActionResult> AddToCart(int id)
		{
			if (!adService.DoesAdExist(id))
			{
				return BadRequest();
			}

			string currentUser = GetUserId();

			if (adService.AdAlreadyExist(id, currentUser))
			{
				return RedirectToAction("All", "Ad");
			}

			await adService.AddAdToCollection(id, currentUser);
			return RedirectToAction("Cart", "Ad");
		}

		public async Task<IActionResult> RemoveFromCart(int id)
		{
			if (!adService.DoesAdExist(id))
			{
				return BadRequest();
			}

			string currentUser = GetUserId();

			if (!adService.AdAlreadyExist(id, currentUser))
			{
				return RedirectToAction("Cart", "Ad");
			}

			await adService.RemoveFromCollection(id, currentUser);
			return RedirectToAction("All", "Ad");
		}
	}
}
