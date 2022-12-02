namespace ReadingList.DataBase.Entities;

public class Author : IBaseEntity
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public List<Book> Books { get; set; }
}