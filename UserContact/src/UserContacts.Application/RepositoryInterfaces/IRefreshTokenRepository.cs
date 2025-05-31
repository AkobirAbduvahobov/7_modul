using UserContacts.Domain.Entities;

namespace UserContacts.Application.RepositoryInterfaces;
public interface IRefreshTokenRepository
{
    Task InsertRefreshTokenAsync(RefreshToken refreshToken);
    Task<RefreshToken> SelectRefreshTokenAsync(string refreshToken, long userId);
    Task RemoveRefreshTokenAsync(string token);
    Task UpdateRefreshTokenAsync(RefreshToken refreshToken);
}
