using System.ComponentModel.DataAnnotations;
using TaskBoardApp.Web.ViewModels.Board;
using static TaskBoardApp.Common.EntityValidationConstants.Task;

namespace TaskBoardApp.Web.ViewModels.Task
{
	public class TaskFormModel
	{
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength, ErrorMessage ="The title must be at least 7 symbols long!")]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = "The description must be at least 7 symbols long!")]
        public string Description { get; set; } = null!;

        [Display(Name ="Board")]
        public int BoardId { get; set; }

        public IEnumerable<BoardSelectViewModel>? AllBoards { get; set; }
    }
}
