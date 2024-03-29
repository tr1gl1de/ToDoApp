﻿using Microsoft.EntityFrameworkCore;
using ToDo.Persistence;
using ToDoApp.Contracts;
using ToDoApp.Entities.Models;

namespace ToDoApp.Repository;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(RepositoryDbContext repositoryDbContext) : base(repositoryDbContext)
    {
    }

    public async Task<User?> GetUserById(Guid id)
    {
        var user = await FindByCondition(u => u.Id == id)
            .FirstOrDefaultAsync();
        return user;
    }

    public async Task<User?> GetUserByUsername(string username)
    {
        var user = await FindByCondition(u => u.Username == username)
            .FirstOrDefaultAsync();
        return user;
    }

    public async Task<bool> UserIsExits(string username)
    {
        var userIsExits = await FindByCondition(
            u => u.Username.Equals(username))
            .AnyAsync();
        return userIsExits;
    }

    public async Task<bool> UserIsExits(Guid id)
    {
        var userIsExits = await FindByCondition(
                u => u.Id.Equals(id))
            .AnyAsync();
        return userIsExits;
    }

    public void CreateUser(User user)
    {
        Create(user);
    }

    // for test
    public async Task<User?> GetUserByUsernameWithNotes(string username)
    {
        var user = await FindByCondition(u => u.Username == username)
            .Include(u => u.Notes)
            .FirstOrDefaultAsync();
        return user;
    }
}