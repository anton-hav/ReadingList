using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ReadingList.DataBase;

public class ReadingListDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        
    }

    public ReadingListDbContext(DbContextOptions<ReadingListDbContext> options)
        : base(options)
    {
    }

}