using System.ComponentModel.DataAnnotations; 

namespace Forum.ViewModels.Post
{
	public class PostAddFormModel
	{
		[Required]
		[MinLength(10)]
		[MaxLength(50)]
		public string Title { get; set; } = null!;

		[Required]
		[MinLength(30)]
		[MaxLength(1500)]
		public string Content { get; set; } = null!;
    }
}
