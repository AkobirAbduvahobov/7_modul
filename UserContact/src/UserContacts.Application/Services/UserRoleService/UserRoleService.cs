using UserContacts.Application.Dtos.UserRoleDtos;
using UserContacts.Application.RepositoryInterfaces;
using UserContacts.Core.Errors;
using UserContacts.Domain.Entities;

namespace UserContacts.Application.Services.UserRoleService;
public class UserRoleService : IUserRoleService
{
    private readonly IUserRoleRepository UserRoleRepository;
    private readonly IUserRepository UserRepository;

    public UserRoleService(IUserRoleRepository userRoleRepository, IUserRepository userRepository)
    {
        UserRoleRepository = userRoleRepository;
        UserRepository = userRepository;
    }

    public async Task<long> AddUserRoleAsync(UserRole userRole)
    {
        var task = await UserRoleRepository.InsertUserRoleAsync(userRole);

        return task;
    }

    public async Task UpdateUserRoleAsync(long userId, UserRoleDto userRoleDto)
    {
        var user = await UserRepository.SelectUserByIdAsync(userId);
        if (user == null)
        {
            throw new EntityNotFoundException($"User with ID {userId} not found.");
        }

        user.Role = new UserRole
        {
            RoleName = userRoleDto.RoleName,
            Description = userRoleDto.Description
        };

        await UserRepository.UpdateUserRoleAsync(userId, user.Role);
    }
}
