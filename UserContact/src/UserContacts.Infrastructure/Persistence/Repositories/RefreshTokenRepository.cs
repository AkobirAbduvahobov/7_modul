using Microsoft.EntityFrameworkCore;
using UserContacts.Application.RepositoryInterfaces;
using UserContacts.Core.Errors;
using UserContacts.Domain.Entities;
using UserContacts.Infrastructure.Persistence;

namespace Infrastructure.Persistence.Repositories;
public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly MyDbContext MyDbContext;

    public RefreshTokenRepository(MyDbContext myDbContext)
    {
        MyDbContext = myDbContext;
    }
    public async Task InsertRefreshTokenAsync(RefreshToken refreshToken)
    {
        await MyDbContext.RefreshTokens.AddAsync(refreshToken);
        await MyDbContext.SaveChangesAsync();
    }
    public async Task RemoveRefreshTokenAsync(string token)
    {
        var rToken = await MyDbContext.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token);
        if (rToken == null)
        {
            throw new KeyNotFoundException($"Refresh token {token} not found.");
        }
        MyDbContext.RefreshTokens.Remove(rToken);
        await MyDbContext.SaveChangesAsync();
    }
    public async Task<RefreshToken> SelectRefreshTokenAsync(string refreshToken, long userId)
    {
        var token = await MyDbContext.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.UserId == userId);
        if (token == null)
        {
            throw new KeyNotFoundException($"Refresh token {refreshToken} for user ID {userId} not found.");
        }
        return token;
    }

    public async Task UpdateRefreshTokenAsync(RefreshToken refreshToken)
    {
        var existingToken = await MyDbContext.RefreshTokens
           .FirstOrDefaultAsync(t => t.Token == refreshToken.Token && t.UserId == refreshToken.UserId);
        if (existingToken == null)
        {
            throw new EntityNotFoundException($"Refresh token {refreshToken.Token} not found for user {refreshToken.UserId}");
        }
        existingToken.IsRevoked = refreshToken.IsRevoked;

        existingToken.ExpirationDate = refreshToken.ExpirationDate;

        MyDbContext.RefreshTokens.Update(existingToken);
        await MyDbContext.SaveChangesAsync();
    }
}
