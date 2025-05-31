using UserContacts.Application.Dtos.UserDtos;
using UserContacts.Application.Services.TokenService;
using UserContacts.Application.Services.UserService;

namespace UserContacts.Web.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {

        var authGroup = app.MapGroup("/api/Auth")
            .WithTags("Auth Management");

        // SignUp 
        authGroup.MapPost("/signup", async (UserCreateDto dto, IUserService userService) =>
        {
            var userId = await userService.CreateUserAsync(dto);
            return Results.Ok(new { Message = "User created", UserId = userId });
        })
        .WithName("SignUp")
        .Produces(200)
        .Produces(400);

        //LogIn
        authGroup.MapPost("/login", async (UserLoginDto dto, IUserService userService, ITokenService tokenService) =>
        {
            var user = await userService.GetUserByUserNameAsync(dto.UserName);
            if (user == null)
            {
                return Results.NotFound("User not found");
            }
            var token = tokenService.GenerateRefreshToken();

            return Results.Ok(new { Token = token });
        })
         .WithName("LogIn")
         .Produces(200)
         .Produces(400)
         .Produces(404);

        //LogOut
        authGroup.MapPost("/logout", async (string token, IUserService userService, ITokenService tokenService) =>
        {
            var user = await userService.GetUserByUserNameAsync(token);
            if (user == null)
            {
                return Results.NotFound("User not found");
            }
            return Results.Ok(new { Message = "Logged out successfully" });
        })
         .WithName("LogOut")
         .Produces(200)
         .Produces(400)
         .Produces(404);

        //RefreshToken
        authGroup.MapPost("/refreshToken", async (string token, IUserService userService, ITokenService tokenService) =>
        {
            var user = await userService.GetUserByUserNameAsync(token);
            if (user == null)
            {
                return Results.NotFound("User not found");
            }
            var newToken = tokenService.GenerateRefreshToken();
            return Results.Ok(new { Token = newToken });
        })
         .WithName("RefreshToken")
         .Produces(200)
         .Produces(400)
         .Produces(404);
    }
}
