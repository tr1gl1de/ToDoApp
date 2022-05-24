using ToDoApp.Contracts;

namespace ToDoApp.Services.Abstraction;

public interface INoteService
{
    Task<IEnumerable<NoteForReadDto>> GetAllByUserIdAsync(Guid userId,
        CancellationToken cancellationToken = default);

    Task<NoteForReadDto> GetByIdAsync(Guid noteId, CancellationToken cancellationToken = default);

    Task<NoteForReadDto> CreateNoteAsync(NoteForCreationDto note, CancellationToken cancellationToken = default);

    Task UpdateNoteAsync(Guid noteId ,NoteForUpdateDto note, CancellationToken cancellationToken = default);

    Task DeleteNoteAsync(Guid noteId, CancellationToken cancellationToken = default);
}