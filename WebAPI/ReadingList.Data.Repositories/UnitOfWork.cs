using ReadingList.Data.Abstractions;
using ReadingList.Data.Abstractions.Repositories;
using ReadingList.DataBase;
using ReadingList.DataBase.Entities;

namespace ReadingList.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ReadingListDbContext _dbContext;
    public IRepository<Author> Authors { get; }
    public IRepository<Category> Categories { get; }
    public IRepository<Book> Books { get; }
    public IRepository<BookNote> BookNotes { get; }

    public UnitOfWork(ReadingListDbContext dbContext, 
        IRepository<Author> authors, 
        IRepository<Category> categories, 
        IRepository<Book> books, 
        IRepository<BookNote> bookNotes)
    {
        _dbContext = dbContext;
        Authors = authors;
        Categories = categories;
        Books = books;
        BookNotes = bookNotes;
    }

    public async Task<int> Commit()
    {
        return await _dbContext.SaveChangesAsync();
    }
}