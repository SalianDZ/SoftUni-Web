using Homies.Models.Event;
using Homies.Models.Type;

namespace Homies.Services.Interfaces
{
	public interface IEventService
	{
		Task<IEnumerable<AllEventViewModel>> GetAllEventsAsync();

		Task<IEnumerable<AllEventViewModel>> GetJoinedEventsAsync(string userId);

		Task<AddEventViewModel> GetModelWithTypes();

		Task<List<TypeViewModel>> GetTypesAsync();

		Task CreateEvent(AddEventViewModel model, string ownerId);

		Task<EditEventViewModel> GetEventByIdAsync(int id);

		Task EditEventAsync(int id, EditEventViewModel model);
	}
}
