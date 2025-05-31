using Microsoft.AspNetCore.Authorization;
using UserContacts.Application.Dtos.UserRoleDtos;
using UserContacts.Application.Services.UserRoleService;
using UserContacts.Domain.Entities;

namespace UserContacts.Web.Endpoints;

public static class RoleEndpoints
{
    public static void MapRoleEndpoints(this WebApplication app)
    {
        var roleGroup = app.MapGroup("/api/role")
            .RequireAuthorization()
            .WithTags("Role Management");


        // Post
        roleGroup.MapPost("/create", [Authorize(Roles = "Admin,SuperAdmin")]
        async (UserRole userRole, IUserRoleService userRoleService) =>
        {
            var roleId = await userRoleService.AddUserRoleAsync(userRole);
            return Results.Ok(new { Message = "User Role created", RoleId = roleId });
        })
        .WithName("CreateUserRole")
        .Produces(200)
        .Produces(400);

        // Put
        roleGroup.MapPut("/update", [Authorize(Roles = "Admin,SuperAdmin")]
        async (long userId, UserRoleDto userRoleDto, IUserRoleService userRoleService) =>
        {
            await userRoleService.UpdateUserRoleAsync(userId, userRoleDto);
            return Results.Ok(new { Message = "User Role updated" });
        })
        .WithName("UpdateUserRole")
        .Produces(200)
        .Produces(404)
        .Produces(400);
    }
}
