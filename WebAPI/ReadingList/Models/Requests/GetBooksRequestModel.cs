namespace ReadingList.WebAPI.Models.Requests;

/// <summary>
/// Request model to get books
/// </summary>
public class GetBooksRequestModel
{
    /// <summary>
    /// Search parameter representing book title
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Search parameter representing the unique identifier of book author 
    /// </summary>
    public Guid? AuthorId { get; set; }

    /// <summary>
    /// Search parameter representing the unique identifier of the book category 
    /// </summary>
    public Guid? CategoryId { get; set; }

    /// <summary>
    /// Search parameter representing the book unique identifier
    /// </summary>
    public Guid? BookNoteId { get; set; }

    /// <summary>
    /// Search parameter representing the number of books on the page
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Search parameter representing the page number
    /// </summary>
    public int PageNumber { get; set; }
}