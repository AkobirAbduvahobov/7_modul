using Microsoft.EntityFrameworkCore;
using UserContacts.Application.RepositoryInterfaces;
using UserContacts.Domain.Entities;
using UserContacts.Infrastructure.Persistence;

namespace Infrastructure.Persistence.Repositories;
public class UserRepository : IUserRepository
{
    private readonly MyDbContext MyDbContext;

    public UserRepository(MyDbContext myDbContext)
    {
        MyDbContext = myDbContext;
    }

    public async Task DeleteUserAsync(long userId)
    {
        var user = await SelectUserByIdAsync(userId);
        MyDbContext.Users.Remove(user);
        await MyDbContext.SaveChangesAsync();
    }

    public async Task<long> InsertUserAsync(User user)
    {
        await MyDbContext.Users.AddAsync(user);
        await MyDbContext.SaveChangesAsync();
        return user.UserId;
    }

    public async Task<ICollection<User>> SelectAllUsersAsync(int skip, int take)
    {
        return await MyDbContext.Users
             .Skip(skip)
             .Take(take)
             .ToListAsync();
    }

    public async Task<User> SelectUserByIdAsync(long id)
    {
        var user = await MyDbContext.Users.FirstOrDefaultAsync(u => u.UserId == id);
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {id} not found.");
        }
        return user;
    }

    public async Task<User?> SelectUserByUserNameAsync(string userName)
    {
        return await MyDbContext.Users
            .FirstOrDefaultAsync(u => u.UserName == userName);
    }

    public async Task UpdateUserRoleAsync(long userId, UserRole userRole)
    {
        var user = await SelectUserByIdAsync(userId);
        MyDbContext.Users.Update(user);

    }
}
