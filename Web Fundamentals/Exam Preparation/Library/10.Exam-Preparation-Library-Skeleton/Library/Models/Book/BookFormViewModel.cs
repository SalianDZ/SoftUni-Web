using Library.Models.Category;
using System.ComponentModel.DataAnnotations;
using static Library.Common.ValidationConstants.BookValidation;

namespace Library.Models.Book
{
    public class BookFormViewModel
    {
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(AuthorMaxLength, MinimumLength = TitleMinLength)]
        public string Author { get; set; } = null!;

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength)]
        public string Description { get; set; } = null!;

        [Required]
        [StringLength(UrlMaxLength)]
        public string Url { get; set; } = null!;

        [Required]
        [Range(RatingMinValue, RatingMaxValue)]
        public decimal Rating { get; set; }

        public int CategoryId { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; } = new HashSet<CategoryViewModel>();
    }
}
