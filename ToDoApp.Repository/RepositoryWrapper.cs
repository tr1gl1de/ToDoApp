﻿using ToDo.Persistence;
using ToDoApp.Contracts;
using ToDoApp.Entities.Helpers;
using ToDoApp.Entities.Models;

namespace ToDoApp.Repository;

public class RepositoryWrapper : IRepositoryWrapper
{
    private readonly RepositoryDbContext _repositoryDbContext;
    private IUserRepository _user;
    private INoteRepository _note;
    private IRefreshTokenRepository _refreshToken;
    private readonly ISortHelper<Note> _helper;


    public RepositoryWrapper(
        ISortHelper<Note> helper,
        RepositoryDbContext repositoryDbContext)
    {
        _repositoryDbContext = repositoryDbContext;
        _helper = helper;
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
                _note = new NoteRepository(_helper ,_repositoryDbContext);
            }

            return _note;
        }
    }

    public IRefreshTokenRepository RefreshToken
    {
        get
        {
            if (_refreshToken is null)
            {
                _refreshToken = new RefreshTokenRepository(_repositoryDbContext);
            }

            return _refreshToken;
        }
    }
}