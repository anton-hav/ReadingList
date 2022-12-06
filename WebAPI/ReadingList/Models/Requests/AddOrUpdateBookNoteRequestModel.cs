using ReadingList.Core;
using ReadingList.DataBase.Entities;

namespace ReadingList.WebAPI.Models.Requests;

/// <summary>
/// Request model to create or update book note
/// </summary>
public class AddOrUpdateBookNoteRequestModel
{
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