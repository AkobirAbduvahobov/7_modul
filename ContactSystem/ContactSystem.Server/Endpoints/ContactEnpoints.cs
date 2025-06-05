using ContactSystem.Application.Dtos;
using ContactSystem.Application.Services;

namespace ContactSystem.Server.Endpoints;

public static class ContactEnpoints
{
    public static void MapContactEndpoints(this WebApplication app)
    {
        var contactGroup = app.MapGroup("/api/contact")
            //.RequireAuthorization()       
            .WithTags("Contacts");

        contactGroup.MapDelete("/delete",
        async (long contactId, IContactService contactService) =>
        {
            await contactService.DeleteAsync(contactId);
        })
            .WithName("DeleteContact")
            .Produces(200)
            .Produces(404);


        contactGroup.MapPut("/put",
        async (ContactDto contactDto, IContactService contactService) =>
        {
            await contactService.UpdateAsync(contactDto);
        })
            .WithName("UpdateContact")
            .Produces(200)
            .Produces(404)
            .Produces(422);


        contactGroup.MapPost("/post",
        async (ContactCreateDto contactDto, IContactService contactService) =>
        {
            return await contactService.PostAsync(contactDto);
        })
            .WithName("PostContact")
            .Produces(200)
            .Produces(422);


        contactGroup.MapGet("/get/{contactId}",
        async (long contactId, IContactService contactService) =>
        {
            return await contactService.GetByIdAsync(contactId);
        })
            .WithName("GetContactById")
            .Produces(200)
            .Produces(404);


        contactGroup.MapGet("/get-all",
        (IContactService contactService) =>
        {
            return contactService.GetAll();
        })
            .WithName("GetAllContacts")
            .Produces(200)
            .Produces(422);
    }
}
