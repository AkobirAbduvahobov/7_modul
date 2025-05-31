using UserContacts.Application.Dtos.LoginResponseDto;
using UserContacts.Application.Dtos.RefreshTokenDtos;
using UserContacts.Application.Dtos.UserDtos;

namespace UserContacts.Application.Services.AuthService;

public interface IAuthService
{
    Task<long> SignUpUserAsync(UserCreateDto userCreateDto);
    Task<LoginResponseDto> LoginUserAsync(UserLoginDto userLoginDto);
    Task<LoginResponseDto> RefreshTokenAsync(RefreshRequestDto request);
    Task LogOutAsync(string token);
}