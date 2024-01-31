using HouseRentingSystem.Services.Data.Interfaces;
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
        public async Task<IActionResult> All()
        {
            return View();
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
    }
}
