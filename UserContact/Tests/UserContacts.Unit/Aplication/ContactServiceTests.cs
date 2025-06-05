using FluentValidation;
using Moq;
using UserContacts.Application.Dtos.ContactDtos;
using UserContacts.Application.RepositoryInterfaces;
using UserContacts.Application.Services.ContactService;
using UserContacts.Application.Validators.ContactValidator;
using UserContacts.Core.Errors;
using UserContacts.Domain.Entities;
// TUnit

namespace UserContacts.Unit.Aplication;
public class ContactServiceTests
{
    private readonly ContactService ContactService;
    private readonly Mock<IContactRepository> ContactRepositoryMock;
    private readonly Mock<IUserRepository> UserRepositoryMock;
    //private readonly Mock<IValidator<ContactCreateDto>> ContactCreateValidatorMock;
    private readonly IValidator<ContactCreateDto> ContactCreateValidator;
    //private readonly Mock<IValidator<ContactDto>> ContactUpdateValidatorMock;
    private readonly IValidator<ContactDto> ContactUpdateValidator;

    public ContactServiceTests()
    {
        ContactRepositoryMock = new Mock<IContactRepository>();
        UserRepositoryMock = new Mock<IUserRepository>();
        //ContactCreateValidatorMock = new Mock<IValidator<ContactCreateDto>>();
        ContactCreateValidator = new ContactCreateValidator();
        ContactUpdateValidator = new ContactUpdateValidator();
        
        ContactService = new ContactService(
            ContactRepositoryMock.Object,
            UserRepositoryMock.Object,
            ContactCreateValidator,
            ContactUpdateValidator
        );
    }

    [Fact]
    public async Task DeleteContactAsync_ShouldCallRepository()
    {
        // Arrange
        var contact = new Contact { ContactId = 1, FirstName = "John Doe" };
        ContactRepositoryMock.Setup(r => r.SelectByIdAsync(1))
                             .ReturnsAsync(contact);

        ContactRepositoryMock.Setup(r => r.DeleteAsync(contact))
                             .Returns(Task.CompletedTask)
                             .Verifiable();



        // Act
        await ContactService.DeleteContactAsync(contact.ContactId);

        // Assert
        ContactRepositoryMock.Verify(r => r.DeleteAsync(contact), Times.Once);
    }

    [Fact]
    public async Task DeleteContactAsync_ShouldThrowException()
    {
        // Arrange
        Contact contact = null;
        ContactRepositoryMock.Setup(r => r.SelectByIdAsync(1))
                             .ReturnsAsync(contact);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => ContactService.DeleteContactAsync(1));
    }



    [Fact]
    public async Task GetContactByIdAsync_ShouldReturnContact()
    {
        // Arrange
        var contactId = 1L;
        var contact = new Contact { ContactId = contactId, FirstName = "Ali" };

        ContactRepositoryMock.Setup(r => r.SelectByIdAsync(contactId))
                              .ReturnsAsync(contact);

        // Act
        var result = await ContactService.GetContactByIdAsync(contactId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Ali", result.FirstName);
    }

    [Fact]
    public async Task GetAllContactsByUserAsync_ShouldReturnList()
    {
        // Arrange
        var userId = 5L;
        var contacts = new List<Contact>
        {
            new Contact { ContactId = 1, FirstName = "Ali" },
            new Contact { ContactId = 2, FirstName = "Vali" }
        };

        ContactRepositoryMock.Setup(r => r.SelectAllContactsAsync(userId, 0, 10))
                             .ReturnsAsync(contacts);

        // Act
        var result = await ContactService.GetAllContactsByUserAsync(userId, 0, 10);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task UpdateContactAsync_ShouldCallRepository()
    {
        // Arrange
        var dto = new ContactDto { ContactId = 1, FirstName = "Ali", PhoneNumber = "998901234567" };

        ContactRepositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Contact>()))
                              .Returns(Task.CompletedTask)
                              .Verifiable();

        var contactId = 1L;
        var contact = new Contact { ContactId = contactId, FirstName = "Ali" };

        ContactRepositoryMock.Setup(r => r.SelectByIdAsync(contactId))
                              .ReturnsAsync(contact);

        // Act
        await ContactService.UpdateContactAsync(dto);

        // Assert
        ContactRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Contact>()), Times.Once);
    }

    [Fact]
    public async Task UpdateContactAsync_ShouldThrowValidationException()
    {
        // Arrange
        var dto = new ContactDto { ContactId = 1, FirstName = "A", PhoneNumber = "998901234567" };

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => ContactService.UpdateContactAsync(dto));
    }

    [Fact]
    public async Task UpdateContactAsync_ShouldThrowException_WhenContactNull()
    {
        var dto = new ContactDto { ContactId = 1, FirstName = "Ali", PhoneNumber = "998901234567" };

        var contactId = 1L;
        Contact contact = null;

        ContactRepositoryMock.Setup(r => r.SelectByIdAsync(contactId))
                              .ReturnsAsync(contact);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => ContactService.UpdateContactAsync(dto));
    }

    [Fact]
    public async Task GetContactByPhoneNumber_ShouldReturnContact()
    {
        // Arrange
        var phone = "998901234567";
        var contact = new Contact { ContactId = 1, PhoneNumber = phone };

        ContactRepositoryMock.Setup(r => r.SelectContactByPhoneNumber(phone))
                              .ReturnsAsync(contact);

        // Act
        var result = await ContactService.GetContactByPhoneNumber(phone);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(phone, result.PhoneNumber);
    }

    [Fact]
    public async Task AddContactAsync_ShouldAddContact()
    {
        // Arrange
        long userId = 4;
        var dto = new ContactCreateDto
        {
            UserId = 4,
            FirstName = "Ali",
            PhoneNumber = "998901234567"
        };

        var user = new User()
        {
            UserId = 4
        };

        var contact = new Contact()
        {
            FirstName = "Test",
        };

        ContactRepositoryMock
            .Setup(x => x.InsertAsync(It.IsAny<Contact>()))
            .ReturnsAsync(4);

        UserRepositoryMock
            .Setup(r => r.SelectUserByIdAsync(userId))
            .ReturnsAsync(user);

        // Act
        var result = await ContactService.AddContactAsync(dto);

        // Assert
        Assert.Equal(4, result); 
    }
}

