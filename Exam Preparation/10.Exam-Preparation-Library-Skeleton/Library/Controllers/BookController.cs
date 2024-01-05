using Library.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Library.Models.Book;

namespace Library.Controllers
{
	public class BookController : BaseController
	{
		private readonly IBookService bookService;

        public BookController(IBookService bookService)
        {
			this.bookService = bookService;
        }

        public async Task<IActionResult> AllAsync()
		{
			var model = await bookService.GetAllBooksAsync();
			return View(model);
		}

		public async Task<IActionResult> Mine()
		{
			var model = await bookService.GetMineBooksAsync(GetUserId());
			return View(model);
		}

		public async Task<IActionResult> AddToCollection(int id)
		{
			var book = await bookService.GetBookByIdAsync(id);

			if (book == null)
			{
				return RedirectToAction("All", "Book");
			}

			var userId = GetUserId();

			await bookService.AddBookToCollectionAsync(userId, book);

			return RedirectToAction("All", "Book");
		}

		public async Task<IActionResult> RemoveFromCollection(int id)
		{
			var book = await bookService.GetBookByIdAsync(id);

			if (book == null)
			{
				return RedirectToAction("Mine", "Book");
			}

			var userId = GetUserId();

			await bookService.RemoveBookFromCollection(userId, book);

			return RedirectToAction("Mine", "Book");
		}

		[HttpGet]
		public async Task<IActionResult> Add()
		{
			AddBookViewModel model = await bookService.GetNewAddBookModelAsync();
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Add(AddBookViewModel model)
		{
			decimal rating;

			if (!decimal.TryParse(model.Rating, out rating) || rating < 0 || rating > 10)
			{
				ModelState.AddModelError(nameof(model.Rating), "Rating must be a number between 0 and 10.");
				return View(model);
			}


			if (!ModelState.IsValid)
			{
				return View(model);
			}

			await bookService.AddBookAsync(model);
			return RedirectToAction("All", "Book");
		}
	}
}
