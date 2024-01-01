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

		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{
			TaskEditGetViewModel model = await taskService.GetForEditByIdAsync(id);
			TaskFormModel modelForm = new TaskFormModel()
			{ 
				Title = model.Title,
				Description = model.Description,
				BoardId = model.BoardId,
				AllBoards = await boardService.AllForSelectAsync(),
			};

			return View(modelForm);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(string id, TaskFormModel taskModel)
		{
			if (!ModelState.IsValid)
			{
				taskModel.AllBoards = await boardService.AllForSelectAsync();
				return View(taskModel);
			}

			bool doesBoardExist = await boardService.ExistsByIdAsync(taskModel.BoardId);
			if (!doesBoardExist)
			{
				ModelState.AddModelError(nameof(taskModel.BoardId), "Selected board does not exist!");
				taskModel.AllBoards = await boardService.AllForSelectAsync();
				return View(taskModel);
			}

			TaskEditGetViewModel model = await taskService.GetForEditByIdAsync(id);

			await taskService.EditAsync(model, taskModel);
			return RedirectToAction("All", "Board");
		}
	}
}
