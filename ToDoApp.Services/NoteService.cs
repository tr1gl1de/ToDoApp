using AutoMapper;
using ToDoApp.Contracts;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Exceptions;
using ToDoApp.Domain.Repositories;
using ToDoApp.Services.Abstraction;

namespace ToDoApp.Services;

public class NoteService : INoteService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public NoteService(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<NoteForReadDto>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var userNotes = await _repositoryManager.NoteRepository
            .GetAllByIUserIdAsync(userId, cancellationToken);
        var userNotesRead = _mapper.Map<IEnumerable<NoteForReadDto>>(userNotes);
        return userNotesRead;
    }

    public async Task<NoteForReadDto> GetByIdAsync(Guid noteId, Guid userId , CancellationToken cancellationToken = default)
    {
        var user = await _repositoryManager.UserRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException(userId);
        }
        
        var note = await _repositoryManager.NoteRepository.GetByIdAsync(noteId, cancellationToken);

        if (note is null)
        {
            throw new NoteNotFoundException(noteId);
        }

        if (note.UserId != user.Id)
        {
            throw new NoteDoesNotBelongToUserException(userId, noteId);
        }
        
        var noteForRead = _mapper.Map<NoteForReadDto>(note);
        return noteForRead;
    }

    public async Task<NoteForReadDto> CreateNoteAsync(Guid userId ,NoteForCreationDto note, CancellationToken cancellationToken = default)
    {
        var user = await _repositoryManager.UserRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException(userId);
        }

        var newNote = _mapper.Map<Note>(note);
        _repositoryManager.NoteRepository.Insert(newNote);
        await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<NoteForReadDto>(newNote);
    }

    public async Task UpdateNoteAsync(Guid userId ,Guid id ,NoteForUpdateDto note, CancellationToken cancellationToken = default)
    {
        var user = await _repositoryManager.UserRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException(userId);
        }
        
        var oldNote = await _repositoryManager.NoteRepository
            .GetByIdAsync(id, cancellationToken);
        
        if (oldNote is null)
        {
            throw new NoteNotFoundException(id);
        }
        
        note.DateUpdate = DateTime.Now;
        _mapper.Map(note, oldNote);
        await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteNoteAsync(Guid userId ,Guid noteId, CancellationToken cancellationToken = default)
    {
        var user = await _repositoryManager.UserRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException(userId);
        }
        
        var oldNote = await _repositoryManager.NoteRepository
            .GetByIdAsync(noteId, cancellationToken);
        if (oldNote is null)
        {
            throw new NoteNotFoundException(noteId);
        }
        _repositoryManager.NoteRepository.Remove(oldNote);
        await _repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}