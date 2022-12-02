using AutoMapper;
using ReadingList.Core.DataTransferObjects;
using ReadingList.DataBase.Entities;
using ReadingList.WebAPI.Models.Requests;

namespace ReadingList.WebAPI.MappingProfiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryDto>();
        CreateMap<CategoryDto, Category>();

        CreateMap<AddOrUpdateCategoryRequestModel, CategoryDto>();
    }
}