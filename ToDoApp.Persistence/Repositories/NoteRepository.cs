using Microsoft.EntityFrameworkCore;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Repositories;

namespace ToDoApp.Persistence.Repositories;

public class NoteRepository : INoteRepository
{
    private readonly RepositoryDbContext _context;

    public NoteRepository(RepositoryDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Note>> GetAllByIUserIdAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        var userNotes = await _context.Notes
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);
        return userNotes;
    }

    public async Task<Note> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var note = await _context.Notes
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return note;
    }

    public void Insert(Note note)
    {
        _context.Notes.Add(note);
    }

    public void Remove(Note note)
    {
        _context.Notes.Remove(note);
    }
}