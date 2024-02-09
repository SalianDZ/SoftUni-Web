using System.Web.Mvc;
using HouseRentingSystem.Services.Data.Interfaces;
using HouseRentingSystem.Web.Infrastructure.Extensions;
using HouseRentingSystem.Web.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;
using Controller = Microsoft.AspNetCore.Mvc.Controller;

namespace HouseRentingSystem.Web.Controllers
{
	[Authorize]
	public class CategoryController : Controller
	{
		private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

		public async Task<IActionResult> All()
		{
			IEnumerable<AllCategoriesViewModel	> viewModel = await categoryService.AllCategoriesForListAsync();
			return View(viewModel);
		}

		public async Task<IActionResult> Details(int id, string information)
		{
			bool categoryExists = await categoryService.ExistsByIdAsync(id);

			if (!categoryExists) 
			{
				return NotFound();
			}
			CategoryDetailsViewModel viewModel = await categoryService.GetDetailsByIdAsync(id);
			if (!categoryExists || viewModel.GetUrlInformation() != information)
			{
				return NotFound();
			}

			return View(viewModel);
		}
	}
}
