using ReadingList.Core.DataTransferObjects;

namespace ReadingList.Core.Abstractions;

public interface IAuthorService
{
    // READ

    /// <summary>
    /// Get author from storage with specified id.
    /// </summary>
    /// <param name="id">an unique identifier as a <see cref="Guid"/></param>
    /// <returns><see cref="AuthorDto"/></returns>
    Task<AuthorDto> GetByIdAsync(Guid id);

    /// <summary>
    /// Get authors from storage
    /// </summary>
    /// <returns>all authors</returns>
    Task<List<AuthorDto>> GetAuthorsAsync();
    
    /// <summary>
    /// Checks if the record with the same name exists in the storage.
    /// </summary>
    /// <param name="name">name of author as a <see cref="string"/></param>
    /// <returns>A boolean</returns>
    Task<bool> IsAuthorExistByNameAsync(string name);

    /// <summary>
    /// Checks if the record exists in the storage by Id.
    /// </summary>
    /// <param name="id">unique identifier as a <see cref="Guid"/></param>
    /// <returns>A boolean</returns>
    Task<bool> IsAuthorExistByIdAsync(Guid id);

    // CREATE

    /// <summary>
    /// Create a new author record in the storage.
    /// </summary>
    /// <param name="dto"><see cref="AuthorDto"/></param>
    /// <returns>the number of successfully created records in the storage.</returns>
    /// <exception cref="ArgumentException"></exception>
    Task<int> CreateAsync(AuthorDto dto);

    // UPDATE

    /// <summary>
    /// Update author in storage
    /// </summary>
    /// <param name="dto"><see cref="AuthorDto"/></param>
    /// <returns>the number of successfully updated records in the storage.</returns>
    Task<int> UpdateAsync(AuthorDto dto);

    /// <summary>
    /// Patch author with specified id in the storage
    /// </summary>
    /// <param name="id">an unique identifier as a <see cref="Guid"/></param>
    /// <param name="dto"><see cref="AuthorDto"/></param>
    /// <returns>the number of successfully patched records in the storage.</returns>
    Task<int> PatchAsync(Guid id, AuthorDto dto);

    // REMOVE

    /// <summary>
    /// Remove an author with specified id from the storage.
    /// </summary>
    /// <param name="id">an unique identifier as a <see cref="Guid"/></param>
    /// <returns>the number of successfully removed records from the storage.</returns>
    Task<int> DeleteAsync(Guid id);
}