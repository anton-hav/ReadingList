using ReadingList.Data.Abstractions;
using ReadingList.Data.Abstractions.Repositories;
using ReadingList.DataBase;
using ReadingList.DataBase.Entities;

namespace ReadingList.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ReadingListDbContext _dbContext;
    
    public UnitOfWork(ReadingListDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> Commit()
    {
        return await _dbContext.SaveChangesAsync();
    }
}