using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeminarHub.Extensions;
using SeminarHub.Models.Seminar;
using SeminarHub.Services.Contracts;

namespace SeminarHub.Controllers
{
    [Authorize]
    public class SeminarController : Controller
    {
        private readonly ISeminarService seminarService;
        private readonly ICategoryService categoryService;
        public SeminarController(ISeminarService seminarService, ICategoryService categoryService)
        {
            this.seminarService = seminarService;
            this.categoryService = categoryService;
        }
        public async Task<IActionResult> All()
        {
            IEnumerable<SeminarViewModel> model = await seminarService.GetAllSeminarsAsync();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            SeminarFormViewModel model = new();
            model.Categories = await categoryService.GetAllCategoriesAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(SeminarFormViewModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            DateTime parsedDate;
            bool isDateValid = DateTime.TryParse(model.DateAndTime, out parsedDate);

            if (!isDateValid) 
            {
                ModelState.AddModelError(nameof(model.DateAndTime), "Please enter a valid date!");
            }

            bool isCategoryValid = await categoryService.IsCategoryValidByIdAsync(model.CategoryId);
            if (!isCategoryValid)
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Please select a valid category!");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await categoryService.GetAllCategoriesAsync();
                return View(model);
            }

            await seminarService.CreateSeminar(model, User.GetUserId());
            return RedirectToAction("All", "Seminar");
        }

        public async Task<IActionResult> Joined()
        {
            IEnumerable<SeminarViewModel> model = await seminarService.GetAllJoinedSeminarsByIdAsync(User.GetUserId());
            return View(model);
        }

        public async Task<IActionResult> Join(int id)
        {
            string userId = User.GetUserId();
            bool isSeminarValid = await seminarService.IsSeminarValidByIdAsync(id);
            bool isSeminarAlreadyAdded = await seminarService.IsSeminarAlreadyInCollectionByIdAsync(id, userId);

            if (!isSeminarValid)
            {
                return BadRequest();
            }

            if (isSeminarAlreadyAdded)
            {
                return RedirectToAction("All", "Seminar");
            }

            bool isCurrentUserOwnerOfSeminar = await seminarService.IsUserOwnerOfSeminarByIdAsync(id, userId);

            if (isCurrentUserOwnerOfSeminar)
            {
                return RedirectToAction("All", "Seminar");
            }

            await seminarService.AddSeminarToUserCollectionByIdAsync(id, userId);
            return RedirectToAction("Joined", "Seminar");
        }

        public async Task<IActionResult> Leave(int id)
        {
            string userId = User.GetUserId();
            bool isSeminarValid = await seminarService.IsSeminarValidByIdAsync(id);
            bool isSeminarAlreadyAdded = await seminarService.IsSeminarAlreadyInCollectionByIdAsync(id, userId);

            if (!isSeminarValid)
            {
                return BadRequest();
            }

            if (!isSeminarAlreadyAdded)
            {
                return RedirectToAction("All", "Seminar");
            }

            bool isCurrentUserOwnerOfSeminar = await seminarService.IsUserOwnerOfSeminarByIdAsync(id, userId);

            if (isCurrentUserOwnerOfSeminar)
            {
                return RedirectToAction("Joined", "Seminar");
            }

            await seminarService.RemoveSeminarFromUserCollectionByIdAsync(id, userId);
            return RedirectToAction("Joined", "Seminar");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            bool isSeminarValid = await seminarService.IsSeminarValidByIdAsync(id);

            if (!isSeminarValid)
            {
                return BadRequest();
            }

            string userId = User.GetUserId();   
            bool isCurrentUserOwnerOfSeminar = await seminarService.IsUserOwnerOfSeminarByIdAsync(id, userId);

            if (!isCurrentUserOwnerOfSeminar)
            {
                return Unauthorized();
            }

            SeminarFormViewModel modelForEdit = await seminarService.GetSeminarFormModelByIdAsync(id);
            modelForEdit.Categories = await categoryService.GetAllCategoriesAsync();
            return View(modelForEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, SeminarFormViewModel model)
        {
            bool isSeminarValid = await seminarService.IsSeminarValidByIdAsync(id);

            if (!isSeminarValid || model == null)
            {
                return BadRequest();
            }

            string userId = User.GetUserId();
            bool isCurrentUserOwnerOfSeminar = await seminarService.IsUserOwnerOfSeminarByIdAsync(id, userId);

            if (!isCurrentUserOwnerOfSeminar)
            {
                return Unauthorized();
            }

            DateTime parsedDate;
            bool isDateValid = DateTime.TryParse(model.DateAndTime, out parsedDate);

            if (!isDateValid)
            {
                ModelState.AddModelError(nameof(model.DateAndTime), "Please enter a valid date!");
            }

            bool isCategoryValid = await categoryService.IsCategoryValidByIdAsync(model.CategoryId);
            if (!isCategoryValid)
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Please select a valid category!");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await categoryService.GetAllCategoriesAsync();
                return View(model);
            }

            await seminarService.EditSeminarByIdAsync(id, model);
            return RedirectToAction("All", "Seminar");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            bool isSeminarValid = await seminarService.IsSeminarValidByIdAsync(id);

            if (!isSeminarValid)
            {
                return BadRequest();
            }

            SeminarDetailsViewModel model = await seminarService.GetSeminarViewModelForDetailsByIdAsync(id);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            bool isSeminarValid = await seminarService.IsSeminarValidByIdAsync(id);

            if (!isSeminarValid)
            {
                return BadRequest();
            }

            string userId = User.GetUserId();
            bool isCurrentUserOwnerOfSeminar = await seminarService.IsUserOwnerOfSeminarByIdAsync(id, userId);

            if (!isCurrentUserOwnerOfSeminar)
            {
                return Unauthorized();
            }

            SeminarDeleteViewModel model = await seminarService.GetSeminarDeleteViewModeForDeleteByAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool isSeminarValid = await seminarService.IsSeminarValidByIdAsync(id);

            if (!isSeminarValid)
            {
                return BadRequest();
            }

            string userId = User.GetUserId();
            bool isCurrentUserOwnerOfSeminar = await seminarService.IsUserOwnerOfSeminarByIdAsync(id, userId);

            if (!isCurrentUserOwnerOfSeminar)
            {
                return Unauthorized();
            }

            try
            {
                await seminarService.DeleteSeminarByIdAsync(id);
            }
            catch (Exception)
            {
                return RedirectToAction("All", "Seminar");
            }

            return RedirectToAction("All", "Seminar");
        }
    }
}
