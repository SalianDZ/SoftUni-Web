using System.ComponentModel.DataAnnotations;
using static SeminarHub.Common.EntityValidations.CategoryValidations;

namespace SeminarHub.Data.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        public ICollection<Seminar> Seminars { get; set; } = new HashSet<Seminar>();
    }
}
