using ReadingList.Core.DataTransferObjects;
using ReadingList.Core;

namespace ReadingList.WebAPI.Models.Requests;

/// <summary>
/// Request model to get book notes count.
/// </summary>
public class GetBookNotesCountRequestModel
{
    /// <summary>
    /// Unique identifier of the book's author
    /// </summary>
    public Guid? AuthorId { get; set; }

    /// <summary>
    /// Unique identifier of the book's category
    /// </summary>
    public Guid? CategoryId { get; set; }

    /// <summary>
    /// Book reading priority from Never to Right Now
    /// </summary>
    public ReadingPriority? Priority { get; set; }

    /// <summary>
    /// Book reading status
    /// (Scheduled = 0, InProgress = 1, Completed = 2, Skipped = 3)
    /// </summary>
    public ReadingStatus? Status { get; set; }
}

