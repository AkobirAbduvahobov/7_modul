using FluentValidation;
using Infrastructure.Persistence.Repositories;
using UserContacts.Application.Dtos.ContactDtos;
using UserContacts.Application.Dtos.UserDtos;
using UserContacts.Application.RepositoryInterfaces;
using UserContacts.Application.Services.AuthService;
using UserContacts.Application.Services.ContactService;
using UserContacts.Application.Services.TokenService;
using UserContacts.Application.Services.UserRoleService;
using UserContacts.Application.Services.UserService;
using UserContacts.Application.Validators.ContactValidator;
using UserContacts.Application.Validators.UserValidator;
using UserContacts.Infrastructure.Persistence.Repositories;

namespace UserContacts.Web.Configurations;

public static class DependicyInjectionConfigurations
{
    public static void ConfigureDI(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserService, UserService>();

        builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        builder.Services.AddScoped<IUserRoleService, UserRoleService>();

        builder.Services.AddScoped<IContactRepository, ContactRepository>();
        builder.Services.AddScoped<IContactService, ContactService>();

        builder.Services.AddScoped<IAuthService, AuthService>();

        builder.Services.AddScoped<ITokenService, TokenService>();

        builder.Services.AddScoped<IValidator<UserCreateDto>, UserCreateValidator>();
        builder.Services.AddScoped<IValidator<UserLoginDto>, UserLoginValidator>();
        builder.Services.AddScoped<IValidator<ContactCreateDto>, ContactCreateValidator>();
        builder.Services.AddScoped<IValidator<ContactDto>, ContactUpdateValidator>();

    }
}
