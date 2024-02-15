using Homies.Models.Event;

namespace Homies.Services.Contracts
{
	public interface IEventService
	{
		Task<IEnumerable<EventViewModel>> GetAllEventsAsync();

		Task<IEnumerable<EventViewModel>> GetJoinedEventsByIdAsync(string userId);

		Task CreateEventAsync(EventFormViewModel model, string userId);

		Task<EventFormViewModel?> GetEventFormModelByIdAsync(int id);

		Task<bool> IsCurrentUserOwnerOfEventByIdAsync(string userId, int eventId);
	}
}
