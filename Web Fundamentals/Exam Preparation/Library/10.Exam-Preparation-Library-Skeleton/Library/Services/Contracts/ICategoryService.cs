using Library.Models.Category;
using Microsoft.EntityFrameworkCore;

namespace Library.Services.Contracts
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync();

        Task<bool> IsCategoryByIdValidAsync(int id);
    }
}
