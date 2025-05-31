using Microsoft.EntityFrameworkCore;
using UserContacts.Application.RepositoryInterfaces;
using UserContacts.Core.Errors;
using UserContacts.Domain.Entities;

namespace UserContacts.Infrastructure.Persistence.Repositories;
public class ContactRepository : IContactRepository
{
    private readonly MyDbContext MyDbContext;

    public ContactRepository(MyDbContext myDbContext)
    {
        MyDbContext = myDbContext;
    }

    public async Task DeleteAsync(Contact contact)
    {
        MyDbContext.Contacts.Remove(contact);
        await MyDbContext.SaveChangesAsync();
    }

    public async Task<long> InsertAsync(Contact contact)
    {
        await MyDbContext.Contacts.AddAsync(contact);
        await MyDbContext.SaveChangesAsync();
        return contact.ContactId;
    }

    public async Task<ICollection<Contact>> SelectAllContactsAsync(long userId, int skip, int take)
    {
        return await MyDbContext.Contacts
             .Where(c => c.UserId == userId)
             .Skip(skip)
             .Take(take)
             .ToListAsync();
    }

    public async Task<Contact?> SelectByIdAsync(long contactId)
    {
        var user = await MyDbContext.Contacts
            .FirstOrDefaultAsync(c => c.ContactId == contactId);
        if (user == null)
        {
            throw new KeyNotFoundException($"Contact with ID {contactId} not found.");
        }
        return user;
    }
    public async Task UpdateAsync(Contact contact)
    {
        MyDbContext.Contacts.Update(contact);
        await MyDbContext.SaveChangesAsync();
    }
    public async Task<Contact?> SelectContactByPhoneNumber(string phoneNumber)
    {
        var contact = await MyDbContext.Contacts
            .FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber);
        if (contact == null)
        {
            throw new NotFoundException($"Contact with phone number {phoneNumber} not found");
        }
        return contact;
    }
}
