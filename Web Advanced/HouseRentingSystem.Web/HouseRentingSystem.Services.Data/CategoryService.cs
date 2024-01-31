using HouseRentingSystem.Data;
using HouseRentingSystem.Services.Data.Interfaces;
using HouseRentingSystem.Web.ViewModels.Category;
using Microsoft.EntityFrameworkCore;

namespace HouseRentingSystem.Services.Data
{
    public class CategoryService : ICategoryService
    {
        private readonly HouseRentingDbContext context;

        public CategoryService(HouseRentingDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<HouseSelectCategoryFormModel>> AllCategoriesAsync()
        {
            IEnumerable<HouseSelectCategoryFormModel> allCategories = await context.Categories.Select(c => new HouseSelectCategoryFormModel
            {
                Id = c.Id,
                Name = c.Name
            })
                .AsNoTracking()
                .ToArrayAsync();

            return allCategories;
        }
    }
}
