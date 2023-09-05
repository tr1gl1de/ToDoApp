using ToDoApp.Entities.DataTransferObjects.QueryStringParameters;
using ToDoApp.Entities.Helpers;
using ToDoApp.Entities.Models;

namespace ToDoApp.Contracts;

public interface INoteRepository
{
    Task<Note?> GetNoteById(Guid noteId);
    Task<IEnumerable<Note>> GetAllNotes();
    Task<IEnumerable<Note>> GetNotesByUserId(Guid userId);
    Task<PagedList<Note>> GetNotesByUserId(Guid userId, NoteQueryStringParameters notesParam);
    Task<IEnumerable<Note>> SearchNotesByNameAsync(Guid userId ,string name);
    Task<PagedList<Note>> SearchNotesByNameAsync(Guid userId, NoteQueryStringParametersForSearch notesParam);
    void CreateNote(Note note);
    void UpdateNote(Note note);
    void DeleteNote(Note note);
}