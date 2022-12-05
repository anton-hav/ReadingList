namespace ReadingList.WebAPI.Models.Requests;

/// <summary>
/// Request model to create or update author
/// </summary>
public class AddOrUpdateAuthorRequestModel
{
    /// <summary>
    /// Author name
    /// </summary>
    public string FullName { get; set; }
}