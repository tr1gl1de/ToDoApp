namespace ToDoApp.Domain.Repositories;

public interface IRepositoryManager
{
    INoteRepository NoteRepository { get; }
    
    IUserRepository UserRepository { get; }
    
    IUnitOfWork UnitOfWork { get; }
}