using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserContacts.Domain.Entities;

namespace UserContacts.Infrastructure.Persistence.Configurations;
public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.ToTable("Contacts");
        builder.HasKey(c => c.ContactId);

        builder.Property(c => c.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(c => c.LastName).IsRequired(false).HasMaxLength(50);

        builder.Property(c => c.Email).IsRequired(false).HasMaxLength(320);

        builder.Property(c => c.PhoneNumber).IsRequired().HasMaxLength(15);

        builder.Property(c => c.CreatedAt).IsRequired();

        builder.HasOne(c => c.User)
            .WithMany(u => u.Contacts)
            .HasForeignKey(c => c.UserId);
    }
}

