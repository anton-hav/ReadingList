using ReadingList.Core.DataTransferObjects;

namespace ReadingList.Core.Abstractions;

public interface IBookNoteService
{
    // READ

    /// <summary>
    /// Get book note from storage with specified id.
    /// </summary>
    /// <param name="id">an unique identifier as a <see cref="Guid"/></param>
    /// <returns><see cref="BookNoteDto"/></returns>
    Task<BookNoteDto> GetByIdAsync(Guid id);

    /// <summary>
    /// Get book notes from storage
    /// </summary>
    /// <returns>all book notes</returns>
    Task<List<BookNoteDto>> GetBookNotesAsync();

    /// <summary>
    /// Get book notes from storage by search parameters
    /// </summary>
    /// <param name="bookId">a book unique identifier as a <see cref="Guid"/></param>
    /// <param name="priority">a reading priority as a <see cref="ReadingPriority"/></param>
    /// <param name="status">a reading status as a <see cref="ReadingStatus"/></param>
    /// <param name="pageNumber">Search parameter representing the page number</param>
    /// <param name="pageSize">Search parameter representing the number of book notes on the page</param>
    /// <returns>book notes matching the search results</returns>
    Task<IEnumerable<BookNoteDto>> GetBookNotesBySearchParametersAsync(Guid? bookId,
        ReadingPriority? priority,
        ReadingStatus? status,
        int pageNumber,
        int pageSize);

    /// <summary>
    /// Get book notes count from storage by search parameters
    /// </summary>
    /// <param name="authorId">an author unique identifier as a <see cref="Guid"/></param>
    /// <param name="categoryId">a category unique identifier as a <see cref="Guid"/></param>
    /// <param name="priority">a book reading priority as a <see cref="ReadingPriority"/></param>
    /// <param name="status">a book reading status as a <see cref="ReadingStatus"/></param>
    /// <returns></returns>
    Task<int> GetBookNotesCountBySearchParametersAsync(Guid? authorId, 
        Guid? categoryId, 
        ReadingPriority? priority,
        ReadingStatus? status);

    /// <summary>
    /// Checks if the record with the same bookId exists in the storage.
    /// </summary>
    /// <param name="bookId">book unique identifier as a <see cref="Guid"/></param>
    /// <returns>A boolean</returns>
    Task<bool> IsBookNoteExistByBookIdAsync(Guid bookId);

    /// <summary>
    /// Checks if the record exists in the storage by Id.
    /// </summary>
    /// <param name="id">unique identifier as a <see cref="Guid"/></param>
    /// <returns>A boolean</returns>
    Task<bool> IsBookNoteExistAsync(Guid id);

    // CREATE

    /// <summary>
    /// Create a new book note record in the storage.
    /// </summary>
    /// <param name="dto"><see cref="BookNoteDto"/></param>
    /// <returns>the number of successfully created records in the storage.</returns>
    /// <exception cref="ArgumentException"></exception>
    Task<int> CreateAsync(BookNoteDto dto);

    // UPDATE

    /// <summary>
    /// Update book note in storage
    /// </summary>
    /// <param name="dto"><see cref="BookNoteDto"/></param>
    /// <returns>the number of successfully updated records in the storage.</returns>
    Task<int> UpdateAsync(BookNoteDto dto);

    /// <summary>
    /// Patch book note with specified id in the storage
    /// </summary>
    /// <param name="id">an unique identifier as a <see cref="Guid"/></param>
    /// <param name="dto"><see cref="BookNoteDto"/></param>
    /// <returns>the number of successfully patched records in the storage.</returns>
    Task<int> PatchAsync(Guid id, BookNoteDto dto);

    // REMOVE

    /// <summary>
    /// Remove a book note with specified id from the storage.
    /// </summary>
    /// <param name="id">an unique identifier as a <see cref="Guid"/></param>
    /// <returns>the number of successfully removed records from the storage.</returns>
    Task<int> DeleteAsync(Guid id);
}