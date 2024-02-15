using System.ComponentModel.DataAnnotations;
using static Homies.Data.ValidationConstants.TypeValidations;

namespace Homies.Data.Models
{
	public class Type
	{
		[Key]
        public int Id { get; set; }

		[Required]
		[MaxLength(NameMaxLength)]
		public string Name { get; set; } = null!;

		public ICollection<Event> Events { get; set; } = new HashSet<Event>();
    }
}