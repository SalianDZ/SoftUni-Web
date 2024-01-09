using Homies.Data;
using Homies.Data.Models;
using Homies.Models.Event;
using Homies.Models.Type;
using Homies.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Homies.Services
{
	public class EventService : IEventService
	{
		private readonly HomiesDbContext context;

        public EventService(HomiesDbContext context)
        {
			this.context = context;
        }

		public async Task CreateEvent(AddEventViewModel model, string ownerId)
		{
			Event newEvent = new Event()
			{ 
				Name = model.Name,
				Description = model.Description,
				Start = model.Start,
				End = model.End,
				CreatedOn = DateTime.UtcNow,
				TypeId = model.TypeId,
				OrganiserId = ownerId
			};

			await context.Events.AddAsync(newEvent);
			await context.SaveChangesAsync();
		}

		public async Task<IEnumerable<AllEventViewModel>> GetAllEventsAsync()
		{
			List<AllEventViewModel> models = await context.Events
				.Select(e => new AllEventViewModel
				{
					Id = e.Id,
					Name = e.Name,
					Start = e.Start.ToString("yyyy-MM-dd H:mm"),
					Type = e.Type.Name,
					Organiser = e.Organiser.UserName
				})
				.ToListAsync();

			return models;
		}

		public async Task<IEnumerable<AllEventViewModel>> GetJoinedEventsAsync(string userId)
		{
			List<AllEventViewModel> models = await context.EventsParticipants
				.Where(ep => ep.HelperId == userId)
				.Select(ep => new AllEventViewModel
				{
					Id = ep.Event.Id,
					Name = ep.Event.Name,
					Start = ep.Event.Start.ToString("yyyy-MM-dd H:mm"),
					Type = ep.Event.Type.Name,
					Organiser = ep.Event.Organiser.UserName
				}).ToListAsync();

			return models;
		}

		public async Task<AddEventViewModel> GetModelWithTypes()
		{
			List<TypeViewModel> types = await context.Types
				.Select(t => new TypeViewModel
				{
					Id = t.Id,
					Name = t.Name
				}).ToListAsync();

			AddEventViewModel model = new AddEventViewModel()
			{
				Types = types
			};

			return model;
		}

		public async Task<List<TypeViewModel>> GetTypesAsync()
		{
			List<TypeViewModel> types = await context.Types.Select(t => new TypeViewModel
			{
				Id = t.Id,
				Name = t.Name
			}).ToListAsync();

			return types;
		}
	}
}
