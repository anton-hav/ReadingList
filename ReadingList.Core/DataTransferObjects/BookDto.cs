namespace ReadingList.Core.DataTransferObjects;

public class BookDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }

    public Guid AuthorId { get; set; }
    public AuthorDto Author { get; set; }

    public Guid CategoryId {get; set; }
    public CategoryDto Categories { get; set; }

    public BookNoteDto BookNote { get; set; }
}