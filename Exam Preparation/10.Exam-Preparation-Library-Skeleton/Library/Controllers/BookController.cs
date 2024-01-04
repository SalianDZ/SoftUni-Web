using Library.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

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
	}
}
