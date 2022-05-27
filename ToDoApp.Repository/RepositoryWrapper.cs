using ToDoApp.Contracts;
using ToDoApp.Entities;

namespace ToDoApp.Repository;

public class RepositoryWrapper : IRepositoryWrapper
{
    private readonly RepositoryDbContext _repositoryDbContext;
    private IUserRepository _user;
    private INoteRepository _note;

    public RepositoryWrapper(
        RepositoryDbContext repositoryDbContext,
        IUserRepository user,
        INoteRepository note)
    {
        _repositoryDbContext = repositoryDbContext;
        _user = user;
        _note = note;
    }

    public async Task SaveAsync()
    {
        await _repositoryDbContext.SaveChangesAsync();
    }

    public IUserRepository User
    {
        get
        {
            if (_user is null)
            {
                _user = new UserRepository(_repositoryDbContext);
            }

            return _user;
        }
    }

    public INoteRepository Note
    {
        get
        {
            if (_note is null)
            {
                _note = new NoteRepository(_repositoryDbContext);
            }

            return _note;
        }
    }
}