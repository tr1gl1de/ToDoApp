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
public class NoteController : BaseController
{
    private IRepositoryWrapper _repository;
    private IMapper _mapper;

    public NoteController(IRepositoryWrapper repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>Create a new note.</summary>
    /// <param name="noteCreate">Object Note.</param>
    /// <response code="201">Created note.</response>
    /// <response code="404">User not found.</response> 
    [HttpPost]
    [ProducesResponseType(typeof(NoteForReadDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateNote([FromBody] NoteForCreationDto noteCreate)
    {
        var userId = GetAuthUserId();
        
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

    /// <summary>Get note with id.</summary>
    /// <param name="id">Identifier of note.</param>
    /// <response code="200">Note received.</response>
    /// <response code="404">Note not found.</response>
    /// <response code="409">Note does not belong to user.</response> 
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(NoteForReadDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> GetNoteById([FromRoute] Guid id)
    {
        var userId = GetAuthUserId();
        
        var note = await _repository.Note.GetNoteById(id);
        if (note is null)
        {
            return NotFound("Not found note with this id");
        }

        if (note.UserId != userId)
        {
            return Conflict("This note does not belong to user");
        }
        
        var readNote = _mapper.Map<NoteForReadDto>(note);
        return Ok(readNote);
    }

    /// <summary>Get all user notes.</summary>
    /// <response code="200">Notes received.</response>
    /// <response code="404">User not found.</response> 
    [HttpGet("all")]
    [ProducesResponseType(typeof(NoteForReadDto[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserNotes()
    {
        var userId = GetAuthUserId();
        
        var userExist = await _repository.User.UserIsExits(userId);
        if (!userExist)
        {
            return NotFound("Not found user with this id");
        }
        
        var userNotes = await _repository.Note.GetNotesByUserId(userId);
        var userNotesRead = _mapper.Map<ICollection<NoteForReadDto>>(userNotes);
        
        return Ok(userNotesRead);
    }

    /// <summary>Update note with id.</summary>
    /// <param name="noteId">Identifier of note.</param>
    /// <param name="noteForUpdateDto">Object note.</param>
    /// <response code="204">Updated note.</response>
    /// <response code="404">Note not found.</response>
    /// <response code="409">Note does not belong to user.</response> 
    [HttpPut("{noteId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateNote([FromRoute]Guid noteId, [FromBody] NoteForUpdateDto noteForUpdateDto)
    {
        var userId = GetAuthUserId();
        
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

    /// <summary>Delete note with id.</summary>
    /// <param name="noteId">Identifier of note.</param>
    /// <response code="204">Deleted note.</response>
    /// <response code="404">Note not found.</response>
    /// <response code="409">Note does not belong to user.</response>
    [HttpDelete("{noteId:guid}")]
    public async Task<IActionResult> DeleteNote([FromRoute] Guid noteId)
    {
        var userId = GetAuthUserId();
        
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