using UserContacts.Application.Dtos.UserDtos;
using UserContacts.Application.Dtos.UserRoleDtos;
using UserContacts.Domain.Entities;

namespace UserContacts.Application.Mappers.UserMapper;
public static class MapUser
{
    public static UserDto ConvertToUserDto(User user)
    {
        return new UserDto()
        {
            UserId = user.UserId,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Role = new UserRoleDto()
            {
                UserRoleId = user.Role.UserRoleId,
                RoleName = user.Role.RoleName,
                Description = user.Role.Description
            }
        };
    }
    public static User ConvertToUser(UserCreateDto dto, string hashedPassword, string salt)
    {
        return new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            UserName = dto.UserName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Password = hashedPassword,
            Salt = salt,
            Role = new UserRole
            {
                RoleName = "User",
                Description = "Default role for new users",
            }
        };
    }
}
