using SeminarHub.Models.Category;

namespace SeminarHub.Services.Contracts
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync();

        Task<bool> IsCategoryValidByIdAsync(int id);
    }
}