using UserContacts.Domain.Entities;

namespace UserContacts.Application.RepositoryInterfaces;
public interface IUserRepository
{
    Task<long> InsertUserAsync(User user);
    Task<User> SelectUserByIdAsync(long id);
    Task<User?> SelectUserByUserNameAsync(string userName);
    Task UpdateUserRoleAsync(long userId, UserRole userRole);
    Task DeleteUserAsync(long userId);
    Task<ICollection<User>> SelectAllUsersAsync(int skip, int take);
}
