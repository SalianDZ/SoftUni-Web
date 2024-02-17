using Library.Data;
using Library.Models.Category;
using Library.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Library.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly LibraryDbContext context;

        public CategoryService(LibraryDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync()
        {
            IEnumerable<CategoryViewModel> categories = await context.Categories.Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name
            }).ToArrayAsync();

            return categories;
        }

        public async Task<bool> IsCategoryByIdValidAsync(int id)
        {
            bool result = await context.Categories.AnyAsync(c => c.Id == id);
            return result;
        }
    }
}
