using UserContacts.Application.Dtos.ContactDtos;
using UserContacts.Application.Services.ContactService;
using UserContacts.Domain.Entities;

namespace UserContacts.Web.Endpoints;

public static class ContactEndpoints
{
    public static void MapContactEndpoints(this WebApplication app)
    {
        var contactGroup = app.MapGroup("/api/contact")
            .RequireAuthorization()       // Require [Authorize] globally for this group
            .WithTags("Contact Management"); // Swagger section name


        // Create Contact
        contactGroup.MapPost("/create", async (ContactCreateDto dto, IContactService contactService) =>
        {
            var contactId = await contactService.AddContactAsync(dto);
            return Results.Ok(new { Message = "Contact created", ContactId = contactId });
        })
        .WithName("CreateContact")
        .Produces(200)
        .Produces(400);


        // Delete Contact
        contactGroup.MapDelete("/delete", async (long contactId, IContactService contactService) =>
        {
            await contactService.DeleteContactAsync(contactId);
            return Results.Ok();
        })
        .WithName("DeleteContact")
        .Produces(200)
        .Produces(404);


        // Get All Contacts
        contactGroup.MapGet("/getAll", async (long userId, int skip, int take, IContactService contactService) =>
        {
            var contacts = await contactService.GetAllContactsByUserAsync(userId, skip, take);
            return Results.Ok(contacts);
        })
        .WithName("GetAllContacts")
        .Produces<ICollection<ContactDto>>(200);


        // Update Contact
        contactGroup.MapPatch("/update", async (ContactDto contact, IContactService contactService) =>
        {
            await contactService.UpdateContactAsync(contact);
            return Results.Ok();
        })
        .WithName("UpdateContact")
        .Produces(200)
        .Produces(404);


        // Get Contact by ID
        contactGroup.MapGet("/getById{contactId:long}", async (long contactId, IContactService contactService) =>
        {
            var contact = await contactService.GetContactByIdAsync(contactId);
            return Results.Ok(contact);
        })
        .WithName("GetContactById")
        .Produces<Contact>(200)
        .Produces(404);
    }
}
