using ReadingList.Core.DataTransferObjects;

namespace ReadingList.Core.Abstractions;

public interface IBookService
{
    // READ

    /// <summary>
    /// Get book from storage with specified id.
    /// </summary>
    /// <param name="id">an unique identifier as a <see cref="Guid"/></param>
    /// <returns><see cref="BookDto"/></returns>
    Task<BookDto> GetByIdAsync(Guid id);

    /// <summary>
    /// Get books from storage
    /// </summary>
    /// <returns>all books</returns>
    Task<List<BookDto>> GetBooksAsync();

    /// <summary>
    /// Get books from storage by search parameters
    /// </summary>
    /// <param name="title">Search parameter representing book title</param>
    /// <param name="authorId">Search parameter representing the unique identifier of book author</param>
    /// <param name="categoryId">Search parameter representing the unique identifier of the book category </param>
    /// <param name="bookNoteId">Search parameter representing the book unique identifier </param>
    /// <param name="pageNumber">Search parameter representing the page number</param>
    /// <param name="pageSize">Search parameter representing the number of books on the page</param>
    /// <returns>books matching the search results</returns>
    Task<IEnumerable<BookDto>> GetBooksBySearchParametersAsync(string? title, 
        Guid? authorId, 
        Guid? categoryId,
        Guid? bookNoteId,
        int pageNumber, 
        int pageSize);

    /// <summary>
    /// Checks if the record with the same title and authorId exists in the storage.
    /// </summary>
    /// <param name="title">title of book as a <see cref="string"/></param>
    /// <param name="authorId">author unique identifier as a <see cref="Guid"/></param>
    /// <returns>A boolean</returns>
    Task<bool> IsBookExistAsync(string title, Guid authorId);

    /// <summary>
    /// Checks if the record exists in the storage by Id.
    /// </summary>
    /// <param name="id">unique identifier as a <see cref="Guid"/></param>
    /// <returns>A boolean</returns>
    Task<bool> IsBookExistAsync(Guid id);

    // CREATE

    /// <summary>
    /// Create a new book record in the storage.
    /// </summary>
    /// <param name="dto"><see cref="BookDto"/></param>
    /// <returns>the number of successfully created records in the storage.</returns>
    /// <exception cref="ArgumentException"></exception>
    Task<int> CreateAsync(BookDto dto);

    // UPDATE

    /// <summary>
    /// Update book in storage
    /// </summary>
    /// <param name="dto"><see cref="BookDto"/></param>
    /// <returns>the number of successfully updated records in the storage.</returns>
    Task<int> UpdateAsync(BookDto dto);

    /// <summary>
    /// Patch book with specified id in the storage
    /// </summary>
    /// <param name="id">an unique identifier as a <see cref="Guid"/></param>
    /// <param name="dto"><see cref="BookDto"/></param>
    /// <returns>the number of successfully patched records in the storage.</returns>
    Task<int> PatchAsync(Guid id, BookDto dto);

    // REMOVE

    /// <summary>
    /// Remove a book with specified id from the storage.
    /// </summary>
    /// <param name="id">an unique identifier as a <see cref="Guid"/></param>
    /// <returns>the number of successfully removed records from the storage.</returns>
    Task<int> DeleteAsync(Guid id);
}