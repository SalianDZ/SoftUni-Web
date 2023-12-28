using TaskBoardApp.Web.ViewModels.Board;

namespace TaskBoardApp.Services.Interfaces
{
    public interface IBoardService
    {
        Task<IEnumerable<BoardAllViewModel>> AllAsync();
    }
}
