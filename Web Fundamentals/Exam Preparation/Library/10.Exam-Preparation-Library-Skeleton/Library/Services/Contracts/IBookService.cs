using Library.Models.Book;

namespace Library.Services.Contracts
{
    public interface IBookService
    {
        Task<IEnumerable<BookViewModel>> GetAllBooksAsync();

        Task<IEnumerable<MineBookViewModel>> GetUserBooksByIdAsync(string userId);

        Task<bool> IsBookInUserCollectionAsync(int bookId, string userId);

        Task<bool> IsBookValidByIdAsync(int bookId);

        Task AddBookToCollectionByIdAsync(int bookId, string userId);

        Task RemoveBookFromCollectionByIdAsync(int bookId, string userId);

        Task AddBook(BookFormViewModel model);
    }
}
