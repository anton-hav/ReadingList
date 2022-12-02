namespace ReadingList.DataBase.Entities;

public class Book : IBaseEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; }

    public Guid AuthorId { get; set; }
    public Author Author { get; set; }

    public Guid CategoryId {get; set; }
    public Category Categories { get; set; }

    public BookNote BookNote { get; set; }
}