using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBoardApp.Extensions;
using TaskBoardApp.Services.Interfaces;
using TaskBoardApp.Web.ViewModels.Task;

namespace TaskBoardApp.Controllers
{
	[Authorize]
	public class TaskController : Controller
	{
		private readonly IBoardService boardService;
		private readonly ITaskService taskService;

        public TaskController(IBoardService boardService, ITaskService taskService)
        {
			this.taskService = taskService;
			this.boardService = boardService;
		}

		[HttpGet]
        public async Task<IActionResult> Create()
		{
			TaskFormModel viewModel = new TaskFormModel() 
			{
				AllBoards = await boardService.AllForSelectAsync()
			};

			return View(viewModel);	
		}

		[HttpPost]
		public async Task<IActionResult> Create(TaskFormModel model)
		{
			if (!ModelState.IsValid)
			{
				model.AllBoards = await boardService.AllForSelectAsync(); 
				return View(model);
			}

			bool doesBoardExist = await boardService.ExistsByIdAsync(model.BoardId);
			if (!doesBoardExist)
			{
				ModelState.AddModelError(nameof(model.BoardId), "Selected board does not exist!");
				model.AllBoards = await boardService.AllForSelectAsync();
				return View(model);
			}

			string currentUserId = User.GetId();

			await taskService.AddAsync(currentUserId, model);

			return this.RedirectToAction("All", "Board");
		}

		[HttpGet]
		public async Task<IActionResult> Details(string id)
		{
			try
			{
				TaskDetailsViewModel viewModel = await taskService.GetForDetailsByIdAsync(id);
				return View(viewModel);
			}
			catch (Exception)
			{
				return RedirectToAction("All", "Board");
			}
		}
	}
}
