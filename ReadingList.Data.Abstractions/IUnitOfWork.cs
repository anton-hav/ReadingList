using ReadingList.Data.Abstractions.Repositories;
using ReadingList.DataBase.Entities;

namespace ReadingList.Data.Abstractions;

public interface IUnitOfWork
{
    Task<int> Commit();
}