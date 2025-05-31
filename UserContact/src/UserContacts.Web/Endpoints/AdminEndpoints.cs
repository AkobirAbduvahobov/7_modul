using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserContacts.Application.Dtos.UserDtos;
using UserContacts.Application.Services.UserService;

namespace UserContacts.Web.Endpoints;

public static class AdminEndpoints
{
    public static void MapAdminEndpoints(this WebApplication app)
    {

        var adminGroup = app.MapGroup("/api/admin")
            .RequireAuthorization("Admin")
            .WithTags("Admin Management");

        //GetAllUsers
        adminGroup.MapGet("/getAll", async (int skip, int take, IUserService userService, [FromServices] ILogger<Program> logger) =>
        {
            logger.LogInformation("Fetching all users with skip: {Skip}, take: {Take}", skip, take);
            var users = await userService.GetAllUsersAsync(skip, take);
            return Results.Ok(users);
        })
        .WithName("GetAllUsers")
        .Produces<ICollection<UserDto>>(200);

        //Delete
        adminGroup.MapDelete("/delete", async (long userId, HttpContext httpContext, IUserService userService, [FromServices] Logger<Program> logger) =>
            {
                logger.LogInformation("Attempting to delete user with ID: {UserId}", userId);

                var role = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
                await userService.DeleteUserByIdAsync(userId, role);
                return Results.Ok();
            })
            .WithName("DeleteUser")
            .Produces(200)
            .Produces(404);

        //Post
        adminGroup.MapPost("/create", async (UserCreateDto dto, IUserService userService) =>
        {
            var userId = await userService.CreateUserAsync(dto);
            return Results.Ok(new { Message = "User created", UserId = userId });
        })
        .WithName("CreateUser")
        .Produces(200)
        .Produces(400);
    }
}
