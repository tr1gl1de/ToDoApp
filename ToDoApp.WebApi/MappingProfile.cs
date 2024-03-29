﻿using AutoMapper;
using ToDoApp.Entities.DataTransferObjects;
using ToDoApp.Entities.DataTransferObjects.Note;
using ToDoApp.Entities.DataTransferObjects.User;
using ToDoApp.Entities.Models;

namespace ToDoApp.WebApi;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserForRegisterDto, User>();
        CreateMap<User, UserForReadDto>();

        CreateMap<NoteForCreationDto, Note>();
        CreateMap<Note, NoteForReadDto>();
        CreateMap<NoteForUpdateDto, Note>();

        CreateMap<TokenPair, TokenPairDto>();
    }
}