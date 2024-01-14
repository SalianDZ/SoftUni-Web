using System.ComponentModel.DataAnnotations;
using static Homies.Common.EntityValidations.TypeValidations;

namespace Homies.Data.Models
{
	public class Type
	{
		[Key]
        public int Id { get; set; }

		[Required]
		[MaxLength(NameMaxLength)]
		public string Name { get; set; } = null!;

		public List<Event> Events { get; set; } = new List<Event>();
    }
}
