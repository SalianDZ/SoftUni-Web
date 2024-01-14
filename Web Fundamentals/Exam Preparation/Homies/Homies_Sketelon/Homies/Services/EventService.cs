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

		public async Task AddEventToCollection(int id, string userId)
		{

			if (context.Events.First(x => x.Id == id).OrganiserId != userId)
			{
				EventParticipant eventParticipant = new EventParticipant()
				{
					EventId = id,
					HelperId = userId
				};

				await context.EventsParticipants.AddAsync(eventParticipant);
				await context.SaveChangesAsync();
			}
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

		public bool DoesExist(int id)
		{
			return context.Events.Any(e => e.Id == id);
		}

		public async Task EditEventAsync(int id, EditEventViewModel model)
		{
			Event? wantedEvent = await context.Events.FirstOrDefaultAsync(e => e.Id == id);

			if (wantedEvent != null) 
			{
				wantedEvent.Name = model.Name;
				wantedEvent.Description = model.Description;
				wantedEvent.Start = DateTime.Parse(model.Start);
				wantedEvent.End = DateTime.Parse(model.End);
				wantedEvent.TypeId = model.TypeId;

				await context.SaveChangesAsync();
			}
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

		public async Task<EditEventViewModel> GetEventByIdAsync(int id)
		{
			List<TypeViewModel> types = await GetTypesAsync();

			EditEventViewModel? model = await context.Events
				.Where(e => e.Id == id)
				.Select(x => new EditEventViewModel
				{
					Name = x.Name,
					Description = x.Description,
					OwnerId = x.OrganiserId,
					Start = x.Start.ToString("yyyy-MM-dd H:mm"),
					End = x.End.ToString("yyyy-MM-dd H:mm"),
					CreatedOn = x.CreatedOn.ToString("yyyy-MM -dd H:mm"),
					TypeId = x.TypeId,
					Types = types
				}).FirstOrDefaultAsync();

			return model;
		}

		public async Task<EventDetailsViewModel> GetEventDetails(int id)
		{
			EventDetailsViewModel model = await context.Events
				.Where(e => e.Id == id)
				.Select(e => new EventDetailsViewModel
				{
					Id = e.Id,
					Name = e.Name,
					Description = e.Description,
					Start = e.Start.ToString("yyyy-MM-dd H:mm"),
					End = e.End.ToString("yyyy-MM-dd H:mm"),
					CreatedOn = e.CreatedOn.ToString("yyyy-MM-dd H:mm"),
					Organiser = e.Organiser.UserName,
					Type = e.Type.Name
				}).FirstAsync();

			return model;
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

		public bool IsAlreadyAdded(int id, string userId)
		{
			return context.EventsParticipants.Any(ep => ep.EventId == id && ep.HelperId == userId);
		}

		public async Task RemoveEventFromCollection(int id, string userId)
		{
			EventParticipant eventParticipant = new EventParticipant()
			{
				EventId = id,
				HelperId = userId
			};

			context.EventsParticipants.Remove(eventParticipant);
			await context.SaveChangesAsync();
		}
	}
}
