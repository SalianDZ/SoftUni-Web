using HouseRentingSystem.Services.Data.Interfaces;
using HouseRentingSystem.Services.Data.Models.House;
using HouseRentingSystem.Web.Infrastructure.Extensions;
using HouseRentingSystem.Web.ViewModels.House;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static HouseRentingSystem.Common.NotificationMessagesConstants;

namespace HouseRentingSystem.Web.Controllers
{
    [Authorize]
    public class HouseController : Controller
    {
        private readonly IHouseService houseService;
        private readonly ICategoryService categoryService;
        private readonly IAgentService agentService;
        public HouseController(IHouseService houseService, ICategoryService categoryService, IAgentService agentService)
        {
            this.houseService = houseService;
            this.categoryService = categoryService;
            this.agentService = agentService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> All([FromQuery]AllHousesQueryModel queryModel)
        {
            AllHousesFilteredAndPagedServiceModel serviceModel =
                await houseService.AllAsync(queryModel);

            queryModel.Houses = serviceModel.Houses;
            queryModel.TotalHouses = serviceModel.TotalHousesCount;
            queryModel.Categories = await categoryService.AllCategoryNamesAsync();
            return View(queryModel);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            string userId = User.GetId()!;
            bool isAgent = await agentService.AgentExistByUserIdAsync(userId);

            if (!isAgent)
            {
                TempData[ErrorMessage] = "You must become an agent in order to add new houses!";
                return RedirectToAction("Become", "Agent");
            }

            HouseFormModel model = new HouseFormModel()
            { 
                Categories = await categoryService.AllCategoriesAsync(),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(HouseFormModel model)
        {
			string userId = User.GetId()!;
			bool isAgent = await agentService.AgentExistByUserIdAsync(userId);

			if (!isAgent)
			{
				TempData[ErrorMessage] = "You must become an agent in order to add new houses!";
				return RedirectToAction("Become", "Agent");
			}

            bool categoryExists = await categoryService.ExistsByIdAsync(model.CategoryId);

            if (!categoryExists) 
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Selected category does not exist!");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await categoryService.AllCategoriesAsync();
                return View(model);
            }

            try
            {
                string? agentId = await agentService.AgentIdByUserIdAsync(userId);
                await houseService.CreateAsync(model, agentId!);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Unexpected error occured while trying to add your new house. Please try again later!");
				model.Categories = await categoryService.AllCategoriesAsync();
				return View(model);
            }

            return RedirectToAction("All", "House");
		}
    }
}
