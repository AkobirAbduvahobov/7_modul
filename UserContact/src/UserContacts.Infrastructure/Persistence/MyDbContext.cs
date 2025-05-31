using Microsoft.EntityFrameworkCore;
using UserContacts.Domain.Entities;

namespace UserContacts.Infrastructure.Persistence;
public class MyDbContext : DbContext
{
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
