using ToDoApp.Domain.Entities;

namespace ToDoApp.Domain.Repositories;

public interface INoteRepository
{
    public Task<IEnumerable<Note>> GetAllByIUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    
    public Task<Note> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    void Insert(Note note);

    void Remove(Note note);
}