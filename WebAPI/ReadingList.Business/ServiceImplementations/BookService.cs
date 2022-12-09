using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ReadingList.Core;
using ReadingList.Core.Abstractions;
using ReadingList.Core.DataTransferObjects;
using ReadingList.Data.Abstractions;
using ReadingList.DataBase.Entities;

namespace ReadingList.Business.ServiceImplementations;

public class BookService : IBookService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;


    public BookService(IMapper mapper, 
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentException"></exception>
    public async Task<BookDto> GetByIdAsync(Guid id)
    {
        var entity = await _unitOfWork.Books.GetByIdAsync(id);
        if (entity == null)
            throw new ArgumentException("Failed to find record in the database that match the specified id. ", nameof(id));
        var dto = _mapper.Map<BookDto>(entity);
        return dto;
    }

    /// <inheritdoc />
    public async Task<List<BookDto>> GetBooksAsync()
    {
        var books = await _unitOfWork.Books
            .Get()
            .ToListAsync();

        return _mapper.Map<List<BookDto>>(books);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<BookDto>> GetBooksBySearchParametersAsync(string? title, 
        Guid? authorId, 
        Guid? categoryId,
        Guid? bookNoteId,
        int pageNumber, 
        int pageSize)
    {
        var entities = _unitOfWork.Books.Get();

        if (!string.IsNullOrEmpty(title))
            entities = entities.Where(entity => entity.Title.Contains(title));

        if (authorId != null && !Guid.Empty.Equals(authorId))
            entities = entities.Where(entity => entity.AuthorId.Equals(authorId));

        if (categoryId != null && !Guid.Empty.Equals(categoryId))
            entities = entities.Where(entity => entity.CategoryId.Equals(categoryId));

        if (bookNoteId != null && !Guid.Empty.Equals(bookNoteId))
            entities = entities.Where(entity => entity.BookNote.Id.Equals(bookNoteId));

        var result = (await entities
                .Skip(pageSize * pageNumber)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync())
            .Select(entity => _mapper.Map<BookDto>(entity))
            .ToArray();
        return result;
    }

    /// <inheritdoc />
    public async Task<bool> IsBookExistAsync(string title, Guid authorId)
    {
        var entity = await _unitOfWork.Books
            .Get()
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => 
                entity.AuthorId.Equals(authorId)
                && entity.Title.Equals(title));

        return entity != null;
    }

    /// <inheritdoc />
    public async Task<bool> IsBookExistAsync(Guid id)
    {
        var entity = await _unitOfWork.Books
            .Get()
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id.Equals(id));

        return entity != null;
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentException"></exception>
    public async Task<int> CreateAsync(BookDto dto)
    {
        var entity = _mapper.Map<Book>(dto);

        if (entity == null)
            throw new ArgumentException("Mapping BookDto to Book was not possible.", nameof(dto));

        await _unitOfWork.Books.AddAsync(entity);
        var result = await _unitOfWork.Commit();
        return result;
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentException"></exception>
    public async Task<int> UpdateAsync(BookDto dto)
    {
        var entity = _mapper.Map<Book>(dto);
        if (entity == null)
            throw new ArgumentException("Mapping BookDto to Book was not possible.", nameof(dto));
        _unitOfWork.Books.Update(entity);
        return await _unitOfWork.Commit();
    }

    /// <inheritdoc />
    public async Task<int> PatchAsync(Guid id, BookDto dto)
    {
        var sourceDto = await GetByIdAsync(id);

        var patchList = new List<PatchModel>();

        if (!dto.Title.Equals(sourceDto.Title))
            patchList.Add(new PatchModel
            {
                PropertyName = nameof(dto.Title),
                PropertyValue = dto.Title
            });

        if (!dto.AuthorId.Equals(sourceDto.AuthorId))
            patchList.Add(new PatchModel
            {
                PropertyName = nameof(dto.AuthorId),
                PropertyValue = dto.AuthorId
            });

        if (!dto.CategoryId.Equals(sourceDto.CategoryId))
            patchList.Add(new PatchModel
            {
                PropertyName = nameof(dto.CategoryId),
                PropertyValue = dto.CategoryId
            });

        await _unitOfWork.Books.PatchAsync(id, patchList);
        return await _unitOfWork.Commit();
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentException"></exception>
    public async Task<int> DeleteAsync(Guid id)
    {
        var entity = await _unitOfWork.Books.GetByIdAsync(id);

        if (entity != null)
        {
            _unitOfWork.Books.Remove(entity);
            return await _unitOfWork.Commit();
        }

        throw new ArgumentException("The books for removing doesn't exist", nameof(id));
    }
}