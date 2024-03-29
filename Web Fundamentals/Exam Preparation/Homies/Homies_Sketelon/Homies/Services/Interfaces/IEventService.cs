﻿using Homies.Models.Event;
using Homies.Models.Type;
using System.Runtime.CompilerServices;

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

		bool DoesExist(int id);

		bool IsAlreadyAdded(int id, string userId);

		Task AddEventToCollection(int id, string userId);

		Task RemoveEventFromCollection(int id, string userId);

		Task<EventDetailsViewModel> GetEventDetails(int id);
	}
}
