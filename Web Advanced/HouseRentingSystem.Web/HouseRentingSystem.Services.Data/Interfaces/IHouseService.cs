using HouseRentingSystem.Services.Data.Models.House;
using HouseRentingSystem.Web.ViewModels.Home;
using HouseRentingSystem.Web.ViewModels.House;

namespace HouseRentingSystem.Services.Data.Interfaces
{
    public interface IHouseService
    {
        Task<IEnumerable<IndexViewModel>> LastFreeHousesAsync();

        Task<string> CreateAndReturnIdAsync(HouseFormModel formModel, string agentId);

        Task<AllHousesFilteredAndPagedServiceModel> AllAsync(AllHousesQueryModel queryModel);

        Task<IEnumerable<HouseAllViewModel>> AllByAgentIdAsync(string agentId);

        Task<IEnumerable<HouseAllViewModel>> AllByUserIdAsync(string userId);

        Task<HouseDetailsViewModel> GetDetailsByIdAsync(string houseId);

        Task<bool> ExistsByIdAsync(string houseId);

        Task<HouseFormModel> GetHouseForEditByIdAsync(string houseId);

        Task<bool> IsAgentWithIdOwnerOfHouseWithIdAsync(string houseId, string agentId);

        Task EditHouseAsync(string id, HouseFormModel formModel);
    }
}
