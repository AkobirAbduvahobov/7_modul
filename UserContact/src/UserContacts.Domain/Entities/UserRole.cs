namespace UserContacts.Domain.Entities;
public class UserRole
{
    public int UserRoleId { get; set; } = default!;
    public string RoleName { get; set; }
    public string Description { get; set; }
    public long UserId { get; set; }
    public User User { get; set; }
}
