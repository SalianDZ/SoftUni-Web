using Microsoft.EntityFrameworkCore;
using TaskBoardApp.Data;
using TaskBoardApp.Services.Interfaces;
using TaskBoardApp.Web.ViewModels.Task;

namespace TaskBoardApp.Services
{
	public class TaskService : ITaskService
	{
		private readonly TaskBoardDbContext dbContext;
        public TaskService(TaskBoardDbContext dbContext)
        {
			this.dbContext = dbContext;
        }

        public async Task AddAsync(string ownerId, TaskFormModel viewModel)
		{
			Data.Models.Task task = new Data.Models.Task()
			{
				Title = viewModel.Title,
				Description = viewModel.Description,
				BoardId = viewModel.BoardId,
				CreatedOn = DateTime.UtcNow,
				OwnerId = ownerId
			};

			await dbContext.Tasks.AddAsync(task);
			await dbContext.SaveChangesAsync();
		}

		public async Task<TaskDetailsViewModel> GetForDetailsByIdAsync(string id)
		{
			TaskDetailsViewModel viewModel = await dbContext
				.Tasks
				.Select(t => new TaskDetailsViewModel()
				{
					Id = t.Id.ToString(),
					Title = t.Title,
					Description = t.Description,
					Owner = t.Owner.UserName,
					CreatedOn = t.CreatedOn.ToString("f"),
					Board = t.Board.Name
				})
				.FirstAsync(t => t.Id == id);

			return viewModel;
		}

		public async Task<TaskEditGetViewModel> GetForEditByIdAsync(string id)
		{
			TaskEditGetViewModel viewModel = await dbContext
				.Tasks
				.Select(t => new TaskEditGetViewModel()
				{
					Id = t.Id.ToString(),
					Title = t.Title,
					Description = t.Description,
					BoardId = t.BoardId
				})
				.FirstAsync(t => t.Id == id);

			return viewModel;
		}
	}
}
