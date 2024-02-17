using Humanizer.Localisation.TimeToClockNotation;
using Library.Extensions;
using Library.Models.Book;
using Library.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private readonly IBookService bookService;
        private readonly ICategoryService categoryService;

        public BookController(IBookService bookService, ICategoryService categoryService)
        {
            this.bookService = bookService;
            this.categoryService = categoryService;
        }

        public async Task<IActionResult> All()
        {
            IEnumerable<BookViewModel> model = await bookService.GetAllBooksAsync();
            return View(model);
        }

        public async Task<IActionResult> Mine()
        {
            string userId = User.GetUserId();
            IEnumerable<MineBookViewModel> model = await bookService.GetUserBooksByIdAsync(userId);
            return View(model);
        }

        public async Task<IActionResult> AddToCollection(int id)
        {
            bool isBookValid = await bookService.IsBookValidByIdAsync(id);
            if (!isBookValid)
            {
                return RedirectToAction("All", "Book");
            }

            bool isBookInUserCollection = await bookService.IsBookInUserCollectionAsync(id, User.GetUserId());

            if (isBookInUserCollection)
            {
                return RedirectToAction("All", "Book");
            }

            await bookService.AddBookToCollectionByIdAsync(id, User.GetUserId());
            return RedirectToAction("Mine", "Book");
        }

        public async Task<IActionResult> RemoveFromCollection(int id)
        {
            bool isBookValid = await bookService.IsBookValidByIdAsync(id);
            if (!isBookValid)
            {
                return RedirectToAction("All", "Book");
            }

            bool isBookInUserCollection = await bookService.IsBookInUserCollectionAsync(id, User.GetUserId());

            if (!isBookInUserCollection)
            {
                return RedirectToAction("All", "Book");
            }

            await bookService.RemoveBookFromCollectionByIdAsync(id, User.GetUserId());
            return RedirectToAction("Mine", "Book");
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            BookFormViewModel model = new BookFormViewModel();
            model.Categories = await categoryService.GetAllCategoriesAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(BookFormViewModel model)
        {
            if (model == null)
            {
                return RedirectToAction("All", "Book");
            }

            bool isCategoryValid = await categoryService.IsCategoryByIdValidAsync(model.CategoryId);

            if (!isCategoryValid)
            {
                ModelState.AddModelError(nameof(model.CategoryId), "Please select a valid category!");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await categoryService.GetAllCategoriesAsync();
                return View(model);
            }

            await bookService.AddBook(model);
            return RedirectToAction("All", "Book");
        }
    }
}
