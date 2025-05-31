using UserContacts.Application.Dtos.UserRoleDtos;
using UserContacts.Domain.Entities;

namespace UserContacts.Application.Services.UserRoleService;

public interface IUserRoleService
{
    Task<long> AddUserRoleAsync(UserRole userRole);
    Task UpdateUserRoleAsync(long userId, UserRoleDto userRoleDto);
}