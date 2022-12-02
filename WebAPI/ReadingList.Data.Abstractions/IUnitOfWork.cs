using ReadingList.Data.Abstractions.Repositories;
using ReadingList.DataBase.Entities;

namespace ReadingList.Data.Abstractions;

public interface IUnitOfWork
{
    IRepository<Author> Authors { get; }
    IRepository<Category> Categories { get; }
    IRepository<Book> Books { get; }
    IRepository<BookNote> BookNotes { get; }

    Task<int> Commit();
}