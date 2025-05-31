using UserContacts.Domain.Entities;

namespace UserContacts.Application.RepositoryInterfaces;
public interface IContactRepository
{
    Task<long> InsertAsync(Contact contact);
    Task DeleteAsync(Contact contact);
    Task<Contact?> SelectByIdAsync(long contactId);
    Task<ICollection<Contact>> SelectAllContactsAsync(long userId, int skip, int take);
    Task UpdateAsync(Contact contact);
    Task<Contact?> SelectContactByPhoneNumber(string phoneNumber);
}
