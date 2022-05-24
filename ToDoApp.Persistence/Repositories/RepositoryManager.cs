using ToDoApp.Domain.Repositories;

namespace ToDoApp.Persistence.Repositories;

public class RepositoryManager : IRepositoryManager
{
    private readonly Lazy<INoteRepository> _lazyNoteRepository;
    private readonly Lazy<IUserRepository> _lazyUserRepository;
    private readonly Lazy<IUnitOfWork> _lazyUnitOfWork;

    public RepositoryManager(RepositoryDbContext dbContext)
    {
        _lazyNoteRepository = new Lazy<INoteRepository>(() => new NoteRepository(dbContext));
        _lazyUserRepository = new Lazy<IUserRepository>(() => new UserRepository(dbContext));
        _lazyUnitOfWork = new Lazy<IUnitOfWork>(() => new UnitOfWork(dbContext));
    }

    public INoteRepository NoteRepository => _lazyNoteRepository.Value;
    
    public IUserRepository UserRepository => _lazyUserRepository.Value;

    public IUnitOfWork UnitOfWork => _lazyUnitOfWork.Value;
}