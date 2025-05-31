using UserContacts.Application.Dtos.ContactDtos;
using UserContacts.Domain.Entities;

namespace UserContacts.Application.Mappers.ContactMapper;
public static class MapContact
{
    public static ContactDto ConvertToContactDto(Contact contact)
    {
        return new ContactDto()
        {
            ContactId = contact.ContactId,
            FirstName = contact.FirstName,
            LastName = contact.LastName,
            PhoneNumber = contact.PhoneNumber,
            Email = contact.Email,
            CreatedAt = contact.CreatedAt,
            UserId = contact.UserId
        };
    }
    public static Contact ConvertToContact(ContactCreateDto contactCreateDto)
    {
        return new Contact
        {
            FirstName = contactCreateDto.FirstName,
            LastName = contactCreateDto.LastName,
            PhoneNumber = contactCreateDto.PhoneNumber,
            Email = contactCreateDto.Email,
            UserId = contactCreateDto.UserId
        };
    }
    public static Contact ConvertToContactEntity(ContactDto contactDto)
    {
        return new Contact
        {
            FirstName = contactDto.FirstName,
            LastName = contactDto.LastName,
            PhoneNumber = contactDto.PhoneNumber,
            Email = contactDto.Email
        };
    }
}
