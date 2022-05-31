using System.Net.Mime;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Contracts;
using ToDoApp.Entities.DataTransferObjects.Note;
using ToDoApp.Entities.Models;

namespace ToDoApp.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
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

    [HttpPost]
    public async Task<IActionResult> CreateNote([FromBody] NoteForCreationDto noteCreate)
    {
        var subClaim = User.Claims.Single(claim => claim.Type == "sub");
        var userId = Guid.Parse(subClaim.Value);
        
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
        var subClaim = User.Claims.Single(claim => claim.Type == "sub");
        var userId = Guid.Parse(subClaim.Value);
        
        var note = await _repository.Note.GetNoteById(id);
        if (note is null)
        {
            return NotFound("Not found note with this id");
        }

        if (note.UserId != userId)
        {
            return Conflict("This note does not belong to you");
        }
        
        var readNote = _mapper.Map<NoteForReadDto>(note);
        return Ok(readNote);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetUserNotes()
    {
        var subClaim = User.Claims.Single(claim => claim.Type == "sub");
        var userId = Guid.Parse(subClaim.Value);
        
        var userExist = await _repository.User.UserIsExits(userId);
        if (!userExist)
        {
            return NotFound("Not found user with this id");
        }
        
        var userNotes = await _repository.Note.GetNotesByUserId(userId);
        var userNotesRead = _mapper.Map<ICollection<NoteForReadDto>>(userNotes);
        
        return Ok(userNotesRead);
    }

    [HttpPut("{noteId:guid}")]
    public async Task<IActionResult> UpdateNote([FromRoute]Guid noteId, [FromBody] NoteForUpdateDto noteForUpdateDto)
    {
        var subClaim = User.Claims.Single(claim => claim.Type == "sub");
        var userId = Guid.Parse(subClaim.Value);
        
        var note = await _repository.Note.GetNoteById(noteId);
        if (note is null)
        {
            return NotFound("Not found note with this id");
        }
        
        if (note.UserId != userId)
        {
            return Conflict("This note does not belong to you");
        }
        note.DateUpdate = DateTime.UtcNow;
        _mapper.Map(noteForUpdateDto, note);
        _repository.Note.UpdateNote(note);
        
        await _repository.SaveAsync();
        
        return NoContent();
    }

    [HttpDelete("{noteId:guid}")]
    public async Task<IActionResult> DeleteNote([FromRoute] Guid noteId)
    {
        var subClaim = User.Claims.Single(claim => claim.Type == "sub");
        var userId = Guid.Parse(subClaim.Value);
        
        var note = await _repository.Note.GetNoteById(noteId);
        if (note is null)
        {
            return NotFound("Not found note with this id");
        }
        
        if (note.UserId != userId)
        {
            return Conflict("This note does not belong to you");
        }
        
        _repository.Note.DeleteNote(note);
        await _repository.SaveAsync();

        var readDto = _mapper.Map<NoteForReadDto>(note);

        return Ok(readDto);
    }
}