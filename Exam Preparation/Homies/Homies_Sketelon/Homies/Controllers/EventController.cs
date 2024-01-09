using Homies.Models.Event;
using Homies.Models.Type;
using Homies.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NuGet.Frameworks;

namespace Homies.Controllers
{
	public class EventController : BaseController
	{
		private readonly IEventService eventService;
        public EventController(IEventService eventService)
        {
            this.eventService = eventService;
        }

        public async Task<IActionResult> All()
		{
			IEnumerable<AllEventViewModel> events = await eventService.GetAllEventsAsync();
			return View(events);
		}

		public async Task<IActionResult> Joined() 
		{
			string userId = GetUserId();
			IEnumerable<AllEventViewModel> events = await eventService.GetJoinedEventsAsync(userId);
			return View(events);
		}

		[HttpGet]
		public async Task<IActionResult> Add()
		{
			AddEventViewModel viewModel = await eventService.GetModelWithTypes();
			return View(viewModel);
		}

		[HttpPost]
		public async Task<IActionResult> Add(AddEventViewModel model)
		{
			var types = await eventService.GetTypesAsync();

			if (!types.Any(t => t.Id == model.TypeId))
			{
				ModelState.AddModelError(nameof(model.TypeId), "Type does not exist!");
			}

			if (!ModelState.IsValid)
			{
				return View(model);
			}


			await eventService.CreateEvent(model, GetUserId());
			return RedirectToAction("All", "Event");
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{ 
			var model = await eventService.GetEventByIdAsync(id);

			if (model == null)
			{
				return BadRequest();
			}

			if (GetUserId() != model.OwnerId)
			{
				return Unauthorized();
			}

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, EditEventViewModel model)
		{
			var types = await eventService.GetTypesAsync();
			if (!types.Any(t => t.Id == model.TypeId))
			{
				ModelState.AddModelError(nameof(model.TypeId), "Type does not exist!");
			}

			await eventService.EditEventAsync(id, model);
			
			return RedirectToAction("All", "Event");
		}

		public async Task<IActionResult> Join(int id)
		{
			if (!eventService.DoesExist(id))
			{
				return BadRequest();
			}

			if (eventService.IsAlreadyAdded(id, GetUserId()))
			{
				return RedirectToAction("All", "Event");
			}

			await eventService.RemoveEventFromCollection(id, GetUserId());

			return RedirectToAction("Joined", "Event");
		}

		public async Task<IActionResult> Leave(int id)
		{
			if (!eventService.DoesExist(id))
			{
				return BadRequest();
			}

			if (!eventService.IsAlreadyAdded(id, GetUserId()))
			{
				return RedirectToAction("All", "Event");
			}

			await eventService.RemoveEventFromCollection(id, GetUserId());

			return RedirectToAction("Joined", "Event");
		}
	}
}
