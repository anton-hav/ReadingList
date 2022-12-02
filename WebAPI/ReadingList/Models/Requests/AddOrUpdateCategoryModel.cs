namespace ReadingList.WebAPI.Models.Requests;

/// <summary>
/// Request model to create or update category
/// </summary>
public class AddOrUpdateCategoryRequestModel
{
    /// <summary>
    /// Category name
    /// </summary>
    public string Name { get; set; }
}