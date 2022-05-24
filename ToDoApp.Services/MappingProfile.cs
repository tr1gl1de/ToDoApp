using AutoMapper;
using ToDoApp.Contracts;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Services;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Note, NoteForReadDto>();
        CreateMap<NoteForCreationDto, Note>();
        CreateMap<NoteForUpdateDto, Note>();

        CreateMap<UserForCreationDto, User>();
        CreateMap<User, UserForReadDto>();
    }
}