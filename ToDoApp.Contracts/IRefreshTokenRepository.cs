using ToDoApp.Entities.Models;

namespace ToDoApp.Contracts;

public interface IRefreshTokenRepository
{
    void AddRefreshToken(RefreshToken token);
    Task<RefreshToken?> GetRefreshTokenByIdAsync(Guid refreshTokenId);
    void DeleteRefreshToken(RefreshToken token);
}