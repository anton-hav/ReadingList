namespace ReadingList.WebAPI.Models.Responses;

/// <summary>
/// Book response model 
/// </summary>
public class GetBookResponseModel
{
    /// <summary>
    /// Book unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Book title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Book author unique identifier
    /// </summary>
    public Guid AuthorId { get; set; }

    /// <summary>
    /// Book category unique identifier
    /// </summary>
    public Guid CategoryId { get; set; }
}