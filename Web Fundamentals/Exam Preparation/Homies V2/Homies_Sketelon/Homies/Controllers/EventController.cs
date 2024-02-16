using Homies.Extensions;
using Homies.Models.Event;
using Homies.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Permissions;

namespace Homies.Controllers
{
	[Authorize]
	public class EventController : Controller
	{
		private readonly IEventService eventService;
		private readonly ITypeService typeService;
        public EventController(IEventService eventService, ITypeService typeService)
        {
            this.eventService = eventService;
			this.typeService = typeService;
        }

		[HttpGet]
		public async Task<IActionResult> All()
		{
			IEnumerable<EventViewModel> model = await eventService.GetAllEventsAsync();
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Joined()
		{
			string userId = User.GetUserId();
			IEnumerable<EventViewModel> model = await eventService.GetJoinedEventsByIdAsync(userId);
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Add()
		{ 
			EventFormViewModel model = new EventFormViewModel()
			{ 
				Types = await typeService.GetAllTypesAsync()
			};
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Add(EventFormViewModel model)
		{
            if (model == null)
            {
				return RedirectToAction("All", "Event");
            }

			bool doesTypeExist = await typeService.DoesTypeExistByIdAsync(model.TypeId);

			if (!doesTypeExist)
			{
				ModelState.AddModelError(nameof(model.TypeId), "Please select a valid type for the event!");
			}

            if (!ModelState.IsValid)
			{
				model.Types = await typeService.GetAllTypesAsync();
				return View(model);
			}

			string userId = User.GetUserId();
			await eventService.CreateEventAsync(model, userId);
			return RedirectToAction("All", "Event");
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{ 
			EventFormViewModel? viewModel = await eventService.GetEventFormModelByIdAsync(id);

			if (viewModel == null)
			{
				return RedirectToAction("All", "Event");
			}

			string userId = User.GetUserId();
			bool isCurrentUserOwnerOfEvent = await eventService.IsCurrentUserOwnerOfEventByIdAsync(userId, id);

			if (!isCurrentUserOwnerOfEvent)
			{
				return RedirectToAction("All", "Event");
			}

			viewModel.Types = await typeService.GetAllTypesAsync();
			return View(viewModel);
		}

		public async Task<IActionResult> Edit(EventFormViewModel model, int id)
		{
			bool doesEventExist = await eventService.DoesEventExistByIdAsync(id);
			if (!doesEventExist)
			{
                return RedirectToAction("All", "Event");
            }

			if (model == null)
			{
                return RedirectToAction("All", "Event");
            }

            string userId = User.GetUserId();
            bool isCurrentUserOwnerOfEvent = await eventService.IsCurrentUserOwnerOfEventByIdAsync(userId, id);


            if (!isCurrentUserOwnerOfEvent)
            {
                return RedirectToAction("All", "Event");
            }

            bool doesTypeExist = await typeService.DoesTypeExistByIdAsync(model.TypeId);

            if (!doesTypeExist)
            {
                ModelState.AddModelError(nameof(model.TypeId), "Please select a valid type for the event!");
            }

            if (!ModelState.IsValid)
            {
                model.Types = await typeService.GetAllTypesAsync();
                return View(model);
            }

			await eventService.EditEventByIdAsync(id, model);
			return RedirectToAction("All", "Event");
        }
	}
}
