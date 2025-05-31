namespace UserContacts.Application.Dtos.ContactDtos;
public class ContactCreateDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string PhoneNumber { get; set; }
    public long UserId { get; set; }
}
