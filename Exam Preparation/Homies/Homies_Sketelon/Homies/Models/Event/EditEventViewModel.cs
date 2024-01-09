using Homies.Models.Type;
using System.ComponentModel.DataAnnotations;
using static Homies.Common.EntityValidations.EventValidations;

namespace Homies.Models.Event
{
	public class EditEventViewModel
	{
		[Required]
		[StringLength(NameMaxLength, MinimumLength = NameMinLength)]
		public string Name { get; set; } = null!;

		[Required]
		[StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
		public string Description { get; set; } = null!;

		[Required]
		public string OwnerId { get; set; } = null!;

        public string CreatedOn { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-dd H:mm");

		[Required]
		public string Start { get; set; } = null!;

		[Required]
		public string End { get; set; } = null!;

		public int TypeId { get; set; }

		public List<TypeViewModel> Types { get; set; } = new List<TypeViewModel>();
	}
}
