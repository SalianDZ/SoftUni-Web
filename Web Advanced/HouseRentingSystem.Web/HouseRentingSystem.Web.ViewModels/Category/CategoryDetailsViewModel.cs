using HouseRentingSystem.Web.ViewModels.Category.Interfaces;

namespace HouseRentingSystem.Web.ViewModels.Category
{
	public class CategoryDetailsViewModel : ICategoryDetailsModel
	{
		public string Name { get; set; } = null!;

		public int Id { get; set; }
    }
}
