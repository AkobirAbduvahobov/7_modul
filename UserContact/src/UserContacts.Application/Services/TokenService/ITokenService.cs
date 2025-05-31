using System.Security.Claims;
using UserContacts.Application.Dtos.UserDtos;

namespace UserContacts.Application.Services.TokenService;

public interface ITokenService
{
    public string GenerateToken(UserDto userDto);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    public string RemoveRefreshTokenAsync(string token);
}