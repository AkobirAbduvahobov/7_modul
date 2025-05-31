using FluentValidation;
using UserContacts.Application.Dtos.ContactDtos;

namespace UserContacts.Application.Validators.ContactValidator;
public class ContactCreateValidator : AbstractValidator<ContactCreateDto>
{
    public ContactCreateValidator()
    {
        RuleFor(x => x.FirstName)
            .Length(2, 50)
            .When(x => !string.IsNullOrWhiteSpace(x.FirstName))
            .WithMessage("FirstName must be between 2 and 50 characters long");

        RuleFor(x => x.LastName)
            .Length(2, 50)
            .When(x => !string.IsNullOrWhiteSpace(x.LastName))
            .WithMessage("LastName must be between 2 and 50 characters long");

        RuleFor(x => x.Email)
             .EmailAddress()
             .When(x => !string.IsNullOrWhiteSpace(x.Email))
             .WithMessage("Invalid email format");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("PhoneNumber is required")
            .Matches(@"^\+?[1-9]\d{1,14}$")
            .WithMessage("Invalid phone number format");
    }
}
