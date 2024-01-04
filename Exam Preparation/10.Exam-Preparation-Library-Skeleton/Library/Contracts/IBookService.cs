using Library.Models.Book;

namespace Library.Contracts
{
	public interface IBookService
	{
		Task AddBookToCollectionAsync(string userId, BookViewModel book);
		Task<IEnumerable<AllBookViewModel>> GetAllBooksAsync();
		Task<BookViewModel?> GetBookByIdAsync(int id);
		Task<IEnumerable<MineBookViewModel>> GetMineBooksAsync(string userId);

		Task RemoveBookFromCollection(string userId, BookViewModel book);
	}
}
