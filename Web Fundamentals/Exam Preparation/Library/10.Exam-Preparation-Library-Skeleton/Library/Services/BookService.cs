using Library.Data;
using Library.Models.Book;
using Library.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Library.Services
{
    public class BookService : IBookService
    {
        private readonly LibraryDbContext context;

        public BookService(LibraryDbContext context)
        {
            this.context = context; 
        }

        public async Task<IEnumerable<BookViewModel>> GetAllBooksAsync()
        {
            IEnumerable<BookViewModel> books =
                await context.Books
                .Include(b => b.Category)
                .Select(b => new BookViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Author = b.Author,
                    Rating = b.Rating,
                    Category = b.Category.Name,
                    ImageUrl = b.ImageUrl
                }).ToArrayAsync();

            return books;
        }

        public async Task<IEnumerable<MineBookViewModel>> GetUserBooksByIdAsync(string userId)
        {
            IEnumerable<MineBookViewModel> books = 
                await context.UsersBooks
                .Include(b => b.Book)
                .ThenInclude(b => b.Category)
                .Where(ub => ub.CollectorId == userId)
                .Select(ub => new MineBookViewModel
            {
                Id = ub.Book.Id,
                Title = ub.Book.Title,
                Description = ub.Book.Description,
                Author = ub.Book.Author,
                Rating = ub.Book.Rating,
                Category = ub.Book.Category.Name,
                ImageUrl = ub.Book.ImageUrl
            }).ToArrayAsync();

            return books;
        }
    }
}
