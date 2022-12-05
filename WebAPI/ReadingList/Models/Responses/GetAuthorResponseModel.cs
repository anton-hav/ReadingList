namespace ReadingList.WebAPI.Models.Responses;

/// <summary>
/// Author response model 
/// </summary>
public class GetAuthorResponseModel
{
    /// <summary>
    /// Author unique identifier
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Author name
    /// </summary>
    public string FullName { get; set; }
}