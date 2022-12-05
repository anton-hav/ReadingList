using AutoMapper;
using ReadingList.Core.DataTransferObjects;
using ReadingList.DataBase.Entities;
using ReadingList.WebAPI.Models.Requests;
using ReadingList.WebAPI.Models.Responses;

namespace ReadingList.WebAPI.MappingProfiles;

public class BookProfile : Profile
{
    public BookProfile()
    {
        CreateMap<Book, BookDto>();
        CreateMap<BookDto, Book>();

        CreateMap<AddOrUpdateBookRequestModel, BookDto>();

        CreateMap<BookDto, GetBookResponseModel>();
    }
}