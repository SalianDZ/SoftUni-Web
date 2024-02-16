using Homies.Models.Event;

namespace Homies.Services.Contracts
{
	public interface IEventService
	{
		Task<IEnumerable<EventViewModel>> GetAllEventsAsync();

		Task<IEnumerable<EventViewModel>> GetJoinedEventsByIdAsync(string userId);

		Task CreateEventAsync(EventFormViewModel model, string userId);

		Task<EventFormViewModel?> GetEventFormModelByIdAsync(int id);

		Task<EventEditViewModel?> GetEventEditViewModelByIdAsync(int id);

		Task<bool> IsCurrentUserOwnerOfEventByIdAsync(string userId, int eventId);

		Task EditEventByIdAsync(int id, EventFormViewModel model);

		Task<bool> DoesEventExistByIdAsync(int id);

		Task<bool> IsEventAlreadyJoinedByUserAsync(string userId, int eventId);

		Task JoinEventByIdAsync(string userId, int eventId);

		Task LeaveEventByIdAsync(string userId, int eventId);

		Task<EventDetailsViewModel> GetEventDetailsModelByIdAsync(int id);
	}
}
