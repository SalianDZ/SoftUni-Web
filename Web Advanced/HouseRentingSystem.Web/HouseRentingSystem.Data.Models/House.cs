using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static HouseRentingSystem.Common.EntityValidationConstants.House;

namespace HouseRentingSystem.Data.Models
{
    public class House
    {
        public House()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(AddressMaxLength)]
        public string Address { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Required]
        [MaxLength(ImageUrlMaxLength)]
        public string ImageUrl { get; set; } = null!;

        public decimal PricePerMonth { get; set; }

        public DateTime CreatedOn { get; set; }

        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; } = null!;

        public Guid AgentId { get; set; }

        [ForeignKey(nameof(AgentId))]
        public virtual Agent Agent { get; set; } = null!;

        public Guid? RenterId { get; set; }

        public ApplicationUser? Renter { get; set; }
    }
}
