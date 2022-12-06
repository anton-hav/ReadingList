using AutoMapper;
using ReadingList.Core.DataTransferObjects;
using ReadingList.DataBase.Entities;
using ReadingList.WebAPI.Models.Requests;
using ReadingList.WebAPI.Models.Responses;

namespace ReadingList.WebAPI.MappingProfiles;

public class BookNoteProfile : Profile
{
    public BookNoteProfile()
    {
        CreateMap<BookNote, BookNoteDto>();
        CreateMap<BookNoteDto, BookNote>();

        CreateMap<AddOrUpdateBookNoteRequestModel, BookNoteDto>();

        CreateMap<BookNoteDto, GetBookNoteResponseModel>();
    }
}