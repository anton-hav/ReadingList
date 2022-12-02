namespace ReadingList.Core.DataTransferObjects;

public class AuthorDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public List<BookDto> Books { get; set; }
}