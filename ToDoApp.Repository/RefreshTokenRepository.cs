using Microsoft.EntityFrameworkCore;
using ToDoApp.Contracts;
using ToDoApp.Entities;
using ToDoApp.Entities.Models;

namespace ToDoApp.Repository;

public class RefreshTokenRepository : RepositoryBase<RefreshToken>, IRefreshTokenRepository
{
    public RefreshTokenRepository(RepositoryDbContext repositoryDbContext) : base(repositoryDbContext)
    {
    }
    
    public void AddRefreshToken(RefreshToken token)
    {
        Create(token);
    }

    public async Task<RefreshToken?> GetRefreshTokenByIdAsync(Guid refreshTokenId)
    {
        var refreshToken = await FindByCondition(rt => rt.Id == refreshTokenId)
            .FirstOrDefaultAsync();
        return refreshToken;
    }

    public void DeleteRefreshToken(RefreshToken token)
    {
        Delete(token);
    }
}