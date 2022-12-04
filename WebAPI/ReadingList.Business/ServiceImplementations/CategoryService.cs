using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ReadingList.Core;
using ReadingList.Core.Abstractions;
using ReadingList.Core.DataTransferObjects;
using ReadingList.Data.Abstractions;
using ReadingList.DataBase.Entities;

namespace ReadingList.Business.ServiceImplementations;

public class CategoryService : ICategoryService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IMapper mapper, 
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentException"></exception>
    public async Task<CategoryDto> GetByIdAsync(Guid id)
    {
        var entity = await _unitOfWork.Categories.GetByIdAsync(id);
        if (entity == null)
            throw new ArgumentException("Failed to find record in the database that match the specified id. ", nameof(id));
        var dto = _mapper.Map<CategoryDto>(entity);
        return dto;
    }

    /// <inheritdoc />
    public async Task<List<CategoryDto>> GetCategoriesAsync()
    {
        var categories = await _unitOfWork.Categories
            .Get()
            .ToListAsync();

        return _mapper.Map<List<CategoryDto>>(categories);
    }

    /// <inheritdoc />
    public async Task<bool> IsCategoryExistByNameAsync(string name)
    {
        var entity = await _unitOfWork.Categories
            .Get()
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Name.Equals(name));

        return entity != null;
    }

    /// <inheritdoc />
    public async Task<bool> IsCategoryExistByIdAsync(Guid id)
    {
        var entity = await _unitOfWork.Categories
            .Get()
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id.Equals(id));

        return entity != null;
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentException"></exception>
    public async Task<int> CreateAsync(CategoryDto dto)
    {
        var entity = _mapper.Map<Category>(dto);

        if (entity == null)
            throw new ArgumentException("Mapping CategoryDto to Category was not possible.", nameof(dto));

        await _unitOfWork.Categories.AddAsync(entity);
        var result = await _unitOfWork.Commit();
        return result;
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentException"></exception>
    public async Task<int> UpdateAsync(CategoryDto dto)
    {
        var entity = _mapper.Map<Category>(dto);
        if (entity == null)
            throw new ArgumentException("Mapping CategoryDto to Category was not possible.", nameof(dto));
        _unitOfWork.Categories.Update(entity);
        return await _unitOfWork.Commit();
    }

    /// <inheritdoc />
    public async Task<int> PatchAsync(Guid id, CategoryDto dto)
    {
        var sourceDto = await GetByIdAsync(id);

        var patchList = new List<PatchModel>();

        if (!dto.Name.Equals(sourceDto.Name))
            patchList.Add(new PatchModel
            {
                PropertyName = nameof(dto.Name),
                PropertyValue = dto.Name
            });

        await _unitOfWork.Categories.PatchAsync(id, patchList);
        return await _unitOfWork.Commit();
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentException"></exception>
    public async Task<int> DeleteAsync(Guid id)
    {
        var entity = await _unitOfWork.Categories.GetByIdAsync(id);

        if (entity != null)
        {
            _unitOfWork.Categories.Remove(entity);
            return await _unitOfWork.Commit();
        }

        throw new ArgumentException("The categories for removing doesn't exist", nameof(id));
    }
}