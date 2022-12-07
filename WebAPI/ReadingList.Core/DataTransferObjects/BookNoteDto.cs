namespace ReadingList.Core.DataTransferObjects;

public class BookNoteDto
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public BookDto Book { get; set; }
    public ReadingPriority Priority { get; set; }
    public ReadingStatus Status { get; set; }
}