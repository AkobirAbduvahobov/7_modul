using UserContacts.Web.Configurations;
using UserContacts.Web.Endpoints;
using UserContacts.Web.Middlewares;

namespace UserContacts.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.ConfigureSerilog();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.ConfigureDB();
        builder.ConfigureDI();
        builder.ConfigureJwtAuth();

        var app = builder.Build();

        app.UseGlobalExceptionHandling();
        app.UseMiddleware<RequestDurationMiddleware>();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.MapAdminEndpoints();
        app.MapAuthEndpoints();
        app.MapContactEndpoints();
        app.MapRoleEndpoints();

        app.Run();
    }
}
