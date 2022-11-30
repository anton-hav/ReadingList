using Microsoft.EntityFrameworkCore;
using System.Data;
using ReadingList.DataBase.Entities;

namespace ReadingList.DataBase;

public class ReadingListDbContext : DbContext
{
    public DbSet<Author> Authors {get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<BookNote> BookNotes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Category>()
            .HasIndex(category => category.Name)
            .IsUnique();

        builder.Entity<Book>()
            .HasIndex(book => new
            {
                book.AuthorId,
                book.Title
            })
            .IsUnique();

        builder.Entity<BookNote>()
            .HasIndex(note => note.BookId)
            .IsUnique();
    }

    public ReadingListDbContext(DbContextOptions<ReadingListDbContext> options)
        : base(options)
    {
    }

}