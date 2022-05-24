using ToDoApp.Contracts;

namespace ToDoApp.Services.Abstraction;

public interface INoteService
{
    Task<IEnumerable<NoteForReadDto>> GetAllByUserIdAsync(Guid userId,
        CancellationToken cancellationToken = default);

    Task<NoteForReadDto> GetByIdAsync(Guid noteId, Guid userId, CancellationToken cancellationToken = default);

    Task<NoteForReadDto> CreateNoteAsync(Guid userId ,NoteForCreationDto note, CancellationToken cancellationToken = default);

    Task UpdateNoteAsync(Guid userId ,Guid noteId ,NoteForUpdateDto note, CancellationToken cancellationToken = default);

    Task DeleteNoteAsync(Guid userId ,Guid noteId, CancellationToken cancellationToken = default);
}