using SoftUniBazar.Models.Category;
using System.ComponentModel.DataAnnotations;
using static SoftUniBazar.Common.EntityValidations.AdValidations;

namespace SoftUniBazar.Models.Ad
{
	public class AddAdViewModel
	{
		[Required]
		[StringLength(NameMaxLength, MinimumLength = NameMinLength)]
		public string Name { get; set; } = null!;

		[Required]
		[StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
		public string Description { get; set; } = null!;

		[Required]
		[Range(0, double.MaxValue)]
		public decimal Price { get; set; }

		[Required]
		public string ImageUrl { get; set; } = null!;

		[Required]
		public DateTime CreatedOn { get; set; }

		[Required]
		[Range(1, int.MaxValue)]
		public int CategoryId { get; set; }

		public List<CategoryViewModel> Categories { get; set; } = null!;
	}
}
