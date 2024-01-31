using HouseRentingSystem.Web.ViewModels.Agent;

namespace HouseRentingSystem.Services.Data.Interfaces
{
	public interface IAgentService
	{
		Task<bool> AgentExistByUserIdAsync(string userId);

		Task<bool> AgentExistsByPhoneNumberAsync(string phoneNumber);

		Task<bool> HasRentsByUserIdAsync(string userId);

		Task Create(string userId, BecomeAgentFormModel model);

		Task<string?> AgentIdByUserIdAsync(string userId);
	}
}
