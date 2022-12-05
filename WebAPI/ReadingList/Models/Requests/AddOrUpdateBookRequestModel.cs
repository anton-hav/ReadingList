using ReadingList.DataBase.Entities;

namespace ReadingList.WebAPI.Models.Requests;

/// <summary>
/// Request model to create or update book
/// </summary>
public class AddOrUpdateBookRequestModel
{
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