using HouseRentingSystem.Services.Data.Interfaces;
using HouseRentingSystem.Web.Infrastructure.Extensions;
using HouseRentingSystem.Web.ViewModels.Agent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;
using static HouseRentingSystem.Common.NotificationMessagesConstants;

namespace HouseRentingSystem.Web.Controllers
{
    [Authorize]
    public class AgentController : Controller
    {
        private readonly IAgentService agentService;
        public AgentController(IAgentService agentService)
        {
            this.agentService = agentService;
        }

		[HttpGet]
		public async Task<IActionResult> Become()
        {
            string? userId = User.GetId();
            bool isAgent = await agentService.AgentExistByUserIdAsync(userId);

            if (isAgent)
            {
                TempData[ErrorMessage] = "You are already an agent!";

                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Become(BecomeAgentFormModel model)
        {
            string? userId = User.GetId();
            bool isAgent = await agentService.AgentExistByUserIdAsync(userId);

            if (isAgent)
            {
                TempData[ErrorMessage] = "You are already an agent!";

                return RedirectToAction("Index", "Home");
            }

            bool isPhoneNumberTaken = await agentService.AgentExistsByPhoneNumberAsync(model.PhoneNumber);

            if (isPhoneNumberTaken)
            {
                ModelState.AddModelError(nameof(model.PhoneNumber), "Agent with the provided phone number already exists!");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool userHasActiveRents = await agentService.HasRentsByUserIdAsync(userId);

            if (userHasActiveRents) 
            {
                TempData["ErrorMessage"] = "You must not have any active rents in order to be an agent!";
                return RedirectToAction("Mine", "House");
            }

            try
            {
                await agentService.Create(userId, model);
            }
            catch (Exception)
            {
                TempData[ErrorMessage] = "An unexpected error occured while registering you as a new agent. Please try again later!";

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("All", "House");
        }
    }
}
