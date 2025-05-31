using FluentValidation;
using System.Security.Claims;
using UserContacts.Application.Dtos.LoginResponseDto;
using UserContacts.Application.Dtos.RefreshTokenDtos;
using UserContacts.Application.Dtos.UserDtos;
using UserContacts.Application.Mappers.UserMapper;
using UserContacts.Application.RepositoryInterfaces;
using UserContacts.Application.Services.Helpers.Security;
using UserContacts.Application.Services.TokenService;
using UserContacts.Core.Errors;
using UserContacts.Domain.Entities;

namespace UserContacts.Application.Services.AuthService;
public class AuthService : IAuthService
{
    private readonly IRefreshTokenRepository RefreshTokenRepository;
    private readonly IUserRepository UserRepository;
    private readonly ITokenService TokenService;
    private readonly IValidator<UserCreateDto> UserValidator;
    private readonly IValidator<UserLoginDto> UserLoginValidator;

    public AuthService(IRefreshTokenRepository refreshTokenRepository, IUserRepository userRepository, ITokenService tokenService, IValidator<UserCreateDto> userValidator, IValidator<UserLoginDto> userLoginValidator)
    {
        RefreshTokenRepository = refreshTokenRepository;
        UserRepository = userRepository;
        TokenService = tokenService;
        UserValidator = userValidator;
        UserLoginValidator = userLoginValidator;
    }

    public async Task<LoginResponseDto> LoginUserAsync(UserLoginDto userLoginDto)
    {
        var validationResult = await UserLoginValidator.ValidateAsync(userLoginDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var user = await UserRepository.SelectUserByUserNameAsync(userLoginDto.UserName);

        var checkUserPassword = PasswordHasher.Verify(userLoginDto.Password, user.Password, user.Salt);

        if (checkUserPassword == false)
        {
            throw new UnauthorizedException("UserName or password incorrect");
        }

        var userGetDto = MapUser.ConvertToUserDto(user);

        var accessToken = TokenService.GenerateToken(userGetDto);

        var refreshToken = CreateRefreshToken(Guid.NewGuid().ToString(), user.UserId);

        await RefreshTokenRepository.InsertRefreshTokenAsync(refreshToken);

        var loginResponseDto = new LoginResponseDto()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            TokenType = "Bearer",
            ExpiresInMinutes = 25,
            AccessTokenExpiration = DateTime.UtcNow.AddMinutes(25)
        };

        return loginResponseDto;
    }

    public async Task<LoginResponseDto> RefreshTokenAsync(RefreshRequestDto request)
    {
        ClaimsPrincipal? principal = TokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal == null) throw new ForbiddenException("Invalid access token.");

        var userClaim = principal.FindFirst(c => c.Type == "UserId");
        var userId = long.Parse(userClaim.Value);

        var refreshToken = await RefreshTokenRepository.SelectRefreshTokenAsync(request.RefreshToken, userId);
        if (refreshToken == null || refreshToken.ExpirationDate < DateTime.UtcNow || refreshToken.IsRevoked)
            throw new UnauthorizedException("Invalid or expired refresh token.");

        refreshToken.IsRevoked = true;
        await RefreshTokenRepository.UpdateRefreshTokenAsync(refreshToken);

        var user = await UserRepository.SelectUserByIdAsync(userId);

        var userGetDto = MapUser.ConvertToUserDto(user);

        var newAccessToken = TokenService.GenerateToken(userGetDto);
        var newRefreshToken = TokenService.GenerateRefreshToken();

        var refreshTokenToDB = new RefreshToken()
        {
            Token = newRefreshToken,
            ExpirationDate = DateTime.UtcNow.AddDays(21),
            IsRevoked = false,
            UserId = user.UserId
        };

        await RefreshTokenRepository.InsertRefreshTokenAsync(refreshTokenToDB);

        return new LoginResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            TokenType = "Bearer",
            ExpiresInMinutes = 25,
            AccessTokenExpiration = DateTime.UtcNow.AddMinutes(25)
        };
    }

    public async Task<long> SignUpUserAsync(UserCreateDto userCreateDto)
    {
        var validationResult = await UserValidator.ValidateAsync(userCreateDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var tupleFromHasher = PasswordHasher.Hasher(userCreateDto.Password);

        var user = MapUser.ConvertToUser(userCreateDto, tupleFromHasher.Hash, tupleFromHasher.Salt);

        var userId = await UserRepository.InsertUserAsync(user);

        var refreshToken = CreateRefreshToken(Guid.NewGuid().ToString(), userId);

        await RefreshTokenRepository.InsertRefreshTokenAsync(refreshToken);

        var accessToken = TokenService.GenerateToken(MapUser.ConvertToUserDto(user));
        return userId;
    }

    public async Task LogOutAsync(string token)
    {
        await RefreshTokenRepository.RemoveRefreshTokenAsync(token);
    }

    private static RefreshToken CreateRefreshToken(string token, long userId)
    {
        return new RefreshToken
        {
            Token = token,
            ExpirationDate = DateTime.UtcNow.AddDays(21),
            IsRevoked = false,
            UserId = userId
        };
    }
}
