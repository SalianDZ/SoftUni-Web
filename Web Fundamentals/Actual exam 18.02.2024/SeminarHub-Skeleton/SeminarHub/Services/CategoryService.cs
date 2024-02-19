using Microsoft.EntityFrameworkCore;
using SeminarHub.Data;
using SeminarHub.Models.Category;
using SeminarHub.Services.Contracts;

namespace SeminarHub.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly SeminarHubDbContext context;

        public CategoryService(SeminarHubDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync()
        {
            IEnumerable<CategoryViewModel> allCategories = await context.Categories.Select(c => new CategoryViewModel
            {
                Id = c.Id,
                Name = c.Name
            }).ToArrayAsync();

            return allCategories;   
        }

        public async Task<bool> IsCategoryValidByIdAsync(int id)
        {
            bool result = await context.Categories.AnyAsync(c => c.Id == id);
            return result;
        }
    }
}
