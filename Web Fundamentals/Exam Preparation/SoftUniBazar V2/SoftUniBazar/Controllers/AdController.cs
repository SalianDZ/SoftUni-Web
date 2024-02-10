using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftUniBazar.Extensions;
using SoftUniBazar.Models.Ad;
using SoftUniBazar.Services.Interfaces;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;

namespace SoftUniBazar.Controllers
{
    [Authorize]
    public class AdController : Controller
    {
        private readonly IAdService adService;
        private readonly ICategoryService categoryService;

        public AdController(IAdService adService, ICategoryService categoryService)
        {
            this.adService = adService;
            this.categoryService = categoryService;
        }

        public async Task<IActionResult> All()
        {
            IEnumerable<AdViewModel> allAds = await adService.GetAllAdsAsync();
            return View(allAds);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            AdFormViewModel model = new AdFormViewModel();
            model.Categories = await categoryService.GetAllCategoriesAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AdFormViewModel model)
        {
            bool doesCategoryExists =
                await categoryService.IsCategoryValidByIdAsync(model.CategoryId);

            if (model == null)
            {
                return RedirectToAction("All", "Ad");
            }

            if (!doesCategoryExists)
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Select a valid category");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await categoryService.GetAllCategoriesAsync();
                return View(model);
            }

            await adService.CreateAdAsync(model, User.GetUserId());
            return RedirectToAction("All", "Ad");   
        }

        public async Task<IActionResult> Cart()
        {
            IEnumerable<AdViewModel> model = await adService.GetAdsForCartByIdAsync(User.GetUserId());
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            AdFormViewModel? model = await adService.GetModelForEditAsync(id);

            if (model == null)
            {
                return RedirectToAction("All", "Ad");
            }

            bool isUserOwnerOfTheAdd = await adService.IsUserByIdOwnerOfTheAd(User.GetUserId(), id);

            if (!isUserOwnerOfTheAdd)
            {
                return RedirectToAction("Cart", "Ad");
            }

            model.Categories = await categoryService.GetAllCategoriesAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdFormViewModel model, int id)
        {
            if (model == null)
            {
                return RedirectToAction("All", "Ad");
            }

            bool isUserOwnerOfTheAdd = await adService.IsUserByIdOwnerOfTheAd(User.GetUserId(), id);

            if (!isUserOwnerOfTheAdd)
            {
                return RedirectToAction("Cart", "Ad");
            }

            bool doesCategoryExists =
                await categoryService.IsCategoryValidByIdAsync(model.CategoryId);

            if (!doesCategoryExists)
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Select a valid category");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await categoryService.GetAllCategoriesAsync();
                return View(model);
            }

            await adService.EditAdByIdAsync(model, id);
            return RedirectToAction("All", "Ad");
        }

        public async Task<IActionResult> AddToCart(int id)
        {
            string userId = User.GetUserId();
            bool doesAdExist = await adService.DoesAdExistByIdAsync(id);
            bool isAlreadyAdded = await adService.AdAlreadyAddedByIdAsync(id, userId);

            if (!doesAdExist || isAlreadyAdded) 
            {
                return RedirectToAction("All", "Ad");
            }

            await adService.AddAdToUserCollectionAsync(id, userId);
            return RedirectToAction("Cart", "Ad");
        }

        public async Task<IActionResult> RemoveFromCart(int id)
        {
            string userId = User.GetUserId();
            bool doesAdExist = await adService.DoesAdExistByIdAsync(id);
            bool isAlreadyAdded = await adService.AdAlreadyAddedByIdAsync(id, userId);

            if (!doesAdExist || !isAlreadyAdded)
            {
                return RedirectToAction("Cart", "Ad");
            }

            await adService.RemoveAdFromUserCollectionAsync(id, userId);
            return RedirectToAction("Cart", "Ad");
        }
    }
}
