using Homies.Models.Type;
using System.ComponentModel.DataAnnotations;
using static Homies.Data.ValidationConstants.EventValidations;

namespace Homies.Models.Event
{
	public class EventFormViewModel
	{
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
		public string Name { get; set; } = null!;

        [Required]
		[StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
		public string Description { get; set; } = null!;

        [Required]
        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }

        [Required]
        public int TypeId { get; set; }

        public IEnumerable<TypeViewModel> Types { get; set; } = new List<TypeViewModel>();
    }
}
