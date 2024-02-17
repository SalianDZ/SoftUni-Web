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

        public BookController(IBookService bookService)
        {
            this.bookService = bookService;
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
    }
}
