using Microsoft.EntityFrameworkCore;
using TaskBoardApp.Data;
using TaskBoardApp.Services.Interfaces;
using TaskBoardApp.Web.ViewModels.Board;
using TaskBoardApp.Web.ViewModels.Task;

namespace TaskBoardApp.Services
{
    public class BoardService : IBoardService
    {
        private readonly TaskBoardDbContext dbContext;
        public BoardService(TaskBoardDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

		public async Task<IEnumerable<BoardAllViewModel>> AllAsync()
        {
            IEnumerable<BoardAllViewModel> allBoards = await dbContext.Boards.Select(x => new BoardAllViewModel()
            {
                Name = x.Name,
                Tasks = x.Tasks.Select(t => new TaskViewModel()
                {
                    Id = t.Id.ToString(),
                    Title = t.Title,
                    Description = t.Description,
                    Owner = t.Owner.UserName
                }).ToList(),
            }).ToListAsync();

            return allBoards;
        }

		public async Task<IEnumerable<BoardSelectViewModel>> AllForSelectAsync()
		{
            IEnumerable<BoardSelectViewModel> allBoards = await dbContext.Boards
                .Select(b => new BoardSelectViewModel()
                {
                    Id = b.Id,
                    Name = b.Name
                }).ToArrayAsync();

            return allBoards;
		}

		public async Task<bool> ExistsByIdAsync(int id)
		{
            bool result = await dbContext.Boards.AnyAsync(b => b.Id == id);
            return result;
		}
	}
}
