using UserContacts.Domain.Entities;

namespace UserContacts.Application.RepositoryInterfaces;
public interface IUserRoleRepository
{
    Task<long> InsertUserRoleAsync(UserRole userRole);
    Task UpdateUserRoleAsync(UserRole userRole);
}


