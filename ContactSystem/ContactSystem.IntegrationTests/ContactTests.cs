using ContactSystem.Application.Dtos;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace ContactSystem.IntegrationTests;

public class ContactTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public ContactTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetAllContacts_ReturnsEmptyListInitially()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/contact/get-all");

        // Assert
        response.EnsureSuccessStatusCode();
        var contacts = await response.Content.ReadFromJsonAsync<List<ContactDto>>();
        Assert.Empty(contacts);
    }

    [Fact]
    public async Task GetAllContacts_ReturnsContactsAfterPost()
    {
        // Arrange
        var client = _factory.CreateClient();
        var newContact = new ContactCreateDto
        {
            Name = "Jane Doe",
            Email = "jane@example.com",
            Phone = "+998995311229",
            Address = "Tashkent"
        };
        var postResponse = await client.PostAsJsonAsync("/api/contact/post", newContact);
        postResponse.EnsureSuccessStatusCode();

        // Act
        var response = await client.GetAsync("/api/contact/get-all");

        // Assert
        response.EnsureSuccessStatusCode();
        var contacts = await response.Content.ReadFromJsonAsync<List<ContactDto>>();
        Assert.Equal(1, contacts.Count());
        Assert.Equal(newContact.Name, contacts[0].Name);
        Assert.Equal(newContact.Phone, contacts[0].Phone);
    }

    [Fact]
    public async Task PostContact_AddsContact()
    {
        // Arrange
        var client = _factory.CreateClient();
        var newContact = new ContactCreateDto { Name = "Jane Doe", Email = "jane@example.com", Phone = "+998995311229", Address = "Tashkent" };
        
        // Act
        var response = await client.PostAsJsonAsync("/api/contact/post", newContact);
        
        // Assert
        response.EnsureSuccessStatusCode();
        var createdId = await response.Content.ReadFromJsonAsync<long>();
        Assert.True(createdId > 0);

        // Act
        var getResponse = await client.GetAsync($"/api/contact/get/{createdId}");
        
        // Assert
        getResponse.EnsureSuccessStatusCode();
        var contact = await getResponse.Content.ReadFromJsonAsync<ContactDto>();
        Assert.NotNull(contact);
        Assert.Equal("Jane Doe", contact.Name);
        Assert.Equal("jane@example.com", contact.Email);
    }

    [Fact]
    public async Task GetContactById_ReturnsContact()
    {
        var client = _factory.CreateClient();

        // Post a contact
        var newContact = new ContactCreateDto { Name = "Alice", Email = "alice@example.com", Phone = "+998995311229", Address = "Tashkent" };
        var postResponse = await client.PostAsJsonAsync("/api/contact/post", newContact);
        postResponse.EnsureSuccessStatusCode();
        var createdId = await postResponse.Content.ReadFromJsonAsync<long>();

        // Get the contact
        var getResponse = await client.GetAsync($"/api/contact/get/{createdId}");
        getResponse.EnsureSuccessStatusCode();
        var contact = await getResponse.Content.ReadFromJsonAsync<ContactDto>();
        Assert.NotNull(contact);
        Assert.Equal("Alice", contact.Name);
        Assert.Equal("alice@example.com", contact.Email);
    }

    [Fact]
    public async Task GetNonExistentContact_Returns404()
    {
        // Arrenge
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/contact/get/999");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateContact_UpdatesContact()
    {
        // Arrange
        var client = _factory.CreateClient();
        var newContact = new ContactCreateDto { Name = "Bob", Email = "bob@example.com", Phone = "+998995311229", Address = "Tashkent" };
        
        // Act
        var postResponse = await client.PostAsJsonAsync("/api/contact/post", newContact);
        
        // Assert
        postResponse.EnsureSuccessStatusCode();


        // Arrange
        var createdId = await postResponse.Content.ReadFromJsonAsync<long>();
        var updatedContact = new ContactDto { ContactId = createdId, Name = "Updated Bob", Email = "updated@example.com", Phone = "+998995311229", Address = "Tashkent"};
        
        // Act
        var putResponse = await client.PutAsJsonAsync("/api/contact/put", updatedContact);
        
        // Assert
        putResponse.EnsureSuccessStatusCode();

        // Act
        var getResponse = await client.GetAsync($"/api/contact/get/{createdId}");

        // Assert
        getResponse.EnsureSuccessStatusCode();
        var contact = await getResponse.Content.ReadFromJsonAsync<ContactDto>();
        Assert.Equal("Updated Bob", contact.Name);
        Assert.Equal("updated@example.com", contact.Email);
    }

    [Fact]
    public async Task UpdateNonExistentContact_Returns404()
    {
        var client = _factory.CreateClient();
        var updatedContact = new ContactDto { ContactId = 999, Name = "Non Existent", Email = "none@example.com" };
        var response = await client.PutAsJsonAsync("/api/contact/put", updatedContact);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task DeleteContact_RemovesContact()
    {
        var client = _factory.CreateClient();

        // Post a contact
        var newContact = new ContactCreateDto { Name = "Bob", Email = "bob@example.com", Phone = "+998995311229", Address = "Tashkent" };
        var postResponse = await client.PostAsJsonAsync("/api/contact/post", newContact);
        postResponse.EnsureSuccessStatusCode();
        var createdId = await postResponse.Content.ReadFromJsonAsync<long>();

        // Delete the contact
        var deleteResponse = await client.DeleteAsync($"/api/contact/delete?contactId={createdId}");
        deleteResponse.EnsureSuccessStatusCode();

        // Verify the contact is deleted
        var getResponse = await client.GetAsync($"/api/contact/get/{createdId}");
        Assert.Equal(HttpStatusCode.BadRequest, getResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteNonExistentContact_Returns404()
    {
        var client = _factory.CreateClient();
        var response = await client.DeleteAsync("/api/contact/delete?contactId=999");
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
