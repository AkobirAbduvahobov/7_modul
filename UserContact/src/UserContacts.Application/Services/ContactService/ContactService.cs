using FluentValidation;
using UserContacts.Application.Dtos.ContactDtos;
using UserContacts.Application.Mappers.ContactMapper;
using UserContacts.Application.RepositoryInterfaces;
using UserContacts.Core.Errors;
using UserContacts.Domain.Entities;

namespace UserContacts.Application.Services.ContactService;
public class ContactService : IContactService
{
    private readonly IContactRepository ContactRepository;
    private readonly IUserRepository UserRepository;
    private readonly IValidator<ContactCreateDto> ContactCreateValidator;
    private readonly IValidator<ContactDto> ContactUpdateValidator;
    public ContactService(IContactRepository contactRepository, IUserRepository userRepository, IValidator<ContactCreateDto> contactValidator, IValidator<ContactDto> contactUpdateValidator)
    {
        ContactRepository = contactRepository;
        UserRepository = userRepository;
        ContactCreateValidator = contactValidator;
        ContactUpdateValidator = contactUpdateValidator;
    }

    public async Task<long> AddContactAsync(ContactCreateDto contactCreateDto)
    {
        var validationResult = await ContactCreateValidator.ValidateAsync(contactCreateDto);
        if (!validationResult.IsValid)
        {   
            throw new ValidationException(validationResult.Errors);
        }

        var user = await UserRepository.SelectUserByIdAsync(contactCreateDto.UserId);
        if (user == null)
        {
            throw new EntityNotFoundException($"User with ID {contactCreateDto.UserId} not found.");
        }
            
        var contact = MapContact.ConvertToContact(contactCreateDto);

        var contactId = await ContactRepository.InsertAsync(contact);

        return contactId;
    }

    public async Task DeleteContactAsync(long contactId)
    {
        var contact = await ContactRepository.SelectByIdAsync(contactId);
        if (contact == null)
        {
            throw new EntityNotFoundException($"Contact with ID {contactId} not found.");
        }
        await ContactRepository.DeleteAsync(contact);
    }

    public async Task<ICollection<ContactDto>> GetAllContactsByUserAsync(long userId, int skip, int take)
    {
        var contacts = await ContactRepository.SelectAllContactsAsync(userId, skip, take);
        if (contacts == null || contacts.Count == 0)
        {
            throw new EntityNotFoundException($"No contacts found for user with ID {userId}.");
        }
        return contacts.Select(MapContact.ConvertToContactDto).ToList();
    }

    public async Task<Contact> GetContactByIdAsync(long contactId)
    {
        var contact = await ContactRepository.SelectByIdAsync(contactId);
        if (contact == null)
        {
            throw new EntityNotFoundException($"Contact with ID {contactId} not found.");
        }
        return contact;
    }

    public async Task<Contact?> GetContactByPhoneNumber(string phoneNumber)
    {
        var contact = await ContactRepository.SelectContactByPhoneNumber(phoneNumber);
        if (contact == null)
        {
            throw new EntityNotFoundException($"Contact with phone number {phoneNumber} not found.");
        }
        return contact;
    }

    public async Task UpdateContactAsync(ContactDto contactDto)
    {
        var validationResult = await ContactUpdateValidator.ValidateAsync(contactDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        var contact = await ContactRepository.SelectByIdAsync(contactDto.ContactId);
        if (contact == null)
        {
            throw new EntityNotFoundException($"Contact with ID {contactDto.ContactId} not found.");
        }

        MapContact.ConvertToContactEntity(contactDto);

        await ContactRepository.UpdateAsync(contact);
    }
}
