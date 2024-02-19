using Microsoft.EntityFrameworkCore.Query;
using SeminarHub.Models.Seminar;

namespace SeminarHub.Services.Contracts
{
    public interface ISeminarService
    {
        Task<IEnumerable<SeminarViewModel>> GetAllSeminarsAsync();

        Task CreateSeminar(SeminarFormViewModel model, string userId);

        Task<IEnumerable<SeminarViewModel>> GetAllJoinedSeminarsByIdAsync(string userId);

        Task<bool> IsSeminarValidByIdAsync(int id);

        Task<bool> IsSeminarAlreadyInCollectionByIdAsync(int seminarId, string userId);

        Task AddSeminarToUserCollectionByIdAsync(int seminarId, string userId);

        Task RemoveSeminarFromUserCollectionByIdAsync(int seminarId, string userId);

        Task<SeminarFormViewModel> GetSeminarFormModelByIdAsync(int seminarId);

        Task<bool> IsUserOwnerOfSeminarByIdAsync(int seminarId, string userId);

        Task EditSeminarByIdAsync(int seminarId, SeminarFormViewModel model);

        Task<SeminarDetailsViewModel> GetSeminarViewModelForDetailsByIdAsync(int seminarId);

        Task<SeminarDeleteViewModel> GetSeminarDeleteViewModeForDeleteByAsync(int seminarId);

        Task DeleteSeminarByIdAsync(int seminarId);
    }
}
