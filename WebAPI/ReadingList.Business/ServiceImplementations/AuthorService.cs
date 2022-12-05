using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ReadingList.Core;
using ReadingList.Core.Abstractions;
using ReadingList.Core.DataTransferObjects;
using ReadingList.Data.Abstractions;
using ReadingList.DataBase.Entities;

namespace ReadingList.Business.ServiceImplementations;

public class AuthorService : IAuthorService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public AuthorService(IMapper mapper, 
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentException"></exception>
    public async Task<AuthorDto> GetByIdAsync(Guid id)
    {
        var entity = await _unitOfWork.Authors.GetByIdAsync(id);
        if (entity == null)
            throw new ArgumentException("Failed to find record in the database that match the specified id. ", nameof(id));
        var dto = _mapper.Map<AuthorDto>(entity);
        return dto;
    }

    /// <inheritdoc />
    public async Task<List<AuthorDto>> GetAuthorsAsync()
    {
        var authors = await _unitOfWork.Authors
            .Get()
            .ToListAsync();

        return _mapper.Map<List<AuthorDto>>(authors);
    }

    /// <inheritdoc />
    public async Task<bool> IsAuthorExistByNameAsync(string fullName)
    {
        var entity = await _unitOfWork.Authors
            .Get()
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.FullName.Equals(fullName));

        return entity != null;
    }

    /// <inheritdoc />
    public async Task<bool> IsAuthorExistByIdAsync(Guid id)
    {
        var entity = await _unitOfWork.Authors
            .Get()
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id.Equals(id));

        return entity != null;
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentException"></exception>
    public async Task<int> CreateAsync(AuthorDto dto)
    {
        var entity = _mapper.Map<Author>(dto);

        if (entity == null)
            throw new ArgumentException("Mapping AuthorDto to Author was not possible.", nameof(dto));

        await _unitOfWork.Authors.AddAsync(entity);
        var result = await _unitOfWork.Commit();
        return result;
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentException"></exception>
    public async Task<int> UpdateAsync(AuthorDto dto)
    {
        var entity = _mapper.Map<Author>(dto);
        if (entity == null)
            throw new ArgumentException("Mapping AuthorDto to Author was not possible.", nameof(dto));
        _unitOfWork.Authors.Update(entity);
        return await _unitOfWork.Commit();
    }

    /// <inheritdoc />
    public async Task<int> PatchAsync(Guid id, AuthorDto dto)
    {
        var sourceDto = await GetByIdAsync(id);

        var patchList = new List<PatchModel>();

        if (!dto.FullName.Equals(sourceDto.FullName))
            patchList.Add(new PatchModel
            {
                PropertyName = nameof(dto.FullName),
                PropertyValue = dto.FullName
            });

        await _unitOfWork.Authors.PatchAsync(id, patchList);
        return await _unitOfWork.Commit();
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentException"></exception>
    public async Task<int> DeleteAsync(Guid id)
    {
        var entity = await _unitOfWork.Authors.GetByIdAsync(id);

        if (entity != null)
        {
            _unitOfWork.Authors.Remove(entity);
            return await _unitOfWork.Commit();
        }

        throw new ArgumentException("The authors for removing doesn't exist", nameof(id));
    }
}