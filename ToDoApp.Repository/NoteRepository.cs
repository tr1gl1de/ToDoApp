using Microsoft.EntityFrameworkCore;
using ToDoApp.Contracts;
using ToDoApp.Entities;
using ToDoApp.Entities.Models;

namespace ToDoApp.Repository;

public class NoteRepository : RepositoryBase<Note>, INoteRepository
{   
    public NoteRepository(RepositoryDbContext repositoryDbContext) : base(repositoryDbContext)
    {
    }

    public async Task<Note?> GetNoteById(Guid noteId)
    {
        var note = await FindByCondition(n => n.Id == noteId)
            .FirstOrDefaultAsync();
        return note;
    }

    public async Task<IEnumerable<Note>> GetAllNotes()
    {
        var notes = await FindAll()
            .ToListAsync();
        return notes;
    }

    public async Task<IEnumerable<Note>> GetNotesByUserId(Guid userId)
    {
        var userNotes = await FindByCondition(n => n.UserId == userId)
            .ToListAsync();
        return userNotes;
    }

    public void CreateNote(Note note)
    {
        Create(note);
    }

    public void UpdateNote(Note note)
    {
        Update(note);
    }

    public void DeleteNote(Note note)
    {
        Delete(note);
    }
}