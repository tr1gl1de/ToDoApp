using Microsoft.AspNetCore.Mvc;
using ToDoApp.Contracts;
using ToDoApp.Services.Abstraction;

namespace Presentation.Controllers;

[ApiController]
[Route("api/user/{userId:guid}/notes")]
public class NoteController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public NoteController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetNotes(Guid userId, CancellationToken cancellationToken)
    {
        var notesDto = await _serviceManager.NoteService
            .GetAllByUserIdAsync(userId, cancellationToken);
        return Ok(notesDto);
    }

    [HttpGet("{noteId:guid}")]
    public async Task<IActionResult> GetNoteById(Guid noteId, Guid userId, CancellationToken cancellationToken)
    {
        var note = await _serviceManager.NoteService.GetByIdAsync(noteId, userId, cancellationToken);
        return Ok(note);
    }

    [HttpPost]
    public async Task<IActionResult> CreateNote(Guid userId,
        [FromBody] NoteForCreationDto noteForCreationDto,
        CancellationToken cancellationToken)
    {
        var newNote = await _serviceManager.NoteService
            .CreateNoteAsync(userId ,noteForCreationDto, cancellationToken);
        
        return CreatedAtAction(nameof(GetNoteById),
            new {userId = newNote.UserId, noteId = newNote.Id}, newNote);
    }

    [HttpPut("{noteId:guid}")]
    public async Task<IActionResult> UpdateNote(Guid userId, Guid noteId,
        [FromBody] NoteForUpdateDto noteDto,
        CancellationToken cancellationToken)
    {
        await _serviceManager.NoteService.UpdateNoteAsync(userId, noteId, noteDto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{noteId:guid}")]
    public async Task<IActionResult> DeleteNote(Guid userId, Guid noteId, CancellationToken cancellationToken)
    {
        await _serviceManager.NoteService.DeleteNoteAsync(userId, noteId, cancellationToken);
        return NoContent();
    }
}