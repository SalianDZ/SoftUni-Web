using TaskBoardApp.Web.ViewModels.Board;
using TaskBoardApp.Web.ViewModels.Task;

namespace TaskBoardApp.Services.Interfaces
{
    public interface IBoardService
    {
        Task<IEnumerable<BoardAllViewModel>> AllAsync();

        //Not implemented yet.
		Task<BoardAllViewModel> AddAsync();

		Task<BoardAllViewModel> AddAsync(TaskViewModel model);
    }
}
