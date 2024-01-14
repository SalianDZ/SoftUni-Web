using Library.Contracts;
using Library.Data;
using Library.Data.Models;
using Library.Models.Book;
using Library.Models.Category;
using Microsoft.EntityFrameworkCore;

namespace Library.Services
{
	public class BookService : IBookService
	{
		private readonly LibraryDbContext context;

        public BookService(LibraryDbContext dbContext)
        {
			context = dbContext;
        }

		public async Task AddBookAsync(AddBookViewModel model)
		{
			Book book = new Book()
			{
				Title = model.Title,
				Author = model.Author,
				ImageUrl = model.Url,
				Description = model.Description,
				CategoryId = model.CategoryId,
				Rating = Decimal.Parse(model.Rating)
			};

			await context.Books.AddAsync(book);
			await context.SaveChangesAsync();
		}

		public async Task AddBookToCollectionAsync(string userId, BookViewModel book)
		{
			bool alreadyAdded = await context.IdentityUserBooks.AnyAsync(ub => ub.CollectorId == userId && ub.BookId == book.Id);

            if (!alreadyAdded)
            {
				var userBook = new IdentityUserBook
				{
					CollectorId = userId,
					BookId = book.Id
				};

				await context.IdentityUserBooks.AddAsync(userBook);
				await context.SaveChangesAsync();
			}
        }

		public async Task<IEnumerable<AllBookViewModel>> GetAllBooksAsync()
		{
			IEnumerable<AllBookViewModel> allBooks = await context.Books
				.Select(book => new AllBookViewModel()
				{
					Id = book.Id,
					Title = book.Title,
					Author = book.Author,
					ImageUrl = book.ImageUrl,
					Rating = book.Rating,
					Category = book.Category.Name
				}).ToListAsync();

			return allBooks;
		}

		public async Task<BookViewModel?> GetBookByIdAsync(int id)
		{
			return await context.Books
				.Where(book => book.Id == id)
				.Select(book => new BookViewModel()
				{
					Id = book.Id,
					Title = book.Title,
					Author = book.Author,
					ImageUrl = book.ImageUrl,
					Rating = book.Rating,
					Description = book.Description,
					CategoryId = book.CategoryId
				}).FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<MineBookViewModel>> GetMineBooksAsync(string userId)
		{
			return await context.IdentityUserBooks
				.Where(ub => ub.CollectorId == userId)
				.Select(book => new MineBookViewModel()
				{
					Id = book.Book.Id,
					Title = book.Book.Title,
					Author = book.Book.Author,
					Description = book.Book.Description,
					ImageUrl = book.Book.ImageUrl,
					Category = book.Book.Category.Name
				}).ToListAsync();
		}

		public async Task<AddBookViewModel> GetNewAddBookModelAsync()
		{
			var categories = await context.Categories
				.Select(c => new CategoryViewModel()
				{
					Id = c.Id,
					Name = c.Name
				}).ToListAsync();

			AddBookViewModel bookViewModel = new AddBookViewModel()
			{
				Categories = categories
			};

			return bookViewModel;
		}

		public async Task RemoveBookFromCollection(string userId, BookViewModel book)
		{
			var userBook = await context.IdentityUserBooks
				.FirstOrDefaultAsync(ub => ub.CollectorId == userId && ub.BookId == book.Id);

			if (userBook != null) 
			{
				context.IdentityUserBooks.Remove(userBook);
				await context.SaveChangesAsync();
			}
		}
	}
}
