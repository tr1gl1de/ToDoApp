﻿using ToDoApp.Contracts;
using ToDoApp.Entities;

namespace ToDoApp.Repository;

public class RepositoryWrapper : IRepositoryWrapper
{
    private RepositoryDbContext _repositoryDbContext;
    private IUserRepository _user;
    private INoteRepository _note;

    public RepositoryWrapper(
        RepositoryDbContext repositoryDbContext)
    {
        _repositoryDbContext = repositoryDbContext;
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