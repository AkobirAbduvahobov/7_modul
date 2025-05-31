using ContactSystem.Application.Dtos;
using ContactSystem.Application.Interfaces;
using ContactSystem.Application.Services;
using ContactSystem.Domain.Entities;
using Moq;
using Xunit;

namespace ContactSystem.Application.Tests;

public class ContactServiceTests
{
    private readonly Mock<IContactRepository> _mockRepo;
    private readonly ContactService _service;

    public ContactServiceTests()
    {
        _mockRepo = new Mock<IContactRepository>();
        _service = new ContactService(_mockRepo.Object);
    }

    [Fact]
    public async Task DeleteAsync_ContactExists_DeletesContact()
    {
        // Arrange
        var contactId = 1L;
        var contact = new Contact { ContactId = contactId };
        _mockRepo.Setup(r => r.SelectByIdAsync(contactId)).ReturnsAsync(contact);

        // Act
        await _service.DeleteAsync(contactId);

        // Assert
        _mockRepo.Verify(r => r.DeleteAsync(contact), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ContactNotFound_ThrowsException()
    {
        // Arrange
        var contactId = 1L;
        _mockRepo.Setup(r => r.SelectByIdAsync(contactId)).ReturnsAsync((Contact)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.DeleteAsync(contactId));
    }

    [Fact]
    public void GetAll_ReturnsMappedDtos()
    {
        // Arrange
        var contacts = new List<Contact>
        {
            new Contact { ContactId = 1, Name = "John" },
            new Contact { ContactId = 2, Name = "Jane" }
        };

        var expectedContacts = new List<ContactDto>
        {
            new ContactDto { ContactId = 1, Name = "John" },
            new ContactDto { ContactId = 2, Name = "Jane" }
        };


        _mockRepo.Setup(r => r.SelectAll()).Returns(contacts.AsQueryable());

        // Act
        var result = _service.GetAll();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(result, c => c.Name == "John");
        Assert.Contains(result, c => c.Name == "Jane");
    }

    [Fact]
    public async Task GetByIdAsync_ContactExists_ReturnsDto()
    {
        // Arrange
        var contactId = 1L;
        var contact = new Contact { ContactId = contactId, Name = "John" };
        _mockRepo.Setup(r => r.SelectByIdAsync(contactId)).ReturnsAsync(contact);

        // Act
        var result = await _service.GetByIdAsync(contactId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("John", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ContactNotFound_ThrowsException()
    {
        // Arrange
        var contactId = 1L;
        _mockRepo.Setup(r => r.SelectByIdAsync(contactId)).ReturnsAsync((Contact)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.GetByIdAsync(contactId));
    }

    [Fact]
    public async Task PostAsync_ValidContact_ReturnsId()
    {
        // Arrange
        var contactCreateDto = new ContactCreateDto { Name = "New Contact" };
        var contact = new Contact { ContactId = 123 };

        // Replace this call if using a custom MapService instead
        _mockRepo.Setup(r => r.InsertAsync(It.IsAny<Contact>())).ReturnsAsync(contact.ContactId);

        // Act
        var result = await _service.PostAsync(contactCreateDto);

        // Assert
        Assert.Equal(123, result);
    }

    [Fact]
    public async Task UpdateAsync_ContactExists_UpdatesContact()
    {
        // Arrange
        var dto = new ContactDto
        {
            ContactId = 1,
            Name = "Updated Name",
            Email = "new@email.com",
            Phone = "123",
            Address = "Address"
        };

        var contact = new Contact { ContactId = 1 };

        _mockRepo.Setup(r => r.SelectByIdAsync(dto.ContactId)).ReturnsAsync(contact);

        // Act
        await _service.UpdateAsync(dto);

        // Assert
        _mockRepo.Verify(r => r.UpdateAsync(contact), Times.Once);
        Assert.Equal("Updated Name", contact.Name);
    }

    [Fact]
    public async Task UpdateAsync_ContactNotFound_ThrowsException()
    {
        // Arrange
        var dto = new ContactDto { ContactId = 99 };
        _mockRepo.Setup(r => r.SelectByIdAsync(dto.ContactId)).ReturnsAsync((Contact)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(dto));
    }
}






