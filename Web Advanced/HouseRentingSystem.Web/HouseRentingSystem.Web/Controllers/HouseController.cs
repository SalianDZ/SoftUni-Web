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
		private readonly IUserService userService;
        public HouseController(IHouseService houseService, ICategoryService categoryService, IAgentService agentService, IUserService userService)
        {
            this.houseService = houseService;
            this.categoryService = categoryService;
            this.agentService = agentService;
			this.userService = userService;
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

            try
            {
				HouseFormModel model = new HouseFormModel()
				{
					Categories = await categoryService.AllCategoriesAsync(),
				};

				return View(model);
			}
            catch (Exception)
            {
				TempData[ErrorMessage] = "Unexpected error occured! Please try again later";
				return RedirectToAction("Index", "Home");
			}
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
                string houseId = await houseService.CreateAndReturnIdAsync(model, agentId!);
				return RedirectToAction("Details", "House", new { id = houseId });
			}
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Unexpected error occured while trying to add your new house. Please try again later!");
				model.Categories = await categoryService.AllCategoriesAsync();
				return View(model);
            }
		}

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            List<HouseAllViewModel> myHouses = new List<HouseAllViewModel>();

            string userId = User.GetId()!;
            bool isUserAgent = await agentService.AgentExistByUserIdAsync(userId);

            try
            {
				if (User.IsAdmin())
				{
                    string? agentId = await agentService.AgentIdByUserIdAsync(userId);
                    myHouses.AddRange(await houseService.AllByAgentIdAsync(agentId));
                    myHouses.AddRange(await houseService.AllByUserIdAsync(userId));
					myHouses = myHouses.DistinctBy(h => h.Id).ToList();
                }
				else if (isUserAgent)
				{
					string? agentId = await agentService.AgentIdByUserIdAsync(userId);
					myHouses.AddRange(await houseService.AllByAgentIdAsync(agentId));
				}
				else
				{
					myHouses.AddRange(await houseService.AllByUserIdAsync(userId));
				}

				return View(myHouses);
			}
            catch (Exception)
            {
				TempData[ErrorMessage] = "Unexpected error occured! Please try again later";
				return RedirectToAction("Index", "Home");
			}
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(string id)
        { 
            bool houseExists = await houseService.ExistsByIdAsync(id);
            if (!houseExists)
            {
				TempData[ErrorMessage] = "This house does not exist!";
				return RedirectToAction("All", "House");
			}

            try
            {
				HouseDetailsViewModel viewModel = await houseService.GetDetailsByIdAsync(id);
				viewModel.Agent.FullName = await userService.GetFullNameByEmailAsync(User.Identity?.Name!);
				return View(viewModel);
			}
            catch (Exception)
            {
				TempData[ErrorMessage] = "Unexpected error occured! Please try again later";
				return RedirectToAction("Index", "Home");
			}
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
			bool houseExists = await houseService.ExistsByIdAsync(id);
			if (!houseExists)
			{
				TempData[ErrorMessage] = "This house does not exist!";
				return RedirectToAction("All", "House");
			}

            bool isUserAgent = await agentService.AgentExistByUserIdAsync(User.GetId()!);

            if (!isUserAgent && !User.IsAdmin()) 
            {
                TempData["ErrorMessage"] = "You must become an agent in order to edit house info!";
                return RedirectToAction("Become", "Agent");
            }

            string? agentId = await agentService.AgentIdByUserIdAsync(User.GetId()!);
            bool isAgentOwner = await houseService.IsAgentWithIdOwnerOfHouseWithIdAsync(id, agentId!);

            if (!isAgentOwner && !User.IsAdmin())
            {
				TempData[ErrorMessage] = "You must be the agent owner of the house you want to edit!";
				return RedirectToAction("Mine", "House");
			}

            try
            {
				HouseFormModel model = await houseService.GetHouseForEditByIdAsync(id);
				model.Categories = await categoryService.AllCategoriesAsync();
				return View(model);
			}
            catch (Exception)
            {
                TempData[ErrorMessage] = "Unexpected error occured! Please try again later";
                return RedirectToAction("Index", "Home");
            }
		}

		[HttpPost]
		public async Task<IActionResult> Edit(string id, HouseFormModel model)
		{
			bool houseExists = await houseService.ExistsByIdAsync(id);
			if (!houseExists)
			{
				TempData[ErrorMessage] = "This house does not exist!";
				return RedirectToAction("All", "House");
			}

			bool isUserAgent = await agentService.AgentExistByUserIdAsync(User.GetId()!);

			if (!isUserAgent && !User.IsAdmin())
			{
				TempData["ErrorMessage"] = "You must become an agent in order to edit house info!";
				return RedirectToAction("Become", "Agent");
			}

			string? agentId = await agentService.AgentIdByUserIdAsync(User.GetId()!);
			bool isAgentOwner = await houseService.IsAgentWithIdOwnerOfHouseWithIdAsync(id, agentId!);

			if (!isAgentOwner && !User.IsAdmin())
			{
				TempData[ErrorMessage] = "You must be the agent owner of the house you want to edit!";
				return RedirectToAction("Mine", "House");
			}

            if (!ModelState.IsValid)
            {
                model.Categories = await categoryService.AllCategoriesAsync();
                return View(model);
            }

            try
            {
                await houseService.EditHouseAsync(id, model);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Unexpected error occured while trying to update the house. Please try agian later!");
                model.Categories = await categoryService.AllCategoriesAsync();
                return View(model);
            }

            return RedirectToAction("Details", "House", new { id});
		}

        public async Task<IActionResult> Delete(string id)
        {
			bool houseExists = await houseService.ExistsByIdAsync(id);
			if (!houseExists)
			{
				TempData[ErrorMessage] = "This house does not exist!";
				return RedirectToAction("Mine", "House");
			}

			bool isUserAgent = await agentService.AgentExistByUserIdAsync(User.GetId()!);

			if (!isUserAgent && !User.IsAdmin())
			{
				TempData["ErrorMessage"] = "You must become an agent in order to delete this house!";
				return RedirectToAction("Become", "Agent");
			}

			string? agentId = await agentService.AgentIdByUserIdAsync(User.GetId()!);
			bool isAgentOwner = await houseService.IsAgentWithIdOwnerOfHouseWithIdAsync(id, agentId!);

			if (!isAgentOwner && !User.IsAdmin())
			{
				TempData[ErrorMessage] = "You must be the agent owner of the house you want to delete!";
				return RedirectToAction("Mine", "House");
			}

			try
			{
				HousePreDeleteDetailsViewModel viewModel = await houseService.GetHouseForDeleteByIdAsync(id);
                return View(viewModel); 
			}
			catch (Exception)
			{
				ModelState.AddModelError(string.Empty, "Unexpected error occured while trying to delete the house. Please try again later!");
                return RedirectToAction("Mine", "House");
			}

		}

        [HttpPost]
        public async Task<IActionResult> Delete(string id, HousePreDeleteDetailsViewModel model)
        {
			bool houseExists = await houseService.ExistsByIdAsync(id);
			if (!houseExists)
			{
				TempData[ErrorMessage] = "This house does not exist!";
				return RedirectToAction("Mine", "House");
			}

			bool isUserAgent = await agentService.AgentExistByUserIdAsync(User.GetId()!);

			if (!isUserAgent && !User.IsAdmin())
			{
				TempData["ErrorMessage"] = "You must become an agent in order to delete this house!";
				return RedirectToAction("Become", "Agent");
			}

			string? agentId = await agentService.AgentIdByUserIdAsync(User.GetId()!);
			bool isAgentOwner = await houseService.IsAgentWithIdOwnerOfHouseWithIdAsync(id, agentId!);

			if (!isAgentOwner && !User.IsAdmin())
			{
				TempData[ErrorMessage] = "You must be the agent owner of the house you want to delete!";
				return RedirectToAction("Mine", "House");
			}

			try
			{
				await houseService.DeleteHouseByIdAsync(id);
				TempData["WarningMessage"] = "The house was succcessfully deleted!";
				return RedirectToAction("Mine", "House");
			}
			catch (Exception)
			{
				ModelState.AddModelError(string.Empty, "Unexpected error occured while trying to delete the house. Please try again later!");
				return RedirectToAction("All", "House");
			}
		}

		public async Task<IActionResult> Rent(string id)
		{
			bool houseExists = await houseService.ExistsByIdAsync(id);

			if (!houseExists)
			{
				TempData[ErrorMessage] = "House with the provided id does not exist!";
				return RedirectToAction("All", "House");
			}

			bool isHouseRented = await houseService.IsRentedByIdAsync(id);

			if (isHouseRented)
			{
				TempData[ErrorMessage] = "House with the provided id has already been rented!";
				return RedirectToAction("All", "House");
			}

			bool isUserAgent = await agentService.AgentExistByUserIdAsync(User.GetId()!);

			if (isUserAgent && !User.IsAdmin())
			{
				TempData[ErrorMessage] = "Agents cannot rent houses! You need to be a user in order to rent houses!";
				return RedirectToAction("Index", "Home");
			}

			try
			{
				await houseService.RentHouseAsync(id, User.GetId()!);
				return RedirectToAction("Mine", "House");
			}
			catch (Exception)
			{
				TempData[ErrorMessage] = "Unexpected error occured while trying to rent the house. Please try again later!";
				return RedirectToAction("All", "House");
			}
		}

		public async Task<IActionResult> Leave(string id)
		{
			bool houseExists = await houseService.ExistsByIdAsync(id);

			if (!houseExists)
			{
				TempData[ErrorMessage] = "House with the provided id does not exist!";
				return RedirectToAction("All", "House");
			}

			bool isHouseRented = await houseService.IsRentedByIdAsync(id);

			if (!isHouseRented)
			{
				TempData[ErrorMessage] = "House with the provided id is not rented by you!";
				return RedirectToAction("Mine", "House");
			}

			bool isCurrentUserRenterOfTheHouse = await houseService.IsRentedByUserWithIdAsync(id, User.GetId()!);

			if (!isCurrentUserRenterOfTheHouse)
			{
				TempData[ErrorMessage] = "You must be the renter of the house in order to leave it!";
				return RedirectToAction("Mine", "House");
			}

			try
			{
				await houseService.LeaveHouseAsync(id);
				return RedirectToAction("Mine", "House");
			}
			catch (Exception)
			{
				TempData[ErrorMessage] = "Unexpected error occured while trying to leave the house. Please try again later!";
				return RedirectToAction("Mine", "House");
			}
		}
	}
}
