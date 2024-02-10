using Microsoft.EntityFrameworkCore;
using SoftUniBazar.Data;
using SoftUniBazar.Models.Category;
using SoftUniBazar.Services.Interfaces;

namespace SoftUniBazar.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly BazarDbContext dbContext;

        public CategoryService(BazarDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<CategoryViewModel>> GetAllCategoriesAsync()
        {
            IEnumerable<CategoryViewModel> allCategories = await dbContext.Categories
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToListAsync();

            return allCategories;
        }

        public async Task<bool> IsCategoryValidByIdAsync(int id)
        {
            bool result = await dbContext.Categories.AnyAsync(c => c.Id == id);
            return result;
        }
    }
}
