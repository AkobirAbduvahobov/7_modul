using Microsoft.EntityFrameworkCore;
using UserContacts.Infrastructure.Persistence;

namespace UserContacts.Web.Configurations;

public static class DatabaseConfigurations
{
    public static void ConfigureDB(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");

        builder.Services.AddDbContext<MyDbContext>(options =>
          options.UseSqlServer(connectionString));
    }
}
