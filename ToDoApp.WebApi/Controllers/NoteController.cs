using System.Net.Mime;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Contracts;
using ToDoApp.Entities.DataTransferObjects.Note;
using ToDoApp.Entities.Models;

namespace ToDoApp.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class NoteController : ControllerBase
{
    private IRepositoryWrapper _repository;
    private IMapper _mapper;

    public NoteController(IRepositoryWrapper repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpPost("{userId:guid}")]
    public async Task<IActionResult> CreateNote([FromRoute] Guid userId,[FromBody] NoteForCreationDto noteCreate)
    {
        var userExist = await _repository.User.UserIsExits(userId);
        if (!userExist)
        {
            return NotFound("Not found user with this id");
        }
        
        var newNote = _mapper.Map<Note>(noteCreate);
        newNote.DateCreation = DateTime.UtcNow;
        newNote.UserId = userId;
        
        _repository.Note.CreateNote(newNote);
        await _repository.SaveAsync();

        return CreatedAtAction(nameof(GetNoteById), 
            new {id = newNote.Id},
            newNote);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetNoteById([FromRoute] Guid id)
    {
        var note = await _repository.Note.GetNoteById(id);
        if (note is null)
        {
            return NotFound("Not found note with this id");
        }
        
        var readNote = _mapper.Map<NoteForReadDto>(note);
        return Ok(readNote);
    }

    [HttpGet("all/user/{userId:guid}")]
    public async Task<IActionResult> GetUserNotes([FromRoute] Guid userId)
    {
        var userExist = await _repository.User.UserIsExits(userId);
        if (!userExist)
        {
            return NotFound("Not found user with this id");
        }
        
        var userNotes = await _repository.Note.GetNotesByUserId(userId);
        var userNotesRead = _mapper.Map<ICollection<NoteForReadDto>>(userNotes);
        
        return Ok(userNotesRead);
    }
}