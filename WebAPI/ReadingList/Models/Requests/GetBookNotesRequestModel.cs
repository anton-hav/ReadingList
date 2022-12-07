using ReadingList.Core;

namespace ReadingList.WebAPI.Models.Requests;

/// <summary>
/// Request model to get book notes
/// </summary>
public class GetBookNotesRequestModel
{
    /// <summary>
    /// Book unique identifier
    /// </summary>
    public Guid? BookId { get; set; }

    /// <summary>
    /// Book reading priority from Never to Right Now
    /// </summary>
    public ReadingPriority? Priority { get; set; }

    /// <summary>
    /// Book reading status
    /// (Scheduled = 0, InProgress = 1, Completed = 2, Skipped = 3)
    /// </summary>
    public ReadingStatus? Status { get; set; }
    
    /// <summary>
    /// Search parameter representing the number of books on the page
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Search parameter representing the page number
    /// </summary>
    public int PageNumber { get; set; }
}