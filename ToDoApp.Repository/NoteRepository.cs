using Microsoft.EntityFrameworkCore;
using ToDoApp.Contracts;
using ToDoApp.Entities;
using ToDoApp.Entities.DataTransferObjects.QueryStringParameters;
using ToDoApp.Entities.Helpers;
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

    public async Task<PagedList<Note>> GetNotesByUserId(Guid userId, NoteQueryStringParameters notesParam)
    {
        var userNotes = FindByCondition(n => n.UserId == userId);
        
        return await PagedList<Note>.ToPagedListAsync(userNotes, 
            notesParam.PageNumber,
            notesParam.PageSize);
    }

    public async Task<IEnumerable<Note>> SearchNotesByNameAsync(Guid userId ,string name)
    {
        var notes = FindByCondition(n => n.UserId == userId);
        
        SearchByName(ref notes, name);

        var resultNotes = await notes.ToListAsync();
        
        return resultNotes;
    }

    public async Task<PagedList<Note>> SearchNotesByNameAsync(Guid userId, NoteQueryStringParametersForSearch notesParam)
    {
        var notes = FindByCondition(n => n.UserId == userId);
        
        SearchByName(ref notes, notesParam.Name);
        
        return await PagedList<Note>.ToPagedListAsync(notes,
            notesParam.PageNumber,
            notesParam.PageSize);
    }

    private void SearchByName(ref IQueryable<Note> notes, string name)
    {
        if (!notes.Any() || string.IsNullOrWhiteSpace(name))
        {
            return;
        }
        
        notes = notes.Where(n => n.Name
            .ToLower()
            .Contains(name
                .Trim()
                .ToLower())
        );
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