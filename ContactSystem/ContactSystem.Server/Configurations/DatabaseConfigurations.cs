using ContactSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ContactSystem.Server.Configurations;

public static class DatabaseConfigurations
{
    public static void ConfigureDB(this WebApplicationBuilder builder)
    {

        if (builder.Environment.EnvironmentName == "Testing")
            return;

        var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");

        builder.Services.AddDbContext<AppDbContext>(options =>
          options.UseSqlServer(connectionString));
    }
}
