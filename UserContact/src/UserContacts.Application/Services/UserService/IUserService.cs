using UserContacts.Application.Dtos.UserDtos;
using UserContacts.Domain.Entities;

namespace UserContacts.Application.Services.UserService;

public interface IUserService
{
    Task<long> CreateUserAsync(UserCreateDto userCreateDto);
    Task<User> GetUserByIdAsync(long userId);
    Task<ICollection<UserDto>> GetAllUsersAsync(int skip, int take);
    Task DeleteUserByIdAsync(long userId, string userRole);
    Task<User?> GetUserByUserNameAsync(string userName);
}