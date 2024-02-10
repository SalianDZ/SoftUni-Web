using NuGet.Protocol.Core.Types;
using System.ComponentModel.DataAnnotations;
using static SoftUniBazar.Data.ValidationConstants.Category;

namespace SoftUniBazar.Data.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        public IEnumerable<Ad> Ads { get; set; } = new List<Ad>();
    }
}