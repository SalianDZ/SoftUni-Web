using Library.Models.Book;

namespace Library.Contracts
{
	public interface IBookService
	{
		Task AddBookAsync(AddBookViewModel model);
		Task AddBookToCollectionAsync(string userId, BookViewModel book);
		Task<IEnumerable<AllBookViewModel>> GetAllBooksAsync();
		Task<BookViewModel?> GetBookByIdAsync(int id);
		Task<IEnumerable<MineBookViewModel>> GetMineBooksAsync(string userId);
		Task<AddBookViewModel> GetNewAddBookModelAsync();
		Task RemoveBookFromCollection(string userId, BookViewModel book);
	}
}
