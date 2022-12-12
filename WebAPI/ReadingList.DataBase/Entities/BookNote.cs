using ReadingList.Core;

namespace ReadingList.DataBase.Entities;

public class BookNote : IBaseEntity
{
    public Guid Id { get; set; }

    public Guid BookId { get; set; }
    public Book Book { get; set; }
    public ReadingPriority Priority { get; set; }
    public ReadingStatus Status { get; set; }
}