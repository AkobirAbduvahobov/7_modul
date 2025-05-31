using FluentValidation;
using UserContacts.Application.Dtos.UserDtos;
using UserContacts.Application.Mappers.UserMapper;
using UserContacts.Application.RepositoryInterfaces;
using UserContacts.Application.Services.Helpers.Security;
using UserContacts.Core.Errors;
using UserContacts.Domain.Entities;

namespace UserContacts.Application.Services.UserService;
public class UserService : IUserService
{
    private readonly IUserRepository UserRepository;
    private readonly IValidator<UserCreateDto> UserCreateValidator;


    public UserService(IUserRepository userRepository, IValidator<UserCreateDto> userCreateValidator)
    {
        UserRepository = userRepository;
        UserCreateValidator = userCreateValidator;
    }

    public async Task<long> CreateUserAsync(UserCreateDto userCreateDto)
    {
        var validationResult = await UserCreateValidator.ValidateAsync(userCreateDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        var salt = PasswordHasher.Hasher(userCreateDto.Password).Salt;

        var hashedPassword = PasswordHasher.Hasher(userCreateDto.Password).Hash;

        var user = MapUser.ConvertToUser(userCreateDto, hashedPassword, salt);

        await UserRepository.InsertUserAsync(user);

        return user.UserId;
    }

    public async Task DeleteUserByIdAsync(long userId, string userRole)
    {
        var user = await GetUserByIdAsync(userId);
        if (user == null)
        {
            throw new EntityNotFoundException($"User with ID {userId} not found.");
        }
        if (userRole != "Admin")
        {
            throw new ForbiddenException("You do not have permission to delete this user");
        }

        await UserRepository.DeleteUserAsync(userId);
    }

    public async Task<ICollection<UserDto>> GetAllUsersAsync(int skip, int take)
    {
        var users = await UserRepository.SelectAllUsersAsync(skip, take);
        if (users == null || users.Count == 0)
        {
            throw new EntityNotFoundException("No users found.");
        }

        return users.Select(MapUser.ConvertToUserDto).ToList();
    }

    public async Task<User> GetUserByIdAsync(long userId)
    {
        var user = await UserRepository.SelectUserByIdAsync(userId);
        if (user == null)
        {
            throw new EntityNotFoundException($"User with ID {userId} not found.");
        }
        return user;
    }

    public async Task<User?> GetUserByUserNameAsync(string userName)
    {
        var user = await UserRepository.SelectUserByUserNameAsync(userName);
        if (user == null)
        {
            throw new EntityNotFoundException($"User with username {userName} not found.");
        }
        return user;
    }
}
