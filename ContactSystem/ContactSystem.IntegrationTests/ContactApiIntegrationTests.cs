using ContactSystem.Application.Dtos;
using ContactSystem.Server;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace ContactSystem.IntegrationTests;

public class ContactApiIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ContactApiIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllContacts_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/contact/get-all");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreateContact_ReturnsOkAndId()
    {
        var newContact = new
        {
            name = "John Doe",
            email = "john@example.com",
            phone = "123456789",
            address = "123 Main St"
        };

        var response = await _client.PostAsJsonAsync("/api/contact/post", newContact);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var id = await response.Content.ReadFromJsonAsync<long>();
        Assert.True(id > 0);
    }

    [Fact]
    public async Task GetContactById_ReturnsContact_WhenExists()
    {
        // First create a contact
        var createResponse = await _client.PostAsJsonAsync("/api/contact/post", new
        {
            name = "Jane Doe",
            email = "jane@example.com",
            phone = "987654321",
            address = "456 Elm St"
        });

        var contactId = await createResponse.Content.ReadFromJsonAsync<long>();

        var getResponse = await _client.GetAsync($"/api/contact/get{contactId}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var contact = await getResponse.Content.ReadFromJsonAsync<ContactDto>();
        Assert.Equal("Jane Doe", contact?.Name);
    }

    [Fact]
    public async Task UpdateContact_ReturnsOk()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/contact/post", new
        {
            name = "Initial",
            email = "initial@example.com",
            phone = "1111111111",
            address = "Nowhere"
        });
        var id = await createResponse.Content.ReadFromJsonAsync<long>();

        var updatedContact = new
        {
            contactId = id,
            name = "Updated Name",
            email = "updated@example.com",
            phone = "999999999",
            address = "Updated St"
        };

        var putResponse = await _client.PutAsJsonAsync("/api/contact/put", updatedContact);
        Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteContact_ReturnsOk_WhenExists()
    {
        var createResponse = await _client.PostAsJsonAsync("/api/contact/post", new
        {
            name = "To Delete",
            email = "delete@example.com",
            phone = "000000000",
            address = "Ghost Town"
        });
        var id = await createResponse.Content.ReadFromJsonAsync<long>();

        var deleteResponse = await _client.DeleteAsync($"/api/contact/delete?contactId={id}");
        Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
    }

    [Fact]
    public async Task GetContactById_ReturnsNotFound_WhenNotExists()
    {
        var response = await _client.GetAsync("/api/contact/get999999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}