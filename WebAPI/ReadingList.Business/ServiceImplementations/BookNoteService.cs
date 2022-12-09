using AutoMapper;
using AutoMapper.QueryableExtensions.Impl;
using Microsoft.EntityFrameworkCore;
using ReadingList.Core;
using ReadingList.Core.Abstractions;
using ReadingList.Core.DataTransferObjects;
using ReadingList.Data.Abstractions;
using ReadingList.DataBase.Entities;

namespace ReadingList.Business.ServiceImplementations;

public class BookNoteService : IBookNoteService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public BookNoteService(IMapper mapper, 
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentException"></exception>
    public async Task<BookNoteDto> GetByIdAsync(Guid id)
    {
        var entity = await _unitOfWork.BookNotes.GetByIdAsync(id);
        if (entity == null)
            throw new ArgumentException("Failed to find record in the database that match the specified id. ", nameof(id));
        var dto = _mapper.Map<BookNoteDto>(entity);
        return dto;
    }

    /// <inheritdoc />
    public async Task<List<BookNoteDto>> GetBookNotesAsync()
    {
        var books = await _unitOfWork.BookNotes
            .Get()
            .ToListAsync();

        return _mapper.Map<List<BookNoteDto>>(books);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<BookNoteDto>> GetBookNotesBySearchParametersAsync(Guid? bookId, 
        ReadingPriority? priority, 
        ReadingStatus? status, 
        int pageNumber, 
        int pageSize,
        OrderParameter? orderParameter,
        bool isOrderDescending)
    {
        var entities = _unitOfWork.BookNotes.Get();

        if (bookId != null && !Guid.Empty.Equals(bookId))
            entities = entities.Where(entity => entity.BookId.Equals(bookId));

        if (priority != null)
            entities = entities.Where(entity => entity.Priority.Equals(priority));

        if (status != null)
            entities = entities.Where(entity => entity.Status.Equals(status));

        // Ordered query by orderParameter and isOrderDescending flag
        if (orderParameter != null
            && Enum.IsDefined(typeof(OrderParameter), orderParameter)
            && !OrderParameter.None.Equals(orderParameter))
        {
            entities = OrderByParameters(entities, (OrderParameter)orderParameter, isOrderDescending);
        }

        var result = (await entities
                .Skip(pageSize * pageNumber)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync())
            .Select(entity => _mapper.Map<BookNoteDto>(entity))
            .ToArray();
        return result;
    }

    /// <inheritdoc />
    public async Task<int> GetBookNotesCountBySearchParametersAsync(Guid? authorId, Guid? categoryId, ReadingPriority? priority,
        ReadingStatus? status)
    {
        var entities = _unitOfWork.BookNotes.Get();

        if (authorId != null && !Guid.Empty.Equals(authorId))
            entities = entities.Where(entity => entity.Book.AuthorId.Equals(authorId));

        if (categoryId != null && !Guid.Empty.Equals(categoryId))
            entities = entities.Where(entity => entity.Book.CategoryId.Equals(categoryId));

        if (priority != null && Enum.IsDefined(typeof(ReadingPriority), priority))
            entities = entities.Where(entity => entity.Priority.Equals(priority));

        if (status != null && Enum.IsDefined(typeof(ReadingStatus), status))
            entities = entities.Where(entity => entity.Status.Equals(status));

        var result = await entities.AsNoTracking().CountAsync();
        return result;
    }

    /// <inheritdoc />
    public async Task<bool> IsBookNoteExistByBookIdAsync(Guid bookId)
    {
        var entity = await _unitOfWork.BookNotes
            .Get()
            .AsNoTracking()
            .FirstOrDefaultAsync(entity =>
                entity.BookId.Equals(bookId));

        return entity != null;
    }

    /// <inheritdoc />
    public async Task<bool> IsBookNoteExistAsync(Guid id)
    {
        var entity = await _unitOfWork.BookNotes
            .Get()
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id.Equals(id));

        return entity != null;
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentException"></exception>
    public async Task<int> CreateAsync(BookNoteDto dto)
    {
        var entity = _mapper.Map<BookNote>(dto);

        if (entity == null)
            throw new ArgumentException("Mapping BookNoteDto to BookNote was not possible.", nameof(dto));

        await _unitOfWork.BookNotes.AddAsync(entity);
        var result = await _unitOfWork.Commit();
        return result;
    }

    /// <inheritdoc />
    /// <exception cref="NotImplementedException"></exception>
    public async Task<int> UpdateAsync(BookNoteDto dto)
    {
        var entity = _mapper.Map<BookNote>(dto);
        if (entity == null)
            throw new ArgumentException("Mapping BookNoteDto to BookNote was not possible.", nameof(dto));
        _unitOfWork.BookNotes.Update(entity);
        return await _unitOfWork.Commit();
    }

    /// <inheritdoc />
    public async Task<int> PatchAsync(Guid id, BookNoteDto dto)
    {
        var sourceDto = await GetByIdAsync(id);

        var patchList = new List<PatchModel>();

        if (!dto.BookId.Equals(sourceDto.BookId))
            patchList.Add(new PatchModel
            {
                PropertyName = nameof(dto.BookId),
                PropertyValue = dto.BookId
            });

        if (!dto.Priority.Equals(sourceDto.Priority)
            && Enum.IsDefined(typeof(ReadingPriority), dto.Priority))
            patchList.Add(new PatchModel
            {
                PropertyName = nameof(dto.Priority),
                PropertyValue = dto.Priority
            });

        if (!dto.Status.Equals(sourceDto.Status)
            && Enum.IsDefined(typeof(ReadingStatus), dto.Status))
            patchList.Add(new PatchModel
            {
                PropertyName = nameof(dto.Status),
                PropertyValue = dto.Status
            });

        await _unitOfWork.BookNotes.PatchAsync(id, patchList);
        return await _unitOfWork.Commit();
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentException"></exception>
    public async Task<int> DeleteAsync(Guid id)
    {
        var entity = await _unitOfWork.BookNotes.GetByIdAsync(id);
        
        if (entity != null)
        {
            _unitOfWork.BookNotes.Remove(entity);
            return await _unitOfWork.Commit();
        }

        throw new ArgumentException("The book notes for removing doesn't exist", nameof(id));
    }

    /// <summary>
    /// Orders query by parameters
    /// </summary>
    /// <param name="query"></param>
    /// <param name="orderParameter">Order parameter as a <see cref="OrderParameter"/></param>
    /// <param name="isOrderDescending">Flag indicating whether the sorting should be in descending order as a <see cref="bool"/></param>
    /// <returns>ordered query as a <see cref="IQueryable{T}"/> where T is <see cref="BookNote"/></returns>
    private IQueryable<BookNote> OrderByParameters(IQueryable<BookNote> query,
        OrderParameter orderParameter,
        bool isOrderDescending)
    {
        if (OrderParameter.Author.Equals(orderParameter))
            query = isOrderDescending
                ? query.OrderByDescending(entity => entity.Book.Author)
                : query.OrderBy(entity => entity.Book.Author);
        else if (OrderParameter.Title.Equals(orderParameter))
            query = isOrderDescending
                ? query.OrderByDescending(entity => entity.Book.Title.ToUpper())
                : query.OrderBy(entity => entity.Book.Title.ToUpper());
        else if (OrderParameter.Category.Equals(orderParameter))
            query = isOrderDescending
                ? query.OrderByDescending(entity => entity.Book.Categories)
                : query.OrderBy(entity => entity.Book.Categories);
        else if (OrderParameter.Priority.Equals(orderParameter))
            query = isOrderDescending
                ? query.OrderByDescending(entity => entity.Priority)
                : query.OrderBy(entity => entity.Priority);
        else if (OrderParameter.Status.Equals(orderParameter))
            query = isOrderDescending
                ? query.OrderByDescending(entity => entity.Status)
                : query.OrderBy(entity => entity.Status);
        
        return query;
    }
}