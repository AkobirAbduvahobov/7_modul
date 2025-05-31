using UserContacts.Application.RepositoryInterfaces;
using UserContacts.Domain.Entities;
using UserContacts.Infrastructure.Persistence;

namespace Infrastructure.Persistence.Repositories;
public class UserRoleRepository : IUserRoleRepository
{
    private readonly MyDbContext MyDbContext;

    public UserRoleRepository(MyDbContext myDbContext)
    {
        MyDbContext = myDbContext;
    }

    public async Task<long> InsertUserRoleAsync(UserRole userRole)
    {
        await MyDbContext.UserRoles.AddAsync(userRole);
        await MyDbContext.SaveChangesAsync();
        return userRole.UserRoleId;
    }

    public async Task UpdateUserRoleAsync(UserRole userRole)
    {
        MyDbContext.UserRoles.Update(userRole);
        await MyDbContext.SaveChangesAsync();
    }
}
