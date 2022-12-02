using ReadingList.Core.DataTransferObjects;

namespace ReadingList.Core.Abstractions;

public interface ICategoryService
{
    // READ

    /// <summary>
    /// Get category from storage with specified id.
    /// </summary>
    /// <param name="id">an unique identifier as a <see cref="Guid"/></param>
    /// <returns><see cref="CategoryDto"/></returns>
    Task<CategoryDto> GetByIdAsync(Guid id);


    /// <summary>
    /// Checks if the record with the same name exists in the storage.
    /// </summary>
    /// <param name="name">name of category as a <see cref="string"/></param>
    /// <returns>A boolean</returns>
    Task<bool> IsCategoryExistByNameAsync(string name);

    /// <summary>
    /// Checks if the record exists in the storage by Id.
    /// </summary>
    /// <param name="id">unique identifier as a <see cref="Guid"/></param>
    /// <returns>A boolean</returns>
    Task<bool> IsCategoryExistByIdAsync(Guid id);

    // CREATE

    /// <summary>
    /// Create a new category record in the storage.
    /// </summary>
    /// <param name="dto"><see cref="CategoryDto"/></param>
    /// <returns>the number of successfully created records in the storage.</returns>
    /// <exception cref="ArgumentException"></exception>
    Task<int> CreateAsync(CategoryDto dto);

    // UPDATE

    /// <summary>
    /// Update category in storage
    /// </summary>
    /// <param name="dto"><see cref="CategoryDto"/></param>
    /// <returns>the number of successfully updated records in the storage.</returns>
    Task<int> UpdateAsync(CategoryDto dto);

    /// <summary>
    /// Patch category with specified id in the storage
    /// </summary>
    /// <param name="id">an unique identifier as a <see cref="Guid"/></param>
    /// <param name="dto"><see cref="CategoryDto"/></param>
    /// <returns>the number of successfully patched records in the storage.</returns>
    Task<int> PatchAsync(Guid id, CategoryDto dto);

    // REMOVE

    /// <summary>
    /// Remove a category with specified id from the storage.
    /// </summary>
    /// <param name="id">an unique identifier as a <see cref="Guid"/></param>
    /// <returns>the number of successfully removed records from the storage.</returns>
    Task<int> DeleteAsync(Guid id);
}