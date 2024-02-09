using HouseRentingSystem.Data;
using HouseRentingSystem.Data.Models;
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

		public async Task<IEnumerable<AllCategoriesViewModel>> AllCategoriesForListAsync()
		{
            IEnumerable<AllCategoriesViewModel> allCategories = await context.Categories
                .AsNoTracking()
                .Select(c => new AllCategoriesViewModel 
                {
                    Id= c.Id,
                    Name = c.Name
                }).ToArrayAsync();

            return allCategories;
		}

		public async Task<IEnumerable<string>> AllCategoryNamesAsync()
		{
			IEnumerable<string> allNames = await context
                .Categories
                .Select(c => c.Name)
                .ToArrayAsync();

            return allNames;
		}

		public async Task<bool> ExistsByIdAsync(int id)
		{
			bool result = await context.Categories.AnyAsync(c => c.Id == id);
            return result;
		}

		public async Task<CategoryDetailsViewModel> GetDetailsByIdAsync(int id)
		{
            Category category = await context.Categories.FirstAsync(c => c.Id == id);
			CategoryDetailsViewModel viewModel = new CategoryDetailsViewModel() 
            {
                Id = category.Id,
                Name = category.Name
            };
            return viewModel;
		}
	}
}
