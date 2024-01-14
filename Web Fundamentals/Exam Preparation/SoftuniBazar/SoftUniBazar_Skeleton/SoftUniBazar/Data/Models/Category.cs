using System.ComponentModel.DataAnnotations;
using static SoftUniBazar.Common.EntityValidations.CategoryValidations;

namespace SoftUniBazar.Data.Models
{
	public class Category
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(NameMaxLength)]
		public string Name { get; set; } = null!;

        public virtual List<Ad> Ads { get; set; } = new List<Ad>();
    }
}