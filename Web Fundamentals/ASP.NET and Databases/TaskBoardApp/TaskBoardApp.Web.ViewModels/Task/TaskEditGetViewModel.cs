using TaskBoardApp.Web.ViewModels.Board;

namespace TaskBoardApp.Web.ViewModels.Task
{
	public class TaskEditGetViewModel
	{
		public string Id { get; set; } = null!;

		public string Title { get; set; } = null!;

		public string Description { get; set; } = null!;

		public int BoardId { get; set; }

		public IEnumerable<BoardSelectViewModel>? AllBoards { get; set; }
	}
}
