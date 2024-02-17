using Library.Models.Book;

namespace Library.Services.Contracts
{
    public interface IBookService
    {
        Task<IEnumerable<BookViewModel>> GetAllBooksAsync();

        Task<IEnumerable<MineBookViewModel>> GetUserBooksByIdAsync(string userId);
    }
}
