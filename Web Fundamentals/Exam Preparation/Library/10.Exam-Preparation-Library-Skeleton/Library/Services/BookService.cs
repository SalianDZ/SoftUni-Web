using Library.Data;
using Library.Data.Models;
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

        public async Task AddBookToCollectionByIdAsync(int bookId, string userId)
        {
            IdentityUserBook iub = new()
            {
                BookId = bookId,
                CollectorId = userId
            };

            await context.UsersBooks.AddAsync(iub);
            await context.SaveChangesAsync();
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

        public async Task<bool> IsBookInUserCollectionAsync(int bookId, string userId)
        {
            bool result = await context.UsersBooks.AnyAsync(ub => ub.CollectorId == userId && ub.BookId == bookId);
            return result;
        }

        public async Task<bool> IsBookValidByIdAsync(int bookId)
        {
            bool result = await context.Books.AnyAsync(b => b.Id == bookId);
            return result;
        }

        public async Task RemoveBookFromCollectionByIdAsync(int bookId, string userId)
        {
            IdentityUserBook iub = await context.UsersBooks.FirstAsync(ub => ub.BookId == bookId && ub.CollectorId == userId);

            context.UsersBooks.Remove(iub);
            await context.SaveChangesAsync();
        }
    }
}
