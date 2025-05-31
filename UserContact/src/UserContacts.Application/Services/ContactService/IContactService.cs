using UserContacts.Application.Dtos.ContactDtos;
using UserContacts.Domain.Entities;

namespace UserContacts.Application.Services.ContactService;

public interface IContactService
{
    Task<long> AddContactAsync(ContactCreateDto contactCreateDto);
    Task DeleteContactAsync(long contactId);
    Task<Contact> GetContactByIdAsync(long contactId);
    Task<ICollection<ContactDto>> GetAllContactsByUserAsync(long userId, int skip, int take);
    Task UpdateContactAsync(ContactDto contactDto);
    Task<Contact?> GetContactByPhoneNumber(string phoneNumber);
}