using ToDoApp.Entities.Models;

namespace ToDoApp.Contracts;

public interface INoteRepository
{
    Task<Note?> GetNoteById(Guid noteId);
    Task<IEnumerable<Note>> GetAllNotes();
    Task<IEnumerable<Note>> GetNotesByUserId(Guid userId);
    void CreateNote(Note note);
    void UpdateNote(Note note);
    void DeleteNote(Note note);
}