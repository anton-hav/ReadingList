using ReadingList.Core.DataTransferObjects;
using ReadingList.Core;

namespace ReadingList.WebAPI.Models.Responses;

/// <summary>
/// Book note response model 
/// </summary>
public class GetBookNoteResponseModel
{
    /// <summary>
    /// Book note unique identifier
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Book unique identifier
    /// </summary>
    public Guid BookId { get; set; }

    /// <summary>
    /// Book reading priority
    /// </summary>
    public ReadingPriority Priority { get; set; }

    /// <summary>
    /// Book reading status
    /// </summary>
    public ReadingStatus Status { get; set; }
}