using Homies.Data;
using Homies.Data.Models;
using Homies.Models.Event;
using Homies.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using static Homies.Data.ValidationConstants.EventValidations;

namespace Homies.Services
{
	public class EventService : IEventService
	{
		private readonly HomiesDbContext context;
        public EventService(HomiesDbContext context)
        {
            this.context = context;
        }

		public async Task CreateEventAsync(EventFormViewModel model, string userId)
		{
			Event createdEvent = new Event()
			{
				Name = model.Name,
				Description = model.Description,
				CreatedOn = DateTime.Now,
				Start = model.Start,
				End = model.End,
				OrganiserId = userId,
				TypeId = model.TypeId
			};

			await context.Events.AddAsync(createdEvent);
			await context.SaveChangesAsync();
		}

		public async Task<IEnumerable<EventViewModel>> GetAllEventsAsync()
		{
			IEnumerable<EventViewModel> allEvents = await context.Events
				.Include(e => e.Type)
				.Select(e => new EventViewModel
			{
				Id = e.Id,
				Name = e.Name,
				Start = e.Start.ToString(DateFormat),
				Type = e.Type.Name,
				Organiser = e.Organiser.UserName
			}).ToListAsync();

			return allEvents;
		}

		public async Task<EventFormViewModel?> GetEventFormModelByIdAsync(int id)
		{
			EventFormViewModel? model = await context.Events.Where(e => e.Id == id).Select(e => new EventFormViewModel
			{
				Name = e.Name,
				Description = e.Description,
				Start = e.Start,
				End = e.End,
				TypeId = e.TypeId
			}).FirstOrDefaultAsync();

			return model;
		}

		public async Task<IEnumerable<EventViewModel>> GetJoinedEventsByIdAsync(string userId)
		{
			IEnumerable<EventViewModel> joinedEvents = await context.EventsParticipants
				.Include(e => e.Event)
				.Include(e => e.Event.Type)
				.Select(e => new EventViewModel
			{
				Id = e.Event.Id,
				Name = e.Event.Name,
				Start = e.Event.Start.ToString(DateFormat),
				Type = e.Event.Type.Name,
				Organiser = e.Event.Organiser.UserName
			}).ToListAsync();

			return joinedEvents;
		}

		public async Task<bool> IsCurrentUserOwnerOfEventByIdAsync(string userId, int eventId)
		{
			Event currentEvent = await context.Events.FirstAsync(e => e.Id == eventId);
			return userId == currentEvent.OrganiserId;
		}
	}
}
