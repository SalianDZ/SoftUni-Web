using SoftUniBazar.Models.Category;

namespace SoftUniBazar.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync();

        Task<bool> IsCategoryValidByIdAsync(int id);
    }
}
