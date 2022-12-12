using AutoMapper;
using ReadingList.Core.DataTransferObjects;
using ReadingList.DataBase.Entities;
using ReadingList.WebAPI.Models.Requests;
using ReadingList.WebAPI.Models.Responses;

namespace ReadingList.WebAPI.MappingProfiles;

public class AuthorProfile : Profile
{
    public AuthorProfile()
    {
        CreateMap<Author, AuthorDto>();
        CreateMap<AuthorDto, Author>();

        CreateMap<AddOrUpdateAuthorRequestModel, AuthorDto>();

        CreateMap<AuthorDto, GetAuthorResponseModel>();
    }
}